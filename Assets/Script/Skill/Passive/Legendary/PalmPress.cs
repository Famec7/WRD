﻿using UnityEngine;

public class PalmPress : PassiveSkillBase
{
    private float _damage;
    private float _stunDuration;
    private float _woundDamage;
    
    [SerializeField]
    private AudioClip sfx;

    protected override void Init()
    {
        base.Init();

        _damage = Data.GetValue(0);
        _stunDuration = Data.GetValue(1);
        _woundDamage = Data.GetValue(2);
    }

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
        {
            return false;
        }

        LayerMask monsterLayer = LayerMaskProvider.MonsterLayerMask;
        var targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, Data.Range, 360.0f, monsterLayer);
        
        if (targets.Count == 0)
        {
            return false;
        }

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(_damage);
                ApplyStun(monster);

                if (HasWound(monster))
                {
                    monster.HasAttacked(_woundDamage);                    
                }
            }
        }

        var effect = EffectManager.Instance.CreateEffect<BuddhaHandEffect>("BuddhaEffect");
        effect.transform.position = targets[0].transform.position;
        effect.PlayEffect();
        SoundManager.Instance.PlaySFX(sfx);

        return true;
    }

    // 자상 디버프가 있으면 자상을 제거하고 true 반환, 없으면 false 반환
    private bool HasWound(Monster monster)
    {
        StatusEffect wound = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));

        if (wound is null)
        {
            return false;
        }

        StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(Wound));
        return true;
    }

    private void ApplyStun(Monster monster)
    {
        StatusEffect stun = new Stun(monster.gameObject, _stunDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);
    }
}