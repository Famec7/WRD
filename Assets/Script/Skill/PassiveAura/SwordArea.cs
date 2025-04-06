using System;
using UnityEngine;

public class SwordArea : PassiveAuraSkillBase
{
    [SerializeField] private SlowZone _slowZone;

    protected override void Init()
    {
        base.Init();
        
        if (_slowZone == null)
        {
            Debug.LogError("slowZone is null");
        }

        float radius = Data.Range;
        float slowRate = Data.GetValue(0);
        
        _slowZone.SetData(0.0f, radius, slowRate);
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