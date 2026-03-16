using TMPro;
using UnityEngine;

namespace PlayerInteraction
{
    public class RadioManager : MonoBehaviour
    {
        [SerializeField]
        private Material radioLED;

        [SerializeField]
        private TMP_Text subtitlesTextBox;


        private void Start()
        {
            radioLED.SetColor("_EmissionColor", Color.green);
        }

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
            radioLED.SetColor("_EmissionColor", status ? Color.green : Color.red);
        }

        private void DisplaySubtitles(string subtitles)
        {
            subtitlesTextBox.text = subtitles;
            // TODO: remove subtitles after certain time based on audio length
        }
    }
}
