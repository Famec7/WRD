using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodOfThunder : InstantaneousSkill
{
    private float _timer = 0.0f;

    private int _originPassiveChance;
    private float _originAttackRange;
    
    [SerializeField]
    private float _chainAttackRange = 7.0f;
    
    private ParticleEffect _electricAura = null;
    
    #region Data

    private float _duration;
    private float _range;
    private float _damage;
    private int _passiveChance;
    private float _stunDuration;

    #endregion

    protected override void Init()
    {
        base.Init();
        
        _originPassiveChance = weapon.passiveSkill.Data.Chance;
        _originAttackRange = weapon.Data.AttackRange;
        
        _duration = Data.GetValue(0);
        _range = Data.GetValue(1);
        _passiveChance = (int)Data.GetValue(2);
        _damage = Data.GetValue(3);
        _stunDuration = Data.GetValue(4);
    }

    protected override void OnActiveEnter()
    {
        _timer = 0.0f;

        // 패시브 스킬 확률을 변경
        weapon.passiveSkill.Data.Chance = _passiveChance;
        
        // 무기의 공격 범위를 변경
        weapon.SetAttackDelay(_range);
        
        _electricAura = EffectManager.Instance.CreateEffect<ParticleEffect>("ElectricAura");
        _electricAura.SetPosition(weapon.owner.transform.position);
        _electricAura.transform.SetParent(weapon.owner.transform, false);
        
        _electricAura.PlayEffect();
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        _timer += Time.deltaTime;
        if (_timer >= _duration)
        {
            IsActive = false;
            return INode.ENodeState.Success;
        }
        
        if(weapon.owner.IsTargetNullOrInactive())
        {
            return INode.ENodeState.Running;
        }
        
        ChainAttack();
        
        return INode.ENodeState.Running;
    }

    protected override void OnActiveExit()
    {
        ResetStat();
        
        if(_electricAura is null)
            return;
        
        _electricAura.StopEffect();
        _electricAura = null;
    }

    private void ResetStat()
    {
        // 패시브 스킬 확률을 원래대로 변경
        weapon.passiveSkill.Data.Chance = _originPassiveChance;
        
        // 무기의 공격 범위를 원래대로 변경
        weapon.SetAttackDelay(_originAttackRange);
    }

    #region ChainAttack

    private void ChainAttack()
    {
        Vector3 pos = weapon.owner.Target.transform.position;
        var targets = RangeDetectionUtility.GetAttackTargets(pos, _chainAttackRange, default, targetLayer);

        StopCoroutine(IE_ChainAttack(targets));
    }

    private IEnumerator IE_ChainAttack(List<Collider2D> targets)
    {
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Status status))
            {
                if(status.IsElectricShock is false)
                    continue;
                
                target.GetComponent<Monster>().HasAttacked(_damage);
                
                StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(ElectricShock));
                StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, 100f, _stunDuration));
                
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    #endregion
}