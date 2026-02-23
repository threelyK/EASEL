using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityNeuroSpeech.Utils;
using TtsWebRequests;

namespace UnityNeuroSpeech.Runtime.ControllerModules
{
    internal class ControllerTTSModule
    {
        private AudioSource _ttsAudioSource;
        private int _agentIndex;
        // TODO: fix unsafe exposed API key!!!
        private const string APIKey = "U1F6R2lpT1VWQVcxSWlZUENIUEx3QW5SbnpxV1VFOHg6MGVINTJ6YkppTmNEMWFmSjYyeUtZeTF6eVRLNFpreE5mc3NyQzVjaTgya1k3RmRrV2lQWHBsRjhhelI2Q0Z2VQ==";

        public ControllerTTSModule(int agentIndex, AudioSource ttsAudioSource)
        {
            _agentIndex = agentIndex;
            _ttsAudioSource = ttsAudioSource;
            
        }

        /// <summary>
        /// Starts TTS process in Mono
        /// </summary>
        public async UniTask StartTTSProcessMonoModular(string llmResponse)
        {
            llmResponse = llmResponse.Replace("\r", "").Replace("\n", " ").Trim();

            var ttsCaller = new InworldTtsCaller(_ttsAudioSource, APIKey);
            await ttsCaller.PostAndPlayToInworldVoice(llmResponse);
        }

        public async UniTask CheckTTSProcessMonoModular(string currentLang, Process currentProcess)
        {
            if (currentProcess != null && currentProcess.HasExited) await CheckIfResultFileExits(currentLang);
        }

        private async UniTask CheckIfResultFileExits(string currentLang) 
        { 
            if (File.Exists(Path.Combine(StaticData.GENERATED_VOICES_PATH, $"{currentLang}_voice{_agentIndex}_result.wav"))) 
            { 
                LogUtils.LogMessage("Generated wav result exists!"); 
                await PlayGeneratedAudioModular(absPath: Path.Combine(StaticData.GENERATED_VOICES_PATH, $"{currentLang}_voice{_agentIndex}_result.wav")); } 
        }
        private async UniTask PlayGeneratedAudioModular(string absPath)
        {
            var url = "file:///" + absPath.Replace("\\", "/");
            using (var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var clip = DownloadHandlerAudioClip.GetContent(request);
                    _ttsAudioSource.clip = clip;
                    _ttsAudioSource.Play();

                    await UniTask.Delay(TimeSpan.FromSeconds(clip.length + 2));
                    if (File.Exists(absPath))
                    {
                        File.Delete(absPath);       
                        LogUtils.LogMessage($"File at path {absPath} deleted succesfully!");
                    }
                }
                else LogUtils.LogError($"Error loading generated audio! Full error message: {request.error}");
            }
        }
    }
}