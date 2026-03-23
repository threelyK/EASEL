
using System;
using UnityEngine;

public class MasterManager : Singleton<MasterManager>
{
    public string SessionID;
    public int visualScore;
    public int auralScore;
    public int rwScore;
    public int kinesScore;

    [SerializeField] private GameObject _embodiedFeatures;
    [SerializeField] private GameObject _radio;

    [SerializeField] private bool _enableEmbodiedFeatures;

    public static Action OnAgenticLoad;
    public static Action OnVARKScoreChanged;
    
    private void Start()
    {
        _embodiedFeatures.SetActive(false);
        _radio.SetActive(false);
        
        if (SessionID is null)
        {
            Debug.LogError("SessionID not set or is TEST");
        }
        
        Debug.LogWarning("SessionID: " + SessionID);

        if (_enableEmbodiedFeatures) LoadAgenticFeatures();
    }

    // TODO:
    private void LoadAgenticFeatures()
    {
        _embodiedFeatures.SetActive(true);
        _radio.SetActive(true);
        
        // Display Questionnaire
        
        // Activate agent
        
        // Spawn Radio
        
    }
}
