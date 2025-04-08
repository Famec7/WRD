using System;
using System.Collections.Generic;
using UnityEngine;

public class HighDomainExpansion : PassiveAuraSkillBase
{
    [SerializeField]
    private SlowZone _slowZone;
    
    protected override void Init()
    {
        base.Init();
        
        _slowZone.SetData(0, Data.Range, Data.GetValue(0));
    }

    private void Start()
    {
        _slowZone.PlayEffect();
    }

    private void OnDisable()
    {
        _slowZone.StopEffect();
    }
}