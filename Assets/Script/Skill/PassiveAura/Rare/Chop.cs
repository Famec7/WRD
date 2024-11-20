using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chop : PassiveAuraSkillBase, IObserver
{
    [SerializeField]
    private AudioClip _chopSound;
    
    private int _attackCount = 0;
    private void OnAttack()
    {
        _attackCount++;

        if (weapon.owner.Target.TryGetComponent(out Monster monster))
        { 
            if (_attackCount == Data.GetValue(0))
            {
                monster.HasAttacked(Data.GetValue(1));
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new SlowDown(monster.gameObject, 100f, Data.GetValue(2)));

                ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("SharpBladeEffect");
                effect.SetPosition(weapon.owner.Target.transform.position);

                _attackCount = 0;
            }
            else
            {
                monster.HasAttacked(weapon.Data.AttackDamage);
                ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
                effect.SetPosition(weapon.owner.Target.transform.position);
            }
            
            SoundManager.Instance.PlaySFX(_chopSound);
        }

    }

    public void OnNotify()
    {
        _attackCount++;

        if (_attackCount == Data.GetValue(0))
        {
            weapon.OnAttack += OnAttack;
            _attackCount = 0;
        }
    }

    private void Start()
    {
        weapon.OnAttack += OnAttack;
    }
}
