
using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

        var id_1 = Random.Range(0, 255);
        var id_2 = Random.Range(0, 255);
        var id_3 = Random.Range(0, 255);
        var id_4 = Random.Range(0, 255);
        
        
        var randVal = Random.value;
        if (randVal <= 0.5)
        {
            _enableEmbodiedFeatures = true;
            SessionID = $"{id_1}_{id_2}_{id_3}_{id_4}AI_ID_";
        }
        else
        {
            _enableEmbodiedFeatures = false;
            SessionID = $"{id_1}_{id_2}_{id_3}_{id_4}ID_";
        }
        
        Debug.LogWarning("SessionID: " + SessionID);

    }
    
    private void Start()
    {
        if (_embodiedFeatures) _embodiedFeatures.SetActive(false); else Debug.LogError("missing Embodied");
        if (_radio) _radio.SetActive(false); else Debug.LogError("missing Radio");
        
        if (SessionID is null)
        {
            Debug.LogError("SessionID not set or is TEST");
        }

        if (_enableEmbodiedFeatures) LoadAgenticFeatures();
    }
    
    private void LoadAgenticFeatures()
    {
        _embodiedFeatures.SetActive(true);
        _radio.SetActive(true);
    }
}
