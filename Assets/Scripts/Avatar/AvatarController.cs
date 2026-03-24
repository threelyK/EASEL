
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
    [SerializeField] private TMP_Text _subtitleText;
    
    private string _response;
    
    private Coroutine mouthRoutine;
    
    void Start()
    {
        _avatar.SetActive(false); // Only show when talking
        
        HandleClip(testClip);
    }

    private void OnEnable()
    {
        HandleClip(testClip); // TODO FOR TESTING
        RadioActions.OnClipGenerated += HandleClip;
        RadioActions.ResponseGenerated += StoreResponse;
    }

    private void StoreResponse(string response)
    {
        _response = response;
    }

    private void OnDisable()
    {
        RadioActions.OnClipGenerated -= HandleClip;
        RadioActions.ResponseGenerated -= StoreResponse;
    }

    private void HandleClip(AudioClip clip)
    {
        if (clip == null) return;
        if (mouthRoutine != null) StopCoroutine(mouthRoutine);
        _avatar.SetActive(true);

        // _subtitleText.text = _response;
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
        _avatar.SetActive(false);
        mouthRoutine = null;
    }
}
