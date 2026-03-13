#if UNITY_EDITOR

#region Usings
using UnityNeuroSpeech.Runtime;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using UnityNeuroSpeech.Runtime.Ollama;
using UnityNeuroSpeech.Runtime.JsonData;
using Whisper;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityNeuroSpeech.Runtime.ControllerModules;
using LogUtils = UnityNeuroSpeech.Utils.LogUtils;
#endregion

namespace UnityNeuroSpeech.Editor
{
    /// <summary>
    /// Base agent controller. This script gets duplicated and modified by CreateAgent.cs,
    /// but the core functionality stays unchanged
    /// </summary>
    internal sealed class BaseAgentController : MonoBehaviour, IAgent
    {
        #region Variables
        // General
        /// <summary>
        /// Generated ScriptableObject
        /// </summary>
        [Header("General")]
        public AgentSettings agentSettings;
        public string JsonDialogHistoryFileName { get; set; } = string.Empty;
        public string EncryptionHistoryKey { get; set; } = string.Empty;

        // STT
        [Header("Speech-To-Text")]
        [SerializeField] private WhisperManager _whisperManager;
        [SerializeField] private MicrophoneRecord _microphoneRecord;
        [SerializeField] private Button _micButton;
        [SerializeField] private Sprite _enableMicSprite, _disableMicSprite;
        public Action<string> AfterSTT { get; set; }
        private bool _processingOtherActions;

        // TTS
        [Header("TTS")]
        [SerializeField] private AudioSource _ttsAudioSource;
        public Action<AgentState> BeforeTTS { get; set; }
        public Action AfterTTS { get; set; }

        // Modules
        private ControllerOllamaModule _ollamaModule = new();
        private ControllerTTSModule _ttsModule;
        private ControllerJsonDataModule _jsonModule;
        #endregion
        
        // Custom
        private string _blank = "blank";
        private string _empty = "empty";
        
        #region Unity methods
        private void Start()
        {
            _jsonModule = new(JsonDialogHistoryFileName, EncryptionHistoryKey);
            var sharedSettings = _jsonModule.LoadSharedSettingsModular();
            if (!sharedSettings) return;

            LogUtils.logLevel = sharedSettings.Value.logLevel;

            _ttsModule = new(agentSettings.agentIndex, _ttsAudioSource);

            _ollamaModule.InitOllamaModular(agentSettings.systemPrompt);

            // Setting Whisper and UI
            _microphoneRecord.OnRecordStop += OnRecordStop;
            _micButton.onClick.AddListener(OnButtonPressed);
            _micButton.image.sprite = _disableMicSprite;
        }

        #endregion

        #region Main

        /// <summary>
        /// STT, Ollama, TTS - everything here
        /// </summary>
        private async UniTask MainCycle(AudioChunk recordedAudio)
        {
            // RadioActions.OnReadyReady?.Invoke(false);
            
            var whisperResult = await GetWhisperResult(recordedAudio);

            var whisperResultStr = whisperResult.Result;
            
            var invalidPrompt = (whisperResultStr.Contains(_blank) || whisperResultStr.Contains(_empty)) && whisperResultStr.Length < 11
                                || whisperResultStr.Equals("");

            if (invalidPrompt)
            {
                // RadioReady?.Invoke(true);
                return;
            }

            var llmResponse = await SendMessageToOllama(whisperResult.Result, whisperResult.Language, this.GetCancellationTokenOnDestroy());

#if ENABLE_MONO
            _ttsModule.StartTTSProcessMonoModular(llmResponse);
            // await MonitorTTSProcess(whisperResult.Language, ttsProcess);
#else
            await _ttsModule.HandleTTSProcessAndOutputIL2CPP(llmResponse, whisperResult.Language);

            LogUtils.LogMessage($"Invoking AfterTTS() for agent");
            AfterTTS?.Invoke();

            _processingOtherActions = false;
#endif
        }

        #endregion

        #region Ollama

        /// <summary>
        /// Sends message to Ollama and invokes BeforeTTS 
        /// </summary>
        /// <returns>Response from LLM</returns>
        private async UniTask<string> SendMessageToOllama(string userPrompt, string lang, CancellationToken token)
        {
            var ollamaResult = await _ollamaModule.SendMessageModular(userPrompt, lang, token);

            _jsonModule.UpdateJsonDialogHistoryModular(lastDialog: new(ollamaResult.userPrompt, ollamaResult.agentMessage));

            LogUtils.LogMessage($"Invoking BeforeTTS() for agent");
            BeforeTTS?.Invoke(ollamaResult);

            return ollamaResult.agentMessage;
        }
        #endregion

        #region STT

        private async UniTask<WhisperResult> GetWhisperResult(AudioChunk audio)
        {
            var whisperRes = await _whisperManager.GetTextAsync(audio.Data, audio.Frequency, audio.Channels);
            if (whisperRes == null)
            {
                LogUtils.LogError("Whisper error in text analysis!");
                return null;
            }

            LogUtils.LogMessage($"Invoking AfterSTT() for agent");
            AfterSTT?.Invoke(whisperRes.Result);

            return whisperRes;
        }

        // Whisper moment
        private async void OnRecordStop(AudioChunk recordedAudio) => await MainCycle(recordedAudio);

        private void OnButtonPressed()
        {
            if (_processingOtherActions) return;

            if (!_microphoneRecord.IsRecording)
            {
                _microphoneRecord.StartRecord();
                _micButton.image.sprite = _enableMicSprite;
            }
            else
            {
                _microphoneRecord.StopRecord();
                _micButton.image.sprite = _disableMicSprite;
                _processingOtherActions = true;
            }
        }

        #endregion

        #region TTS

#if ENABLE_MONO

        /// <summary>
        /// Monitors TTS process. When process exits, will play generated audio and then delete it.
        /// </summary>
        private async UniTask MonitorTTSProcess(string lang, Process ttsProcess)
        {
            await UniTask.SwitchToThreadPool();

            ttsProcess.WaitForExit();

            await UniTask.SwitchToMainThread();

            await _ttsModule.CheckTTSProcessMonoModular(lang, ttsProcess);

            LogUtils.LogMessage($"Invoking AfterTTS() for agent");
            AfterTTS?.Invoke();

            _processingOtherActions = false;
        }
#endif

        #endregion
    }
}
#endif //