using UnityEngine;

namespace Bots
{
    public class Part : MonoBehaviour
    {
        [SerializeField] private BotPartType _type;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("PartialBot")) return;

            var _incompleteBot = other.collider.gameObject.GetComponent<WIPBot>();

            if (_incompleteBot.hasPart(_type)) return;
            _incompleteBot.OnPartAdded(_type);
            Destroy(gameObject); // Use up this part
        }
    }
}
