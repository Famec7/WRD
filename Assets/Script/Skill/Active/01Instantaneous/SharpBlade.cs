using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpBlade : InstantaneousSkill
{
    #region Data

    private float _damage;
    private float _woundDamge;

    [SerializeField] private Vector3 _range = new Vector3(1.5f, 3.5f, 1);

    #endregion

    protected override void Init()
    {
        base.Init();
        
        _damage = Data.GetValue(0);
        _woundDamge = Data.GetValue(1);
    }
    
    protected override void OnActiveEnter()
    {
        ;
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        if (weapon.owner.Target is null)
        {
            return INode.ENodeState.Running;
        }
        
        weapon.enabled = false;
        
        Vector3 dir = weapon.owner.Target.transform.position - weapon.owner.transform.position;

        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, _range, dir, targetLayer);

        // 이펙트 재생
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("SharpBladeEffect");

        effect.SetPosition(weapon.owner.transform.position + dir);
        effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.left, dir)));
        effect.PlayEffect();

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
                    monster.HasAttacked(_woundDamge);
                }
            }
        }

        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {
        weapon.enabled = true;
    }
}