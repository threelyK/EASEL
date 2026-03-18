
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Bots
{
    public class BotBattery : MonoBehaviour
    {
        
        [SerializeField, Tooltip("Out of 100")] private float _batteryLevel = 100;
        
        private const float _dischargeTime = 30; // 100 - 0 in 30s
        private const float _dischargeWait =  1f; // Discharges every 1s
        private CancellationTokenSource _cts;
        
        public Action OnBatteryLevelChanged;
        
        private void OnEnable()
        {
            StartRepeating();
        }

        private void OnDisable()
        {
            _cts.Cancel();
        }

        private void StartRepeating()
        {
            _cts =  new CancellationTokenSource();
            DischargeBatteryLoopAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid DischargeBatteryLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    _batteryLevel -= Mathf.Floor(100 / _dischargeTime);
                    OnBatteryLevelChanged?.Invoke();

                    var dischargeWaitInMilliSeconds = (int)_dischargeWait * 1000;
                    await UniTask.Delay(dischargeWaitInMilliSeconds, DelayType.DeltaTime, cancellationToken: token);
                }
            }
            catch (OperationCanceledException){}
        }
        
        /// <param name="newLevel">The new level to set it to. (Should be between 0-100)</param>
        public void SetBatteryLevel(int newLevel)
        {
            _batteryLevel = newLevel;
        }
        
        public float GetBatteryLevel()
        {
            return _batteryLevel;
        }

    }
}
