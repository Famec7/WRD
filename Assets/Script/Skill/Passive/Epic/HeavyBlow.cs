﻿using UnityEngine;

public class HeavyBlow : PassiveSkillBase
{
    [SerializeField] private AudioClip _heavyBlowSound;

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
            return false;

        Vector3 targetPosition = target.transform.position;
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Data.Range, default, targetLayer);

        if (targets.Count == 0)
            return false;
        
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("HeavyBlowEffect");
        effect.SetPosition(targetPosition);
        effect.PlayEffect();
        
        SoundManager.Instance.PlaySFX(_heavyBlowSound);

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));

                float stunDuration = Data.GetValue(1);

                Stun stun = new Stun(monster.gameObject, stunDuration);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);

                StatusEffect woundEffect = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));

                if (woundEffect != null)
                {
                    TakeWoundDamage(monster);
                }
            }
        }

        return true;
    }
    
    protected virtual void TakeWoundDamage(Monster monster)
    {
        monster.HasAttacked(Data.GetValue(2));
    }
}