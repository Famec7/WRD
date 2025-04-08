using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : ClickTypeSkill
{
    [SerializeField]
    private AudioClip _fireSound;
    
    private float _damage = 0.0f;
    private float _additionalDamage = 0.0f;
    private float _slowDuration = 0.0f;
    private float _slowRate = 0.0f;
    
    public override void OnActiveEnter()
    {
        _damage = Data.GetValue(0) * Data.GetValue(1);
        _slowDuration = Data.GetValue(2);
        _slowRate = Data.GetValue(3);
        _additionalDamage = Data.GetValue(4);
    }

    public override bool OnActiveExecute()
    {
        OnAttackMultipleTargets();
        
        return true;
    }

    public override void OnActiveExit()
    {
        
    }

    private void OnAttackMultipleTargets()
    {
        if (IndicatorMonsters.Count == 0)
        {
            return;
        }
        
        foreach (var target in IndicatorMonsters)
        {
            target.HasAttacked(_damage);
            
            Status status = target.status;
            StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, _slowRate, _slowDuration));
            
            StatusEffect mark = StatusEffectManager.Instance.GetStatusEffect(status, typeof(Mark));
            if (mark != null)
            {
                target.HasAttacked(_additionalDamage);
            }

            ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
            effect.SetPosition(target.transform.position);
            effect.PlayEffect();
        }

        SoundManager.Instance.PlaySFX(_fireSound);
    }
}