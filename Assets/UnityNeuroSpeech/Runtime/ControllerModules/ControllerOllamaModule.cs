using Cysharp.Threading.Tasks;
using System.Threading;
using UnityNeuroSpeech.Utils;
using TtsWebRequests;

namespace UnityNeuroSpeech.Runtime.Ollama
{
    internal class ControllerOllamaModule
    {
        private int _responseCount;
        private OllamaRequests _ollamaRequester;
        
        // TODO: Keeping chatlogs for test purposes?
        
        public void InitOllamaModular(string systemPrompt)
        {
            // Init ollama client
            _ollamaRequester = new OllamaRequests(systemPrompt);
        }

        public async UniTask<AgentState> SendMessageModular(string userPrompt, string lang, CancellationToken token)
        {
            LogUtils.LogMessage("Sending message to Ollama...");

            var chatResponse = await _ollamaRequester.SendPrompt(userPrompt);
            
            LogUtils.LogMessage($"Ollama response: {chatResponse}");
            _responseCount++;

            return new(_responseCount, chatResponse, userPrompt, null, null);
        }
    }
}