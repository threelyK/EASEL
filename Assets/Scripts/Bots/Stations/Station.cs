using UnityEngine;

namespace Bots.Stations
{
    public abstract class Station : MonoBehaviour
    {
        public GameObject activeBot { get; private set; } // Bot currently using this
        public int zone;
        
        private protected BGBotController _botController;
        private protected BotManager _botManager;
        
        public bool _stationInUse;
        
        public void UseStation(GameObject bot)
        {
            if (activeBot != bot) return;
            
            _botManager = bot.GetComponent<BotManager>();
            _botController = _botManager.botController;
            
            _stationInUse = true;
            ExecuteStationProcess();
            ResetBotVariables();
            _stationInUse = false;
        }

        private protected abstract void ExecuteStationProcess();

        private void ResetBotVariables()
        {
            SetActiveBot(null);
            _botController = null;
            _botManager = null;
        }
        
        public void SetActiveBot(GameObject bot)
        {
            activeBot = bot;
        }
    }
}
