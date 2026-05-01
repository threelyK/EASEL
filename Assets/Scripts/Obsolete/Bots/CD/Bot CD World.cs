using System;
using UnityEngine;

namespace Bots.CD
{
    public class BotCDWorld : MonoBehaviour
    {
        [SerializeField]
        private BotCDSO _botCDSO;

        public static Action<BotCDSO, int> OnCDCollected;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Bot"))
            {
                var obj = other.gameObject;

                var botController = obj.GetComponent<BGBotController>();
                botController.SwitchCD(_botCDSO);
                
                Destroy(gameObject);
            }
        }
    
    
    }
}
