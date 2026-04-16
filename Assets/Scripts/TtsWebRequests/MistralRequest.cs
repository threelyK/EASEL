using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TtsWebRequests
{
    public class MistralRequest
    {
        public static Action<string> OnResponseReceived;
        
        private static string _endpoint = "https://api.mistral.ai/v1/chat/completions";
        private static string _model = "mistral-small-2603";
        private static string _reasoningEffort = "none";
        private static string _apiKey;


        private string _personality = "You are a narrator in a game where your overall aim is to teach the player " +
                                      "agentic AI. You should try teaching using subtle reflective questioning. Do " +
                                      "NOT include stage directions";

        private string _gameContext = "Game: The player has just witnessed a robot demonstrate an agentic ai trait of " +
                                     "self-organisation by watching them arranges boxes to move to the button that let" +
                                     " the player enter the next room.";

        private List<message> _chatHistory; // Need to make sure we stay within context length
        private message _lastSystemPrompt;

        public string _systemPrompt { get; set; }

        public static MistralRequest Instance { get; } = new MistralRequest();

        private MistralRequest()
        {
            _chatHistory = new List<message>();
            _apiKey = Environment.GetEnvironmentVariable("MISTRAL_API_KEY");

            if (string.IsNullOrEmpty(_apiKey))
            {
                Debug.LogError("MISTRAL_API_KEY environment variable not set");
            }

            SetupSystem();
        }

        private void SetupSystem()
        {
            // Sets up the system parts of the chat history.
            AddSystemPrompt(_personality); // index 0
            AddSystemPrompt(_gameContext); // index 1
        }

        private void AddSystemPrompt(string message)
        {
            _chatHistory.Add(new message("system", message));
        }

        private void SetGameContext(string message)
        {
            _chatHistory[1].content = message;
        }

        /// <summary>
        /// Used to update the agent's knowledge of the robot and lessons the player has learnt.
        /// </summary>
        /// <param name="message">The string to append</param>
        public void AppendToGameContext(string message)
        {
            _chatHistory[1].content += " " + message;
        }
        

        private UnityWebRequest CreatePostReq(string userPrompt)
        {
            _systemPrompt ??= "";
            
            message UserMessage = new message("user", userPrompt);
            _chatHistory.Add(UserMessage);
            
            var jsonReqBody = JsonUtility.ToJson(new MistralReqBody
            {
                messages = _chatHistory,
                model =  _model,
                reasoning_effort = _reasoningEffort
            });
            
            
            Debug.Log($"Req body = {jsonReqBody}");
            
            var request = new UnityWebRequest(_endpoint, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonReqBody);
            
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {_apiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            
            return request;
        }

        public async UniTask<string> SendPrompt(string userPrompt)
        {
            var postRequest = CreatePostReq(userPrompt);
            await postRequest.SendWebRequest();
            
            var responseJson = JsonUtility.FromJson<MistralResponseObject>(postRequest.downloadHandler.text);
            
            Debug.Log($"Response json: {responseJson}");

            var responseContent = responseJson.choices[0].message.content;
            _chatHistory.Add( new message("assistant", responseContent));
            
            RadioActions.ResponseGenerated?.Invoke(responseContent);
            return responseContent;
        }
        
    
    }

    [Serializable]
    public class MistralReqBody
    {
        public List<message> messages;
        public string model;
        public string reasoning_effort;
    }
    
    [Serializable]
    public class MistralResponseObject
    {
        public ChatCompletionChoice[] choices;
    }

    [Serializable]
    public class message
    {
        public string role;
        public string content;

        public message(string role, string content)
        {
            this.role = role;
            this.content = content;
        }
    }
    
    [Serializable]
    public class ChatCompletionChoice
    {
        public message message;
    }
}
