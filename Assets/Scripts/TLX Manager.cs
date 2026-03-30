using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class TLXManager : MonoBehaviour
{
    [SerializeField] private Slider mentalSlider;
    [SerializeField] private Slider physicalSlider;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Slider performanceSlider;
    [SerializeField] private Slider effortSlider;
    [SerializeField] private Slider frustrationSlider;

    private string _sessionID;

    private float mentalRating;
    private float physicalRating;
    private float timeRating;
    private float performanceRating;
    private float effortRating;
    private float frustrationRating;
    

    private void Start()
    {
        _sessionID = MasterManager.Instance.SessionID;
    }

    private void OnEnable()
    {
        mentalSlider.onValueChanged.AddListener(UpdateMental);
        physicalSlider.onValueChanged.AddListener(UpdatePhysical);
        timeSlider.onValueChanged.AddListener(UpdateTime);
        performanceSlider.onValueChanged.AddListener(UpdatePerformance);
        effortSlider.onValueChanged.AddListener(UpdateEffort);
        frustrationSlider.onValueChanged.AddListener(UpdateFrustration);
    }

    private void OnDisable()
    {
        mentalSlider.onValueChanged.RemoveListener(UpdateMental);
        physicalSlider.onValueChanged.RemoveListener(UpdatePhysical);
        timeSlider.onValueChanged.RemoveListener(UpdateTime);
        performanceSlider.onValueChanged.RemoveListener(UpdatePerformance);
        effortSlider.onValueChanged.RemoveListener(UpdateEffort);
        frustrationSlider.onValueChanged.RemoveListener(UpdateFrustration);
    }


    private void UpdateMental(float value)
    {
        mentalRating = value;
    }
    private void UpdatePhysical(float value)
    {
        physicalRating = value;
    }
    private void UpdateTime(float value)
    {
        timeRating = value;
    }
    private void UpdatePerformance(float value)
    {
        performanceRating = value;
    }
    private void UpdateEffort(float value)
    {
        effortRating = value;
    }
    private void UpdateFrustration(float value)
    {
        frustrationRating = value;
    }

    public void ConfirmAnswers()
    {
        SaveTLX();
        Destroy(gameObject);
    }
    
    // Call on confirm button press (RED)
    private void SaveTLX()
    {
        var savePath = Path.Combine(Application.persistentDataPath, $"{_sessionID}_TLX_Save.json");

        var saveData = new []
        {
            mentalRating,
            physicalRating,
            timeRating,
            performanceRating,
            effortRating,
            frustrationRating,
        };
            
        var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            
        File.WriteAllText(savePath, json);
        Debug.LogWarning($"TLX data saved at: {savePath}");
    }
}
