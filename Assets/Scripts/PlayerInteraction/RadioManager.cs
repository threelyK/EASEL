using UnityEngine;
using TMPro;

public class RadioManager : MonoBehaviour
{
    [SerializeField]
    private Material radioLED;

    [SerializeField]
    private TMP_Text subtitlesTextBox;
    
    private void OnEnable()
    {
         RadioActions.OnRadioReady += HandleStatus;
         RadioActions.ResponseGenerated += DisplaySubtitles;
    }

    private void OnDisable()
    {
        RadioActions.OnRadioReady -= HandleStatus;
    }

    private void HandleStatus(bool status)
    {
        radioLED.color = status ? Color.green : Color.red;
    }

    private void DisplaySubtitles(string subtitles)
    {
        subtitlesTextBox.text = subtitles;
        // TODO: remove subtitles after certain time based on audio length
    }
}
