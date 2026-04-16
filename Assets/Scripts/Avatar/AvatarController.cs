
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AvatarController : MonoBehaviour
{
    public Image mouthImage;
    public AudioClip testClip;
    
    [SerializeField] private Sprite _mouthClosed;
    [SerializeField] private Sprite _mouthMid;
    [SerializeField] private Sprite _mouthOpen;

    [SerializeField] private GameObject _avatar;
    [SerializeField] private bool alwaysDisplayAvatar;
    private Coroutine mouthRoutine;
    
    void Start()
    {
        if (alwaysDisplayAvatar)
        {
            _avatar.SetActive(true);
            mouthImage.sprite = _mouthClosed;
        }
        else
        {
            _avatar.SetActive(false); // Only show when talking
        }
    }

    private void OnEnable()
    {
        RadioActions.OnClipGenerated += HandleClip;
    }

    private void OnDisable()
    {
        RadioActions.OnClipGenerated -= HandleClip;
    }

    private void HandleClip(AudioClip clip)
    {
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
            if (rVal < 0.3f) mouthImage.sprite = _mouthClosed;
            else if (rVal < 0.8) mouthImage.sprite = _mouthMid;
            else mouthImage.sprite = _mouthOpen;
            
            yield return new WaitForSeconds(0.08f);
            time += 0.08f;
        }

        mouthImage.sprite = _mouthClosed;
    }

    private IEnumerator StopMouth(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!alwaysDisplayAvatar)
        {
            _avatar.SetActive(false);
        }
        mouthRoutine = null;
    }
}
