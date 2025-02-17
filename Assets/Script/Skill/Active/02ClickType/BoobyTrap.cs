using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoobyTrap : ClickTypeSkill
{
    [SerializeField]
    private SlowZone _slowZone;
    [SerializeField]
    private DamageAmplificationZone _damageAmplificationZone;

    protected override void Init()
    {
        base.Init();

        float effectTime = Data.GetValue(0);
        _slowZone.SetData(effectTime, Data.Range, Data.GetValue(2));
        
        _damageAmplificationZone.SetData(effectTime, Data.Range, Data.GetValue(1));
    }

    public override void OnActiveEnter()
    {
        _slowZone.SetPosition(ClickPosition);
        _slowZone.PlayEffect();
        
        _damageAmplificationZone.SetPosition(ClickPosition);
        _damageAmplificationZone.PlayEffect();
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {

    }
}
