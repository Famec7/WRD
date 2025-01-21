using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : ClickTypeSkill
{
    [SerializeField]
    private AudioClip _fireSound;
    
    private float _singleDamage = 0.0f;
    private float _multipleDamage = 0.0f;
    private float slowDuration = 0.0f;
    private float slowRate = 0.0f;
    
    public override void OnActiveEnter()
    {
        _singleDamage = Data.GetValue(0);
        slowDuration = Data.GetValue(1);
        slowRate = Data.GetValue(2);
        _multipleDamage = Data.GetValue(3);
    }

    public override bool OnActiveExecute()
    {
        Monster target = SelectMonsterAtClickPosition();

        if (target != null)
        {
            StatusEffect markStatus = StatusEffectManager.Instance.GetStatusEffect(target.status, typeof(Mark));

            if (markStatus is null)
            {
                OnAttackMultipleTargets();
                return true;
            }

            OnAttackSingleTarget(target);
        }
        else
        {
            OnAttackMultipleTargets();
        }
        
        return true;
    }

    public override void OnActiveExit()
    {
        
    }

    private void OnAttackSingleTarget(Monster monster)
    {
        monster.HasAttacked(_singleDamage);
        
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
        effect.SetPosition(monster.transform.position);
        effect.PlayEffect();
        
        SoundManager.Instance.PlaySFX(_fireSound);
    }

    private void OnAttackMultipleTargets()
    {
        if (IndicatorMonsters.Count == 0)
        {
            return;
        }
        
        foreach (var target in IndicatorMonsters)
        {
            target.HasAttacked(_multipleDamage);
            
            Status status = target.status;
            StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, slowRate, slowDuration));
            
            ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
            effect.SetPosition(target.transform.position);
            effect.PlayEffect();
        }

        SoundManager.Instance.PlaySFX(_fireSound);
    }
}