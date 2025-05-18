using System;
using UnityEngine;

public class TridentOfPoseidon : PassiveAuraSkillBase
{
    [Header("이속 감소 범위")]
    [SerializeField]
    private float _slowZoneRange = 1.0f;
    
    [SerializeField] private SlowZone _slowZone;

    protected override void Init()
    {
        base.Init();
        _slowZone.SetData(0, _slowZoneRange, Data.GetValue(1));
    }

    private void OnEnable()
    {
        _slowZone.PlayEffect();
    }
    
    private void OnDisable()
    {
        _slowZone.StopEffect();
    }
}