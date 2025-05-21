using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilBullet : ClickTypeSkill
{
    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private Animator _animator;
    
    private float _attackDamage = 0.0f;
    private float _amplification = 0.0f;
    private float _attackSpeedIncrease = 0.0f;
    
    private float _originAttackSpeed;

    public override void OnActiveEnter()
    {
        _originAttackSpeed = weapon.Data.AttackSpeed;
        
        _attackDamage = Data.GetValue(0);
        _amplification = Data.GetValue(1);
    }

    public override bool OnActiveExecute()
    {
        Monster target = SelectMonsterAtClickPosition();

        if (target is null)
        {
            return true;
        }
        
        target.HasAttacked(_attackDamage);
        
        StatusEffect devilBulletDamageAmplification = new DevilBulletDamageAmplification(target.gameObject, _amplification);
        StatusEffectManager.Instance.AddStatusEffect(target.status, devilBulletDamageAmplification);

        SoundManager.Instance.PlaySFX(_bulletSound);
        
        _animator.Play("DevilBullet", -1, 0f);
        
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }

    public override void ExecuteCoolTimeCommand()
    {
        commandInvoker.AddCommand(new DevilCooldownCommand(this));
    }
}