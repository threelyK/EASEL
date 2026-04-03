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

    private void UpdateGameContext(int id)
    {
        var newContext = _sysPrompts[id];
        _mistralRequest.AppendToGameContext(newContext);
    }
    
    
}
