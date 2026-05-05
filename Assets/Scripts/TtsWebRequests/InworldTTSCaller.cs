using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

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

        private void PlayVoiceClip(String responseText)
        {
            // Response.data contains audioContent{...} in base64 contains "RIFF" header
            var responseJson = JsonUtility.FromJson<PostResponseJson>(responseText);
            // audioContent is encoded in base64
            var audioBytes = Convert.FromBase64String(responseJson.audioContent);
            
            // Debug.Log("Response json/audioContent = " + responseJson.audioContent);
            AudioClip clip = ConvertBytesToFloatToClip(audioBytes);

            Debug.LogWarning($"Clip length (s) = {clip.length}");
            
            _ttsAudioSource.clip = clip;
            RadioActions.OnClipGenerated?.Invoke(clip);
            _ttsAudioSource.Play();
        }

        private static AudioClip ConvertBytesToFloatToClip(byte[] audioBytes)
        { 
            int offset = FindDataOffset(audioBytes); // Skipping past headers and finding data start
            
            // Audio bytes are PCM16 type = 16 bits = 2 bytes per sample
            var dataBytes = audioBytes.Length -  offset;
            int sampleCount = dataBytes / 2; // Number of 16-bit PCM samples in the original dataBytes
            
            float[] samples = new float[sampleCount]; // Just the data no header

            for (int i = 0; i < sampleCount; i++)
            {
                int byteIndex = offset + i * 2; // 2 because each sample is represented by 2 bytes
                
                short pcm = BitConverter.ToInt16(audioBytes, byteIndex);
                samples[i] = pcm / 32768f; // 32768 = max value of int16 --> normalizing to float between -1 to 1
            }
            
            var audioClip = AudioClip.Create(
                "Linear16Audio",
                audioBytes.Length,
                Channels,
                SampleRate,
                false
            );

            audioClip.SetData(samples, 0);
            return audioClip;
        }

        private static int FindDataOffset(byte[] audioBytes)
        {
            // Finding where the data begins
            
            for (var i = 12; i < audioBytes.Length; i++)
            {
                if (audioBytes[i] == 'd' && 
                    audioBytes[i + 1] == 'a' && 
                    audioBytes[i + 2] == 't' &&
                    audioBytes[i + 3] == 'a')
                {
                    return i + 8; // skips the headers "data"
                }
            }

            return 44;
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

    
