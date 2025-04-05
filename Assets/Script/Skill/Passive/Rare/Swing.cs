using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Swing : PassiveSkillBase
{
    private float _skillDamage;
    
    [SerializeField]
    private Vector2 _hitSize;
    
    [SerializeField]
    private AudioClip _swingSound;
    
    protected override void Init()
    {
        base.Init();
        
        _skillDamage = Data.GetValue(0);
    }
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        Vector3 dir = weapon.owner.Target.transform.position - weapon.owner.transform.position;
        if (dir == Vector3.zero)
            return false;
        
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, _hitSize, dir, targetLayer);

        if(targets.Count == 0)
            return false;
        
        // 이펙트 재생
        ParticleEffect effect = GetSwingEffect();
        
        effect.SetPosition(weapon.owner.transform.position);
        effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir)));
        effect.PlayEffect();
        
        SoundManager.Instance.PlaySFX(_swingSound);
        
        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new Wound(tar.gameObject));
                TakeDamage(monster);
            }
        }

        return true;

    }

    protected virtual ParticleEffect GetSwingEffect()
    {
        return EffectManager.Instance.CreateEffect<ParticleEffect>("SwingEffect");
    }

    protected virtual void TakeDamage(Monster monster)
    {
        monster.HasAttacked(_skillDamage);
    }
}