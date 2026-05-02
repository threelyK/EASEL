
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AvatarController : MonoBehaviour
{
    [SerializeField] private Image _mouthImage;
    [SerializeField] private Image _eyeImage;
    [SerializeField] private Image _thinkBubble;
    
    
    
    [SerializeField] private Sprite _mouthClosed;
    [SerializeField] private Sprite _mouthMid;
    [SerializeField] private Sprite _mouthOpen;
    
    [SerializeField] private Sprite _blink;
    [SerializeField] private Sprite _gazeCenter;
    [SerializeField] private Sprite _gazeStraight;
    
    private Sprite _normalEye;

    [SerializeField] private GameObject _avatar;
    [SerializeField] private bool alwaysDisplayAvatar;
    
    private Coroutine mouthRoutine;
    
    private WaitForSeconds _mouthSwitchDelay = new(0.08f);
    private WaitForSeconds _blinkDelay = new(4f);
    private WaitForSeconds _blinkDuration = new(0.2f);
    
    void Start()
    {
        if (alwaysDisplayAvatar)
        {
            _thinkBubble.enabled = false;
            _avatar.SetActive(true);
            _mouthImage.sprite = _mouthClosed;
            
            _normalEye = _gazeCenter;
            _eyeImage.sprite = _normalEye;
        }
        else
        {
            _avatar.SetActive(false); // Only show when talking
        }
        
        StartCoroutine(AnimateBlink());
    }

    private void OnEnable()
    {
        RadioActions.OnClipGenerated += HandleClip;
        RadioActions.OnRadioReady += HandleThinking;
    }

    private void OnDisable()
    {
        RadioActions.OnClipGenerated -= HandleClip;
        RadioActions.OnRadioReady -= HandleThinking;
    }

    private void HandleThinking(bool isRadioReady)
    {
        if (isRadioReady)
        {
            // Agent is done thinking or finished talking
            _thinkBubble.enabled = false;
        }
        else
        {
            // Agent is thinking
            _thinkBubble.enabled = true;
        }
    }
    
    private void HandleClip(AudioClip clip)
    {
        AgentLookAtPlayer(true);
        
        if (clip == null) return;
        if (mouthRoutine != null) StopCoroutine(mouthRoutine);
        if (!alwaysDisplayAvatar)
        {
            _avatar.SetActive(true);
        }

        mouthRoutine = StartCoroutine(AnimateMouth(clip.length));
        StartCoroutine(StopMouth(clip.length + 0.2f));
    }
    
    private IEnumerator AnimateMouth(float clipLength)
    {
        var time = 0f;
        while (time < clipLength)
        {
            float rVal = Random.value;
            if (rVal < 0.3f) _mouthImage.sprite = _mouthClosed;
            else if (rVal < 0.8) _mouthImage.sprite = _mouthMid;
            else _mouthImage.sprite = _mouthOpen;
            
            yield return _mouthSwitchDelay;
            time += 0.08f;
        }

        _mouthImage.sprite = _mouthClosed;
    }

    private IEnumerator StopMouth(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!alwaysDisplayAvatar)
        {
            _avatar.SetActive(false);
        }
        mouthRoutine = null;
        AgentLookAtPlayer(false);
    }

    private void AgentLookAtPlayer(bool lookAtPlayer)
    {
        if (lookAtPlayer)
        {
            _normalEye = _gazeStraight;
            _eyeImage.sprite = _normalEye;
        }
        else
        {
            _normalEye = _gazeCenter;
        }
    }

    private IEnumerator AnimateBlink()
    {
        while (true)
        {
            _eyeImage.sprite = _blink;
            yield return _blinkDuration;
            _eyeImage.sprite = _normalEye;

            yield return _blinkDelay;
        }
    }
}
