using System;
using UnityEngine;

namespace Bots.Stations
{
    public class SlimeStation : Station
    {
        private StationType _stationType = StationType.SLIME;
        
        public static Action<GameObject> OnSlimed;
        public static Action<GameObject> OnLeaveSlime;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Bot"))
            {
                OnSlimed?.Invoke(other.gameObject);
            }
        }

        private protected override void ExecuteStationProcess(){}
    }
}
