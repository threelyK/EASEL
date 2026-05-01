using System;
using UnityEngine;

public class Mailbox : MonoBehaviour
{
    public BuddyBot buddyBot;
    private AudioSource _mailboxAudioSource;

    private void Start()
    {
        _mailboxAudioSource = gameObject.GetComponent<AudioSource>();
        if (_mailboxAudioSource is null)
        {
            Debug.LogError("MailboxAudioSource is null");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("BotComponent")) return;
        
        var botComponent = other.gameObject.GetComponent<BotComponent>();
        botComponent.AddComponent();
        Destroy(other.gameObject);
        
        _mailboxAudioSource.Play();
    }
}
