
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotManager : MonoBehaviour
    {
        public BGBotController botController;
        private protected BotAnimationController _animationController;
        public BehaviorGraphAgent bgAgent;
        public NavMeshAgent navMeshAgent;

        private protected void Awake()
        {
            botController = GetComponent<BGBotController>();
            _animationController = GetComponent<BotAnimationController>();
            bgAgent = GetComponent<BehaviorGraphAgent>();

            LoadExtras();
        }

        private protected virtual void LoadExtras(){}
    }
}
