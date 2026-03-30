using TtsWebRequests;
using UnityEngine;

public class MistralRequesterManager : MonoBehaviour
{
    private MistralRequest _mistralRequest;
    private string[] _sysPrompts;
    
    private void Awake()
    {
        _mistralRequest = MistralRequest.Instance;
    }

    public void SetSystemPrompt(string newPrompt)
    {
        _mistralRequest._systemPrompt = newPrompt;
    }
    
}
