using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindRandomPoint", story: "[NavAgent] find random [targetVector] in [Radius] radius", category: "Action", id: "9565841fcfa90dbfea97d9d422064713")]
public partial class FindRandomPointAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavAgent;
    [SerializeReference] public BlackboardVariable<Vector3> TargetVector;
    [SerializeReference] public BlackboardVariable<float> Radius;
    
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _navPath;
    
    private Vector3 _currentPos;
    private Vector3 _targetPos;
    
    private int _pathIterations = 10;
    
    protected override Status OnStart()
    {
        _navMeshAgent = NavAgent.Value;
        _navPath = new NavMeshPath();
        
        if (_navMeshAgent == null)
        {
            Debug.LogWarning("NavMeshAgent not found");
            return Status.Failure;
        }
        
        _currentPos = _navMeshAgent.transform.position;

        if (!GetRandomPoint())
        {
            TargetVector.Value = _currentPos;
        }
        else
        {
            TargetVector.Value = _targetPos;
        }
        
        return Status.Success;
    }
    
    private bool GetRandomPoint()
    {
        for (int i = 0; i < _pathIterations; i++)
        {
            _targetPos = _currentPos + Random.insideUnitSphere * Radius.Value;
            _targetPos.y = _navMeshAgent.transform.position.y;
            _navMeshAgent.CalculatePath(_targetPos, _navPath);
            if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                return true;
        }

        return false;
    }
}

