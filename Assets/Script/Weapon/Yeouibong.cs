using System;
using UnityEngine;

public class Yeouibong : WeaponBase
{
    private bool _isMelee = false;

    private float _stunDuration = 0.0f;
    private float _damage = 0.0f;
    private float _additionalDamage = 0.0f;

    private bool _isAttackEnd = false;

    public void SetData(PassiveSkillData passiveSkillData, ActiveSkillData activeSkillData, bool isMelee)
    {
        _isMelee = isMelee;

        _stunDuration = passiveSkillData.GetValue(0);
        _damage = passiveSkillData.GetValue(1);
        _additionalDamage = activeSkillData.GetValue(_isMelee ? 0 : 1);
        
        anim.OnEnd = () =>
        {
            if (owner is CloneController cloneController)
            {
                cloneController.Despawn();
                _isAttackEnd = false;
            }
        };
    }

    private void ExecutePassiveSkill(Monster monster)
    {
        owner.SetFlip(monster.transform.position.x > owner.transform.position.x);
        
        ApplyStun(monster.status);
        monster.HasAttacked(_damage);

        if (_isMelee)
        {
            bool isWound = HasDebuff(monster.status, typeof(Wound));

            if (isWound)
            {
                monster.HasAttacked(_additionalDamage);
            }
        }
        else
        {
            bool isMark = HasDebuff(monster.status, typeof(Mark));

            if (isMark)
            {
                monster.HasAttacked(_additionalDamage);
            }
        }
        
        anim?.PlayAnimation();
    }

    private void ApplyStun(Status monster)
    {
        Stun stun = new Stun(monster.gameObject, _stunDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster, stun);
    }

    private bool HasDebuff(Status monster, Type debuffType)
    {
        StatusEffect isDebuff = StatusEffectManager.Instance.GetStatusEffect(monster, debuffType);

        return isDebuff != null;
    }

    private void Update()
    {
        if (_isAttackEnd)
        {
            return;
        }
        
        GameObject target = owner.FindNearestTarget();
        if (target == null)
        {
            if (owner is CloneController cloneController)
            {
                return;
            }
        }
        
        if (target.TryGetComponent(out Monster monster))
        {
            _isAttackEnd = true;
            ExecutePassiveSkill(monster);
        }
    }

    protected override void Attack()
    {
        ;
    }
}