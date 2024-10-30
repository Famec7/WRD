using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attaque : ClickTypeSkill
{
    #region Data

    private float _duration;
    private float _damage;
    private float _passiveChance;
    private float _originPassiveChance;
    #endregion

    protected override void Init()
    {
        _damage = Data.GetValue(1);
        _duration = Data.GetValue(2);
        _passiveChance = Data.GetValue(3);
        _originPassiveChance = weapon.passiveSkill.Data.Chance;
    }

    protected override void OnActiveEnter()
    {
    
        if (pivotPosition == Vector2.zero)
        {
            // 스킬 범위 안에 적이 있으면 타겟을 적으로 설정
            var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Data.Range, default, targetLayer);

            if (targets.Count > 0)
            {
                pivotPosition = targets[0].transform.position;
            }

            else
            {
                IsActive = false;
                return;
            }
        }

    }

    protected override INode.ENodeState OnActiveExecute()
    {
        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {
        ;
    }
}
