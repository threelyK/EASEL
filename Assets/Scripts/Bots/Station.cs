using System;
using DG.Tweening;
using UnityEngine;

namespace Bots
{
    public class Station : MonoBehaviour
    {
        [SerializeField] public StationType StationType = StationType.NOT_SET;
        public GameObject activeBot; // Bot currently using this
        private BotManager _botManager;

        private const float _chargeTime = 5; // Should be moved to a different class

        public bool _stationInUse;

        private protected void Start()
        {
            if (StationType == StationType.NOT_SET)
            {
                throw new Exception("Station Type not set for: " + gameObject.name);
            }
        }

        public void UseStation(GameObject bot)
        {
            if (_stationInUse)  return;
            
            _stationInUse = true;
            activeBot = bot;
            _botManager = activeBot.GetComponent<BotManager>();

            if (_botManager == null)
            {
                Debug.LogError("No bot manager found for: " + gameObject.name);
            }
            
            ExecuteStationProcess();
            _stationInUse = false;
        }

        private void ExecuteStationProcess()
        {
            switch (StationType)
            {
                case StationType.NOT_SET:
                    throw new Exception("Station Type not set for: " + gameObject.name);
                case StationType.FRUIT:
                    // Play noise + particle effects
                    var heightChange = 1f;
                    var duration = 1f;
                    transform.DOMoveY(transform.localPosition.y + heightChange, duration).SetEase(Ease.InSine)
                        .OnComplete(() => Destroy(gameObject));
                    
                    break;
                case StationType.CHARGER:
                    _stationInUse = true; // Maybe should be set to true when bot is making way to it
                    
                    Invoke(nameof(chargeBatteryToFull), _chargeTime);
                    _botManager.botController.MoveToSnap(transform);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void chargeBatteryToFull()
        {
            if (_botManager == null) return;
            _botManager.botBattery.SetBatteryLevel(100);
        }
    }


    public enum StationType
    {
        NOT_SET,
        FRUIT,
        CHARGER,
    }
}
