
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Behavior;
using Action = System.Action;

namespace Bots
{
    public class BotBattery : MonoBehaviour
    {
        
        [SerializeField, Tooltip("Out of 100")] private float _batteryLevel = 100;
        
        private BehaviorGraphAgent _bgAgent;
        private BlackboardVariable<float> _batteryLevelVariable;
        
        private const float _dischargeTime = 30; // 100 - 0 in 30s
        private const float _dischargeWait =  1f; // Discharges every 1s
        private CancellationTokenSource _cts;

        private void Awake()
        {
            _bgAgent = GetComponent<BehaviorGraphAgent>();
            _bgAgent.GetVariable("BatteryLevel", out _batteryLevelVariable);
        }

        private void Start()
        {
            _batteryLevelVariable.Value = _batteryLevel;
        }

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
                    _batteryLevelVariable.Value = _batteryLevel;

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
    }
}
