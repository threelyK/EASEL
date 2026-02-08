using UnityEngine;

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

        private void RunTest()
        {
            var ttsCaller = new InworldTtsCaller(ttsAudioSourceTest, apiKey);
            
            var result = ttsCaller.PostAndPlayToInworldVoice(TestMessage);
            StartCoroutine(result);
        }
    }
}
