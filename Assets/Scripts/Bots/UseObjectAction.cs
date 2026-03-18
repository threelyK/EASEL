using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Bots
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "UseObject", story: "[Bot] uses [Object]", category: "Action", id: "890b5c1856a62f0d83419bb55c210d8a")]
    public partial class UseObjectAction : Action
    {
    [SerializeReference] public BlackboardVariable<GameObject> Bot;
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    private Station _station;
    
        protected override Status OnStart()
        {
            _station = Object.Value.GetComponent<Station>();
            _station.UseStation(Bot);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}

