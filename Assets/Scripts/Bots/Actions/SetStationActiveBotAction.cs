using System;
using Bots.Stations;
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

    private GameObject _target;
    
    protected override Status OnStart()
    {
        _target = Target.Value;
        
        var station = _target.GetComponent<Station>();
        if (station == null) return Status.Failure;
        
        station.SetActiveBot(Self.Value);
        return Status.Success;
    }
}

