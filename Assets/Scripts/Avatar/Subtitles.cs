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
    // - Maybe split the subtitle per sentence. New subtitle = new sentence
    
    [SerializeField] private float fadeOutDuration;

    private float _maxBoxHeight;
    [SerializeField] private TMP_Text _subtitleBox;

    private string _subtitle;
    // private string _subtitleSpoken; // The subtitle which audio has been generated for
    private AudioClip _audioClip;
    private float _audioClipLength;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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

    // Update is called once per frame
    void Update()
    {
        
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
