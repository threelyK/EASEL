using UnityEngine;

public class BGMLooper : MonoBehaviour
{

    [SerializeField] private AudioClip _songA;
    [SerializeField] private AudioClip _songB;

    AudioSource src;
    int index;
    AudioClip[] songs;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        songs = new AudioClip[] { _songA, _songB };
        index = 0;
        PlayCurrent();
    }

    void PlayCurrent()
    {
        if (songs[index] == null) return;
        src.clip = songs[index];
        src.Play();
        CancelInvoke(nameof(Advance)); // safe if previously scheduled
        Invoke(nameof(Advance), songs[index].length);
    }

    void Advance()
    {
        index = (index + 1) % songs.Length;
        PlayCurrent();
    }

}
