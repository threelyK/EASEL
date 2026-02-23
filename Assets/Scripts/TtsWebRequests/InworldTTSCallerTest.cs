using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TtsWebRequests
{
    public class InworldTtsCallerTest : MonoBehaviour
    {
        
        private const string TestMessage = "Hello world, I am James";
        [SerializeField]
        private AudioSource ttsAudioSourceTest;
        [SerializeField]
        private string apiKey;
        
        void Start()
        {
            RunTest();
        }

        private async UniTask RunTest()
        {
            var ttsCaller = new InworldTtsCaller(ttsAudioSourceTest, apiKey);
            
            await ttsCaller.PostAndPlayToInworldVoice(TestMessage);
        }
    }
}
