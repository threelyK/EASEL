

#region Usings

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
using TtsWebRequests;
using PlayerInteraction;

#endregion

namespace UnityNeuroSpeech.Runtime
{
    /// <summary>
    /// James controller
    
    /// </summary>
    public sealed class JamesController : MonoBehaviour, IAgent
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
        // [SerializeField] private Button _micButton;
        // [SerializeField] private Sprite _enableMicSprite, _disableMicSprite;
        public Action<string> AfterSTT { get; set; }
        private bool _processingOtherActions;

        // TTS
        [Header("TTS")]
        [SerializeField] private AudioSource _ttsAudioSource;

        private readonly string _apiKey = Environment.GetEnvironmentVariable("INWORLD_API_KEY");
        public Action<AgentState> BeforeTTS { get; set; }
        public Action AfterTTS { get; set; }

        // Modules
        private ControllerOllamaModule _ollamaModule = new();
        private ControllerTTSModule _ttsModule;
        private ControllerJsonDataModule _jsonModule;
        #endregion

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
        }

        private void OnEnable()
        {
            RadioCollider.OnPlayerTalking += OnButtonPressed;
            RadioCollider.OnPlayerStoppedTalking += OnButtonRelease;
        }
        
        private void OnDisable() {
            RadioCollider.OnPlayerTalking -= OnButtonPressed;
            RadioCollider.OnPlayerStoppedTalking -= OnButtonRelease;
        }

        #endregion

        #region Main

        /// <summary>
        /// STT, Ollama, TTS - everything here
        /// </summary>
        private async UniTask MainCycle(AudioChunk recordedAudio)
        {
            var whisperResult = await GetWhisperResult(recordedAudio);

            var llmResponse = await SendMessageToOllama(whisperResult.Result, whisperResult.Language, this.GetCancellationTokenOnDestroy());

            var ttsCaller = new InworldTtsCaller(_ttsAudioSource, _apiKey);

            await ttsCaller.PostAndPlayToInworldVoice(llmResponse);
            AfterTTS?.Invoke();

            _processingOtherActions = false;
            
            // Don't need ttsModule
            // StartCoroutine(_ttsModule.StartTTSProcessMonoModular(llmResponse));
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
                // _micButton.image.sprite = _enableMicSprite;
            }
        }

        private void OnButtonRelease()
        {
            if (_processingOtherActions) return;
            if (!_microphoneRecord.IsRecording) return;
            
            _microphoneRecord.StopRecord();
            _processingOtherActions = true;
            // _micButton.image.sprite = _disableMicSprite;
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
