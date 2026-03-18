using System;
using Bots;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Station Active Bot", story: "Check [Target] Station's ActiveBot is [self] or null", category: "Conditions", id: "34c30d8146ddf3c70725e7afe4768500")]
public partial class CheckStationActiveBotCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    public override bool IsTrue()
    {
        var obj = Target.Value;
        if (obj == null) return false;
        
        var station = obj.GetComponent<Station>();
        if (station == null) return false;

        return station.activeBot == Self.Value || station.activeBot == null;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
