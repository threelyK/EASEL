using System;
using TMPro;
using TtsWebRequests;
using UnityEngine;

public class Subtitles : MonoBehaviour
{
    // Features
    // - Text contrast with text border.
    // - Scrolling up after reaching max height -- Possibly using a mask layer
    // - Subtitle follows player camera w/ smoothing
    // - Don't cover entire screen
    // - Every or every other sentence = new subtitle
    
    [SerializeField] private float fadeOutDuration;

    private float _maxBoxHeight;
    [SerializeField] private TMP_Text _subtitleBox;

    private string _subtitle;
    // private string _subtitleSpoken; // The subtitle which audio has been generated for
    private AudioClip _audioClip;
    private float _audioClipLength;
    
    private void OnEnable()
    {
        RadioActions.ResponseGenerated += SetSubtitleContent;
        RadioActions.OnClipGenerated += UpdateClipData;
    }

    private void OnDisable()
    {
        RadioActions.ResponseGenerated -= SetSubtitleContent;
        RadioActions.OnClipGenerated -= UpdateClipData;
    }

    private void SetSubtitleContent(string newContent)
    {
        _subtitle = newContent;
    }

    private void UpdateClipData(AudioClip clip)
    {
        _audioClip = clip;
        _audioClipLength = clip.length + fadeOutDuration; // with fade out
    }
}
