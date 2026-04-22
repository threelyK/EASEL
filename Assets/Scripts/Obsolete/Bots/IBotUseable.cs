
namespace Bots
{
    public interface IBotUseable
    {
        public bool isBeingUsed { get; set; }

        abstract void HandleInteraction();
    }
}
