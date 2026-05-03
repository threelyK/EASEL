using System;
using UnityEngine.Networking;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TtsWebRequests
{
    [Serializable]
    public class OllamaParams
    {
        public string model;
        public string prompt;
        public string system;
        public bool stream;
        public bool think;
        public string keep_alive;
    }

    [Serializable]
    public class LLMResponseObject
    {
        public string response;
    }
    
    public class OllamaRequests
    {
        private const string IPAddress = "127.0.0.1";
        private const string OllamaEndpoint = "http://" + IPAddress + ":11434/api/generate";
        private const string Model = "llama3.2:3b";
        
        private const bool Streaming = false;
        private const bool Thinking = false;
        private const string KeepAlive = "20m";
        private readonly string _systemPrompt;

        public OllamaRequests(string systemPrompt)
        {
            _systemPrompt = systemPrompt;
        }
        
        public async UniTask<string> SendPrompt(string prompt)
        {
            // String returned is a json object
            var postRequest = CreatePostRequest(prompt, _systemPrompt);
            await postRequest.SendWebRequest();
            
            var responseJson = JsonUtility.FromJson<LLMResponseObject>(postRequest.downloadHandler.text);
            return responseJson.response;
        }

        private static UnityWebRequest CreatePostRequest(string userPrompt, string systemPrompt)
        {
            var jsonParams = JsonUtility.ToJson(new OllamaParams
            {
                model = Model,
                prompt = userPrompt,
                system = systemPrompt,
                stream = Streaming,
                think = Thinking,
                keep_alive = KeepAlive
            });
            
            var request = UnityWebRequest.Post(OllamaEndpoint, jsonParams, contentType: "application/json");
            return request;
        }
        
    }
}
