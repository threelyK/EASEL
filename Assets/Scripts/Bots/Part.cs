using UnityEngine;

namespace Bots
{
    public enum BotPartType
    {
        PERCEPTION,
        REASONING,
        TOOLS,
        MEMORY,
        LEARNING,
        ORCHESTRATION
    }
    public class Part : MonoBehaviour
    {
        [SerializeField] private BotPartType _type;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("PartialBot")) return;
            
            var _incompleteBot = other.collider.gameObject.GetComponent<WIPBot>();

            if (_incompleteBot.hasPart(_type)) return;
            _incompleteBot.OnPartAdded(_type);
        }
    }
}
