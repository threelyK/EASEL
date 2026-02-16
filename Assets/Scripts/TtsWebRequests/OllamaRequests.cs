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
        public OllamaMessage[] messages;
        public bool stream;
        public bool think;
        public string keep_alive;
    }

    [Serializable]
    public class OllamaMessage
    {
        public string role;
        public string content;
    }
    
    // TODO: Find out if it remembers previous messages
    public class OllamaRequests
    {
        private const string IPAddress = "127.0.0.1";
        private const string OllamaEndpoint = "http://" + IPAddress + ":11434/api/chat";
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
            
            var response = postRequest.downloadHandler.text;
            return response;
        }

        private static UnityWebRequest CreatePostRequest(string prompt, string systemPrompt)
        {
            var userMessage = new OllamaMessage{role = "user", content = prompt};
            var systemMessage = new OllamaMessage{role = "system", content = systemPrompt};
            
            var jsonParams = JsonUtility.ToJson(new OllamaParams
            {
                model = Model,
                messages = new [] {userMessage, systemMessage},
                stream = Streaming,
                think = Thinking,
                keep_alive = KeepAlive
            });
            
            var request = UnityWebRequest.Post(OllamaEndpoint, jsonParams, contentType: "application/json");
            return request;
        }
        
    }
}
