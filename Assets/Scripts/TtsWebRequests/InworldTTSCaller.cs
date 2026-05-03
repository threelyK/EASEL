using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using PlayerInteraction;

namespace TtsWebRequests
{
    public class InworldTtsCaller
    {

        #region Request Parameters
        private const string URL = "https://api.inworld.ai/tts/v1/voice";
        private readonly string _apiKey;    
        
        // Request parameters
        private const string VoiceID = "Clive"; // Voice profile can be configured
        private const string ModelID = "inworld-tts-1.5-mini";
        private const string TextNormalization = "ON";
        private const string AudioEncoding = "LINEAR16"; // Linear16 = wav
        
        // Audio related, defaults are based on Linear16
        private const int SampleRate = 16000; // Difference above 16000 is not obvious
        private const int Channels = 1;
        private readonly AudioSource _ttsAudioSource;
        private const int HeaderOffset = 24; // 24 makes static stopped
        #endregion
        
        public InworldTtsCaller(AudioSource ttsAudioSource, string apiKey)
        {
            _ttsAudioSource = ttsAudioSource;
            _apiKey = apiKey;
        }


        public async UniTask PostAndPlayToInworldVoice(string text)
        {
            var response = await PostToInworldVoice(text);
            PlayVoiceClip(response);
        }
        
        private async UniTask<string> PostToInworldVoice(string text)
        {
            using var postRequest = CreatePostRequest(text);
            
            await postRequest.SendWebRequest();
            
            Debug.Log("Sent Request");
            // -> downloadHandler.text = audioContent{<BYTES>}

            if (postRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("TTS request failed: " + postRequest.error);
                Debug.LogError("Response: " + postRequest.downloadHandler.text);
                throw new Exception(postRequest.error);
            }
            
            Debug.Log("TTS Response OK");
            
            return postRequest.downloadHandler.text;
        }

        private byte[] DecodeBase64(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        private void PlayVoiceClip(String responseText)
        {
            // Response.data contains audioContent{...} in base64 contains "RIFF" header
            var responseJson = JsonUtility.FromJson<PostResponseJson>(responseText);
            // audioContent is encoded in base64
            var audioBytes = DecodeBase64(responseJson.audioContent);
            
            // Debug.Log("Response json/audioContent = " + responseJson.audioContent);
            var clip = ProcessLinear16Audio(audioBytes);
            
            _ttsAudioSource.clip = clip;
            RadioActions.OnClipGenerated?.Invoke(clip);
            _ttsAudioSource.Play();
        }

        private static float[] ConvertLinear16ToFloat(byte[] audioBytes)
        {
            // AI generated helper function
            
            var sampleCount = (audioBytes.Length / 2) - HeaderOffset;
            float[] samples = new float[sampleCount];
            
            for (int i = 0; i < sampleCount; i++)
            {
                int byteIndex = (i + HeaderOffset) * 2;
                var sample = (short)(audioBytes[i * 2] | (audioBytes[byteIndex + 1] << 8));
                samples[i] = sample / 32768f; // normalize to -1..1
            }

            return samples;
        }
        
        private static AudioClip ProcessLinear16Audio(byte[] audioBytes)
        {
            float[] samples = ConvertLinear16ToFloat(audioBytes);
            var sampleCount = samples.Length / Channels;
            
            var audioClip = AudioClip.Create(
                "Linear16Audio",
                sampleCount,
                Channels,
                SampleRate,
                false
            );
            
            audioClip.SetData(samples, 0);

            return audioClip;
        }
        

        private UnityWebRequest CreatePostRequest(string text)
        {
            if (_apiKey == null)
            {
                throw new Exception("API key not set");
            }
            
            var request = new UnityWebRequest(URL, "POST");
            request.SetRequestHeader("Authorization", "Basic " + _apiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            var jsonData = CreateBodyJson(text);
            
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);

            return request;
        }
        
        private static string CreateBodyJson(string text)
        {
            var jsonData = JsonUtility.ToJson(new PostRequestParams
            {
                text = text,
                voiceId = VoiceID,
                modelId = ModelID,
                audioConfig = new AudioConfig
                {
                    audioEncoding = AudioEncoding,
                    sampleRateHertz = SampleRate
                },
                applyTextNormalization = TextNormalization,
            });
            
            Debug.Log(jsonData);

            return jsonData;
        }
    }

    [Serializable]
    public class PostRequestParams
    {
        // typecase has to match inworld API
        public string text;
        public string voiceId;
        public string modelId;
        public AudioConfig audioConfig;
        public string applyTextNormalization;
    }

    [Serializable]
    public class AudioConfig
    {
        public string audioEncoding;
        public int sampleRateHertz;
    }

    [Serializable]
    public class PostResponseJson
    {
        public string audioContent;
    }
}

    
