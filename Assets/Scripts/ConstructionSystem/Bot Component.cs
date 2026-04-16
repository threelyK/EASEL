using UnityEngine;

public class BotComponent : MonoBehaviour
{
    // Can be put into the mailbox
    
    public BotComponentType componentType;
    [SerializeField] private AudioClip addNoise;
    [SerializeField] private AudioSource playerAudioSource;

    private void Start()
    {
        if (addNoise is null)
        {
            Debug.LogWarning("No noise provided");
        }

        if (playerAudioSource is null)
        {
            Debug.LogWarning("No player audio source provided");
        }
    }

    public void AddComponent()
    {
        BuddyBot.OnBotComponentAdd?.Invoke(componentType);
        playerAudioSource.PlayOneShot(addNoise);
    }
    
    
}
