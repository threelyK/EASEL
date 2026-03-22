
using System;
using UnityEngine;

public class MasterManager : Singleton<MasterManager>
{
    public string SessionID;
    public int visualScore;
    public int auralScore;
    public int rwScore;
    public int kinesScore;

    [SerializeField] private bool _enableAgenticFeatures;

    public static Action OnAgenticLoad;
    
    private void Start()
    {
        if (SessionID is null)
        {
            Debug.LogError("SessionID not set or is TEST");
        }
        
        Debug.LogWarning("SessionID: " + SessionID);
        
        if (_enableAgenticFeatures) OnAgenticLoad?.Invoke();
    }

    // TODO:
    private void LoadAgenticFeatures()
    {
        // Display Questionnaire
        
        // Activate agent
        
        // Spawn Radio
        
    }
}
