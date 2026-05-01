namespace Bots.Stations
{
    public class DeadStation: Station
    {
        // A station that does nothing
        public StationType stationType = StationType.DEAD;

        private protected override void ExecuteStationProcess(){}
    }
}
