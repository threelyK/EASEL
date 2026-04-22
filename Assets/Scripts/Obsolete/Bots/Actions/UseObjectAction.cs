using System;
using Bots.Stations;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;
using Action = Unity.Behavior.Action;

namespace Bots.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "UseObject", story: "[Bot] uses [StationObj]", category: "Action", id: "890b5c1856a62f0d83419bb55c210d8a")]
    public partial class UseObjectAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Bot;
        [FormerlySerializedAs("Station")] [SerializeReference] public BlackboardVariable<GameObject> StationObj;
        private Station _station;
    
        protected override Status OnStart()
        {
            var obj = StationObj.Value;
            _station = obj.GetComponent<Station>();
            if (!_station) return Status.Failure;
            
            _station.UseStation(Bot);
            return Status.Success;
        }
    }
}

