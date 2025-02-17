using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ElectricEffect : EffectBase
{
    #region Data
    
    public void SetData(SkillData data)
    {
        _slowZone.SetData(data.GetValue(1), data.Range, data.GetValue(2));
        _electricZone.SetData(data.GetValue(1), data.Range, data.GetValue(4), data.GetValue(3));
    }
    
    #endregion
    
    [SerializeField]
    private SlowZone _slowZone;
    [SerializeField]
    private ElectricZone _electricZone;
    
    protected override void Init()
    {
        ;
    }

    public override void PlayEffect()
    {
        _slowZone.SetPosition(this.transform.position);
        _slowZone.PlayEffect();
        
        _electricZone.SetPosition(this.transform.position);
        _electricZone.PlayEffect();
    }

    public override void StopEffect()
    {
        _slowZone.StopEffect();
        _electricZone.StopEffect();
        EffectManager.Instance.ReturnEffectToPool(this, "ThunderEffect");
    }
}