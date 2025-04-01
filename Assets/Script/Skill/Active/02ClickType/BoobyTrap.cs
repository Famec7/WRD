using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoobyTrap : ClickTypeSkill
{
    [SerializeField]
    private GameObject _trap;

    protected override void Init()
    {
        base.Init();

        float effectTime = Data.GetValue(0);
        
        _trap.GetComponent<SlowZone>().SetData(effectTime, Data.Range, Data.GetValue(2));
        _trap.GetComponent<DamageAmplificationZone>().SetData(effectTime, Data.Range, Data.GetValue(1));
    }

    public override void OnActiveEnter()
    {
        _trap.transform.SetParent(null);
        
        _trap.GetComponent<SlowZone>().SetPosition(ClickPosition);
        _trap.GetComponent<SlowZone>().PlayEffect();
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {

    }
}
