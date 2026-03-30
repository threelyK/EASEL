using Unity.Behavior;

namespace Bots.CD
{
    [BlackboardEnum]
    public enum BotIntent
    {
        Default,
        Assist,
        Construct,
        Transport,
        ButtonPusher,
    }
}
