using System;
using Bots;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetStationActiveBot", story: "Set [Target] Station ActiveBot to [self]", category: "Action", id: "b34362621fdf18fd4d8f669831409540")]
public partial class SetStationActiveBotAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        
        var obj = Target.Value;
        if (obj == null) return Status.Failure;
        
        var station = obj.GetComponent<Station>();
        if (station == null) return Status.Failure;
        station.activeBot = Self;
        
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

