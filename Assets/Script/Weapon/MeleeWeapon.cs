using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    public enum Type
    {
        Sword,
        Spear,
        Club,
        Joker,
    }
    
    [SerializeField]
    private Type type;
    
    [Space] [Header("무기 종류에 맞는 사운드")]
    [SerializeField] private AudioClip _attackSound;
    
    protected override void Attack()
    {
        if (owner.Target.TryGetComponent(out Monster monster))
        {
#if WEAPON_DEBUG
            Debug.Log($"{Data.AttackDamage}의 데미지를 입힘");
#endif
            monster.HasAttacked(Data.AttackDamage);
            
            EffectBase hitEffect = GetHitEffect();
            hitEffect.SetPosition(owner.Target.transform.position);
            hitEffect.PlayEffect();
            
            SoundManager.Instance.PlaySFX(_attackSound);
        }
    }

    private EffectBase GetHitEffect()
    {
        switch (type)
        {
            case Type.Sword:
                return EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
            case Type.Spear:
                return EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
            case Type.Club:
                return EffectManager.Instance.CreateEffect<ParticleEffect>("ClubHit");
            case Type.Joker:
                return EffectManager.Instance.CreateEffect<AnimationEffect>("JokerSlash");
            default:
                break;
        }

        return null;
    }
}