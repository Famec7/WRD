using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    private enum Type
    {
        Sword,
        Spear,
        Club,
    }
    
    [SerializeField]
    private Type type;
    
    protected override void Attack()
    {
        base.Attack();
        
        if (owner.Target.TryGetComponent(out Monster monster))
        {
#if WEAPON_DEBUG
            Debug.Log($"{Data.AttackDamage}의 데미지를 입힘");
#endif
            monster.HasAttacked(Data.AttackDamage);
            
            ParticleEffect particleEffect = GetHitEffect();
            particleEffect.SetPosition(owner.Target.transform.position);
        }
    }
    
    public ParticleEffect GetHitEffect()
    {
        switch (type)
        {
            case Type.Sword:
                return EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
            case Type.Spear:
                return EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
            case Type.Club:
                return EffectManager.Instance.CreateEffect<ParticleEffect>("ClubHit");
            default:
                break;
        }

        return null;
    }
}