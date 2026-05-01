using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindObjectsWithTags", story: "Finds [objects] in [float] range with [strTags] tags", category: "Action", id: "23c87619cae596410e34fa115be9d835")]
public partial class FindObjectsWithTagsAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Objects;
    [SerializeReference] public BlackboardVariable<float> Float;
    [SerializeReference] public BlackboardVariable<List<string>> StrTags;
    
    private Collider[] _hitColliders;
    private const int _maxColliders = 10;

    protected override Status OnStart()
    {
        if (Float.Value == 0 || StrTags.Value == null) return Status.Failure;

        var objectsList = Objects.Value;
        
        _hitColliders = new Collider[_maxColliders];
        _hitColliders = Physics.OverlapSphere(GameObject.transform.position, Float.Value);

        objectsList.AddRange(from collider in _hitColliders from tag in StrTags.Value where collider.CompareTag(tag) select collider.gameObject);

        return Status.Success;
    }
}

