using UnityEngine;

namespace Bots.Components
{
    public class Part : MonoBehaviour
    {
        [SerializeField] private MissingPartType _partType;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Bot")) return;
            
            var obj = other.collider.gameObject;

            if (obj.TryGetComponent<WIPBot>(out var wipBot))
            {
                if (!wipBot.isOnWorkbench) return;

                if (wipBot.hasPart(_partType)) return;
                wipBot.OnPartAdded(_partType);
                
                Destroy(gameObject); // Use up this part
            }
        }
        
    }
}
