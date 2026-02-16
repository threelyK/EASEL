using System;
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

        private async void RunTest()
        {
            try
            {
                var ollamaRequester = new OllamaRequests(SystemPrompt);
            
                var response = await ollamaRequester.SendPrompt(Prompt);
                Debug.Log(response);
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }

    }
}
