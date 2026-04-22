namespace Bots
{
    public class BatteryBotManager: BotManager
    {
        public BotBattery botBattery;

        private protected override void LoadExtras()
        {
            botBattery = GetComponent<BotBattery>();
        }
    }
}
