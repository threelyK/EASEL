namespace Bots.Stations
{
    public class ChargerStation : Station
    {
        private const float _chargeTime = 5; // Time it takes to fully charge
        public StationType stationType = StationType.CHARGER;
        
        private protected override void ExecuteStationProcess()
        {
            Invoke(nameof(chargeBatteryToFull), _chargeTime);
            _botManager.botController.MoveToSnap(transform);
        }
    
        private void chargeBatteryToFull()
        {
            if (_botManager == null) return;
            _botManager.botBattery.SetBatteryLevel(100);
        }
    }
}
