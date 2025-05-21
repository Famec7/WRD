using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpBlade : InstantaneousSkill
{
    #region Data

    private float _damage;
    protected float WoundDamage { get; private set; }

    #endregion
    
    private bool _isAttack = false;

    [SerializeField] private AudioClip sfx;

    protected override void Init()
    {
        base.Init();
        
        _damage = Data.GetValue(0);
        WoundDamage = Data.GetValue(1);
    }
    
    public override void OnActiveEnter()
    {
        _isAttack = false;
        weapon.OnAttack += OnAttack;
    }

    public override bool OnActiveExecute()
    {
        if (_isAttack)
        {
            return true;
        }

        return false;
    }

    public override void OnActiveExit()
    {
        weapon.OnAttack -= OnAttack;
    }

    private void OnAttack()
    {
        Vector3 dir = weapon.owner.Target.transform.position - weapon.owner.transform.position;

        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, Data.Range, 360.0f, targetLayer);

        // 이펙트 재생
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("SwingEffect");

        effect.SetPosition(weapon.owner.transform.position);
        effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir)));
        effect.PlayEffect();
        
        SoundManager.Instance.PlaySFX(sfx);

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                var wound = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));

                if (wound is null)
                {
                    monster.HasAttacked(_damage);
                }
                else
                {
                    StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(Wound));
                    TakeWoundDamage(monster);
                }
            }
        }
        
        _isAttack = true;
    }
    
    protected virtual void TakeWoundDamage(Monster monster)
    {
        monster.HasAttacked(WoundDamage);
    }
}