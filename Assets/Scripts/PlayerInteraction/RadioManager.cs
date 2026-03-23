
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace PlayerInteraction
{
    public class RadioManager : MonoBehaviour
    {
        [SerializeField]
        private Material radioLED;

        [SerializeField]
        private TMP_Text subtitlesTextBox;

        private bool _isGrabbed;
        private bool _isTimerOn;
        private float _timer;
        private float _displayTime;

        [SerializeField] private GameObject _radio;
        [SerializeField] private GameObject _subtitleSnapLocation;
        [SerializeField] private GameObject _snapLocation;


        private void Start()
        {
            radioLED.SetColor("_EmissionColor", Color.green);
            HideSubtitles();
        }

        private void Update()
        {
            if (!_isTimerOn) return;

            _timer += Time.deltaTime;

            if (_timer % 2 <= 1)
            {
                subtitlesTextBox.transform.DOMove(_subtitleSnapLocation.transform.position, 1);
            }
            
            if (_timer >= _displayTime)
            {
                HideSubtitles();
                _isTimerOn = false;
            }
        }

        private void OnEnable()
        {
            RadioActions.OnRadioReady += HandleStatus;
            // RadioActions.OnClipGenerated += StartTimer;
            RadioActions.ResponseGenerated += DisplaySubtitles;
        }

        private void OnDisable()
        {
            RadioActions.OnRadioReady -= HandleStatus;
            // RadioActions.OnClipGenerated -= StartTimer;
            RadioActions.ResponseGenerated -= DisplaySubtitles;
        }

        private void HandleStatus(bool status)
        {
            radioLED.SetColor("_EmissionColor", status ? Color.green : Color.red);
        }

        private void DisplaySubtitles(string subtitles)
        {
            subtitlesTextBox.alpha = 1;
            subtitlesTextBox.text = subtitles;
        }

        private void HideSubtitles()
        {
            subtitlesTextBox.alpha = 0;
        }

        private void StartTimer(float duration)
        {
            _timer = 0;
            _isTimerOn = true;

            _displayTime = duration + 5f;
        }
    }
}
