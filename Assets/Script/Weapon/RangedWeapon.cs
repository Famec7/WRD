using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class RangedWeapon : WeaponBase
{
    public enum Type
    {
        Bow,
        Gun,
        Orb,
        HighOrb,
        Wand,
        Stone,
    }

    [SerializeField] protected Type type;
    public Type WeaponType => type;

    [Space] [Header("무기 종류에 맞는 사운드")] [SerializeField]
    private AudioClip _attackSound;

    protected AudioClip AttackSound => _attackSound;

    protected override void Attack()
    {
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            OnHit(monster, Data.AttackDamage);
            PlayEffect(monster.transform.position);
            PlayAttackSound();
        }
    }

    protected virtual void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);
    }

    protected virtual void PlayEffect(Vector3 position)
    {
        ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>(type + "Hit");
        particleEffect.SetPosition(position);
    }

    protected virtual void PlayAttackSound()
    {
        SoundManager.Instance.PlaySFX(_attackSound);
    }
}