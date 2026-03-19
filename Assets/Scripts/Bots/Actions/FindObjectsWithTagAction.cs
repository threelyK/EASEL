using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindObjectsWithTag", story: "Finds [objects] in [float] range with [strTags] tags", category: "Action", id: "23c87619cae596410e34fa115be9d835")]
public partial class FindObjectsWithTagAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Objects;
    [SerializeReference] public BlackboardVariable<float> Float;
    [SerializeReference] public BlackboardVariable<List<string>> StrTags;
    
    private Collider[] _hitColliders;
    private int maxColliders = 10;

    protected override Status OnStart()
    {
        if (Float.Value == 0 || StrTags.Value == null) return Status.Failure;

        var objectsList = Objects.Value;
        
        _hitColliders = new Collider[maxColliders];
        _hitColliders = Physics.OverlapSphere(GameObject.transform.position, Float.Value);

        objectsList.AddRange(from collider in _hitColliders from tag in StrTags.Value where collider.CompareTag(tag) select collider.gameObject);

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

