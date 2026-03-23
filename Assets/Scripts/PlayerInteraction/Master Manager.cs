
using System;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    public string SessionID;
    public int visualScore;
    public int auralScore;
    public int rwScore;
    public int kinesScore;
    public string bestVARK;

    private static MasterManager _instance;
    public static MasterManager Instance => _instance;
    
    [SerializeField] private GameObject _embodiedFeatures;
    [SerializeField] private GameObject _radio;

    [SerializeField] private bool _enableEmbodiedFeatures;

    public static Action OnAgenticLoad;
    public static Action OnVARKScoreChanged;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    
    private void Start()
    {
        if (_embodiedFeatures) _embodiedFeatures.SetActive(true); else Debug.LogError("missing Embodied");
        if (_radio) _radio.SetActive(false); else Debug.LogError("missing Radio");
        
        if (SessionID is null)
        {
            Debug.LogError("SessionID not set or is TEST");
        }
        
        Debug.LogWarning("SessionID: " + SessionID);

        if (_enableEmbodiedFeatures) LoadAgenticFeatures();
    }
    
    private void LoadAgenticFeatures()
    {
        _embodiedFeatures.SetActive(true);
        // Display Questionnaire
        // Activate agent
        
        _radio.SetActive(true);
        

        
        // Spawn Radio
        
    }
}
