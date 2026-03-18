
using Unity.Behavior;
using UnityEngine;

namespace Bots
{
    public class BotManager : MonoBehaviour
    {
        public BGBotController botController;
        private BotAnimationController _animationController;
    
        public BotBattery botBattery;
        public BehaviorGraphAgent bgAgent;
        private BlackboardVariable<float> _bgBatteryLevel;

        private void Awake()
        {
            botController = GetComponent<BGBotController>();
            _animationController = GetComponent<BotAnimationController>();
            
            botBattery = GetComponent<BotBattery>();
            bgAgent = GetComponent<BehaviorGraphAgent>();
            
            bgAgent.GetVariable("BatteryLevel", out _bgBatteryLevel);
        }

        private void OnEnable()
        {
            if (botBattery != null)
            {
                botBattery.OnBatteryLevelChanged += UpdateBGBatteryLevel;
            }
        }
        
        private void OnDisable()
        {
            if (botBattery != null)
            {
                botBattery.OnBatteryLevelChanged -= UpdateBGBatteryLevel;
            }
        }

        private void UpdateBGBatteryLevel()
        {
            _bgBatteryLevel.Value = botBattery.GetBatteryLevel();
        }
        
    }
}
