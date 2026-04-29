using UnityEngine;

public class MasterManagerV2 : Singleton<MasterManagerV2>
{
    public string sessionID;

    [SerializeField] private GameObject embodiedAgent;
    [SerializeField] private bool enableEmbodied;
    
    private void Awake()
    {
        base.Awake();
        var id1 = Random.Range(0, 255);
        var id2 = Random.Range(0, 255);
        var id3 = Random.Range(0, 255);
        var id4 = Random.Range(0, 255);
        
        Debug.LogWarning($"Session ID: {sessionID}");
    }

    private void Start()
    {
        if (enableEmbodied)
        {
            embodiedAgent.SetActive(true);
        }
    }
    
    public void EnableAgent()
    {
        embodiedAgent.SetActive(true);
    }
}
