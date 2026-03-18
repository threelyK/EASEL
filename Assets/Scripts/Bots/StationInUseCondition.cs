using System;
using Bots;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "StationInUse", story: "[Target] Station is being used", category: "Conditions", id: "6894f87f6c450f9c7649671e5a26c990")]
public partial class StationInUseCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    private Station _station;

    public override bool IsTrue()
    {
        return IsStation() && _station._stationInUse;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }

    private bool IsStation()
    {
        var obj= Target.Value;
        if (obj == null) return false;
        
        _station = obj.GetComponent<Station>();
        return _station != null;
    }
}
