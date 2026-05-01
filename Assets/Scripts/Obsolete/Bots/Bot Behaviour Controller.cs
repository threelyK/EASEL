using UnityEngine;

namespace Bots
{
    public class BotBehaviourController : MonoBehaviour
    {
        // Handle the behaviour tree of the bots

        #region Operations Area Info

        public GameObject OperationsArea; // Area where bot is able to find target dest. and objects

        // SafeArea got from OperationalArea
        private GameObject SafeArea; // Area to go to if safety levels fall below a threshold

        #endregion

        private BotBattery _botBattery; // Bots shouldn't all spawn with same battery level to avoid overwhelming chargers
        private int _safetyLevel;
    
        private BotPersonality _personality;
        public BotGoal _goal; // Can be changed by the player

        private void CheckEnvironmentState()
        {
            // TODO:
            // Current state of stations, resources, other robots?
        }

        private void CheckBotState()
        {
            // TODO: Check own bot needs
            // Checking battery %
            // Safety need check
        }

        private void HandleBehaviourState()
        {
        
        }

        private void Start()
        {
            _botBattery = GetComponent<BotBattery>();
        }
    }

    public enum BotPersonality
    {
        OPTIMISER,
        GREEDY, // Hoarder
        LAZY,
        TERRITORIAL,
    }

    public enum BotGoal
    {
        ASSIST,
        STOCK,
        REPAIR,
        ASSEMBLE,
        MOVE
    }
}