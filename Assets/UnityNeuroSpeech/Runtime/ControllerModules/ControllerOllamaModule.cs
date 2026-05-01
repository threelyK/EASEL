using Cysharp.Threading.Tasks;
using System.Threading;
using UnityNeuroSpeech.Utils;
using TtsWebRequests;

namespace UnityNeuroSpeech.Runtime.Ollama
{
    internal class ControllerOllamaModule
    {
        private int _responseCount;
        // private OllamaRequests _ollamaRequester;
        private MistralRequest _mistralRequester;
        
        public void InitOllamaModular(string systemPrompt)
        {
            // _ollamaRequester = new OllamaRequests(systemPrompt);
            
            // Init mistral client
            _mistralRequester = MistralRequest.Instance;
        }

        public async UniTask<AgentState> SendMessageModular(string userPrompt, string lang, CancellationToken token)
        {
            LogUtils.LogMessage("Sending message to LLM...");

            var mistralResponse = await _mistralRequester.SendPrompt(userPrompt);
            
            // var chatResponse = await _ollamaRequester.SendPrompt(userPrompt);
            
            LogUtils.LogMessage($"LLM response: {mistralResponse}");
            _responseCount++;

            return new(_responseCount, mistralResponse, userPrompt, null, null);
        }
    }
}