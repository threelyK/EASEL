using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BlindWalk", story: "[Navagent] blindly walks forward for [duration]s", category: "Action", id: "8dae6f7e1eb80c0904a48053d517b574")]
public partial class BlindWalkAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Navagent;
    [SerializeReference] public BlackboardVariable<float> Duration;
    
    private NavMeshAgent _agent;
    private float _timer;
    private bool _started;
    protected override Status OnStart()
    {
        if (Duration.Value <= 0f) return Status.Failure;

        _agent = Navagent.Value;
        _started = true;
        _timer = 0f;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (!_started) return Status.Failure;
        _timer += Time.deltaTime;

        _agent.Move(_agent.transform.forward * Time.deltaTime);
        
        if (_timer >= Duration.Value)
        {
            return Status.Success;
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
        _started = false;
    }
}

