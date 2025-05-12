using System;
using System.Collections;
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
        
        _delay = new WaitForSeconds(data.GetValue(1));
    }
    
    #endregion
    
    [SerializeField]
    private SlowZone _slowZone;
    [SerializeField]
    private ElectricZone _electricZone;
    
    [SerializeField]
    private Animator _thunderStrikeEffect;

    private WaitForSeconds _delay;
    
    protected override void Init()
    {
        ;
    }

    public override void PlayEffect()
    {
        StartCoroutine(IE_PlayEffect());
    }

    public override void StopEffect()
    {
        _slowZone.StopEffect();
        _electricZone.StopEffect();
        EffectManager.Instance.ReturnEffectToPool(this, "ThunderEffect");
    }
    
    private IEnumerator IE_PlayEffect()
    {
        _thunderStrikeEffect.Play("ThunderStrike");
        
        _slowZone.SetPosition(this.transform.position);
        _slowZone.PlayEffect();
        
        _electricZone.SetPosition(this.transform.position);
        _electricZone.PlayEffect();

        yield return _delay;
        
        StopEffect();
        
        _slowZone.StopEffect();
        _electricZone.StopEffect();
    }
}