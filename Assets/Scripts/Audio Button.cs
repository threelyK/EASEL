using UnityEngine;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    
    public void playClip()
    {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}
