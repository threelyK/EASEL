using TtsWebRequests;
using UnityEngine;

public class AgentContextManager : MonoBehaviour
{
    public void AppendGameContext(string context)
    {
        MistralRequest.Instance.AppendToGameContext(context);
    }
}
