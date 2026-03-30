using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindObjectsWithTag", story: "Finds [Objects] in [float] range with [tagToFind] tag", category: "Action", id: "bf070e4005a7271304d0d4343bf5c247")]
public partial class FindObjectsWithTagAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Objects;
    [SerializeReference] public BlackboardVariable<float> Float;
    [SerializeReference] public BlackboardVariable<string> TagToFind;
    
    private Collider[] _hitColliders;
    private const int _maxColliders = 10;

    protected override Status OnStart()
    {
        if (Float.Value == 0 || TagToFind.Value == null) return Status.Failure;

        var objectsList = Objects.Value;

        _hitColliders = new Collider[_maxColliders];
        _hitColliders = Physics.OverlapSphere(GameObject.transform.position, Float.Value);

        objectsList.AddRange(from collider in _hitColliders 
            where collider.CompareTag(TagToFind) select collider.gameObject);

        return Status.Success;
    }
}

