using UnityEngine;

namespace Bots
{
    public abstract class Station : MonoBehaviour
    {
        public GameObject activeBot; // Bot currently using this
        private protected BotManager _botManager;
        
        public bool _stationInUse;
        
        public void UseStation(GameObject bot)
        {
            if (activeBot != bot) return;
            
            _botManager = activeBot.GetComponent<BotManager>();

            if (_botManager == null)
            {
                Debug.LogError("No bot manager found for: " + gameObject.name);
            }
            
            _stationInUse = true;
            ExecuteStationProcess();
            activeBot = null;
            _stationInUse = false;
        }

        private protected abstract void ExecuteStationProcess();
    }
    
    public enum StationType
    {
        NOT_SET,
        FRUIT,
        CHARGER,
    }
}
