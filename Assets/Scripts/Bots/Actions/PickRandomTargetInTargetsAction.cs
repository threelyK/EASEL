using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PickRandomTargetInTargets", story: "Pick random [Target] in [Targets]", category: "Action", id: "14bc89bb771cbcc1f9481452e2d6b756")]
public partial class PickRandomTargetInTargetsAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;

    protected override Status OnStart()
    {
        var objs = Targets.Value;
        if (objs.Count == 0) return Status.Failure;

        Target.Value = objs[Random.Range(0, objs.Count)];
        
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

