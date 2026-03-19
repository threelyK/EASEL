
namespace Bots.Stations
{
    public class ChargerStation : Station
    {
        public StationType stationType = StationType.CHARGER;
        private const float _chargeTime = 5; // Time it takes to fully charge
        private BotBattery _battery;
        private BatteryBotManager _bBotManager;
        
        private protected override void ExecuteStationProcess()
        {
            _bBotManager = _botManager.gameObject.GetComponent<BatteryBotManager>();
            if (_bBotManager == null) return;
            
            _battery = _bBotManager.botBattery;
            if (_battery == null) return;
            
            _botController.MoveToSnap(transform);
            Invoke(nameof(chargeBatteryToFull), _chargeTime);
        }
    
        private void chargeBatteryToFull()
        {
            _battery.SetBatteryLevel(100);
        }
    }
}
