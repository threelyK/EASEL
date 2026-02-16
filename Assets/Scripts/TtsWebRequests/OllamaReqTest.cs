using UnityEngine;

namespace TtsWebRequests
{
    public class OllamaReqTest : MonoBehaviour
    {
        private const string Prompt = "1+1";
        private const string SystemPrompt = "Answer in German";

        void Start()
        {
            RunTest();
        }

        private void RunTest()
        {
            var ollamaRequester = new OllamaRequests(SystemPrompt);
            
            var response = ollamaRequester.SendPrompt(Prompt);
            Debug.Log(response);
        }

    }
}
