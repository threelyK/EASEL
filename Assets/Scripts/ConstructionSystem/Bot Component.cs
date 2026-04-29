using UnityEngine;

public class BotComponent : MonoBehaviour
{
    // Can be put into the mailbox
    
    public BotComponentType componentType;
    
    public void AddComponent()
    {
        BuddyBot.OnBotComponentAdd?.Invoke(componentType);
    }
}
