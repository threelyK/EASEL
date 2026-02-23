using System;
using System.Collections;
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
        private const string VoiceID = "Clive"; // Change to other voices maybe
        private const string ModelID = "inworld-tts-1.5-mini";
        private const string TextNormalization = "ON";
        private const string AudioEncoding = "LINEAR16"; // Linear16 = wav
        
        // Audio related, defaults are based on Linear16
        private const int SampleRate = 16000; // Difference above 16000 is not obvious
        private const int Channels = 1;
        private readonly AudioSource _ttsAudioSource;
        private const int HeaderOffset = 24; // idk why it's 24 but the static stopped
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
        
        private async UniTask<DownloadHandler> PostToInworldVoice(string text)
        {
            Debug.Log("Started Post");
            var postRequest = CreatePostRequest(text);
            await postRequest.SendWebRequest();
            // -> downloadHandler.text = audioContent{<BYTES>}

            try
            {
                Debug.Log(postRequest.result is UnityWebRequest.Result.ConnectionError
                    or UnityWebRequest.Result.ProtocolError
                    ? postRequest.error
                    : postRequest.downloadHandler.text);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            
            var response = postRequest.downloadHandler;
            return response;
        }


        private void PlayVoiceClip(DownloadHandler response)
        {
            // Response.data contains audioContent{...} in base64 contains "RIFF" header
            var responseJson = JsonUtility.FromJson<PostResponseJson>(response.text);
            // audioContent is encoded in base64
            var audioBytes = Convert.FromBase64String(responseJson.audioContent);
            
            // Debug.Log("Response json/audioContent = " + responseJson.audioContent);
            var clip = ProcessLinear16Audio(audioBytes);
            
            _ttsAudioSource.clip = clip;
            _ttsAudioSource.Play();
        }

        private static float[] ConvertLinear16ToFloat(byte[] audioBytes)
        {
            // AI generated helper function
            
            var sampleCount = audioBytes.Length / 2 - HeaderOffset;
            float[] samples = new float[sampleCount];

            for (int i = HeaderOffset+1; i < sampleCount; i++)
            {
                var sample = (short)(audioBytes[i * 2] | (audioBytes[i * 2 + 1] << 8));
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
            
            // Debug.Log(jsonData);

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
        // Try setting sample rate to be lower
    }

    [Serializable]
    public class PostResponseJson
    {
        public string audioContent;
    }
}

    
