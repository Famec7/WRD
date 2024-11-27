using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilBullet : ClickTypeSkill
{
    private int _attackCount = 0;

    protected override void OnActiveEnter()
    {
        FindTarget();

        if (target is null)
        {
            return;
        }

        if (target.TryGetComponent(out Monster monster))
        {
            StatusEffect markStatus = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

            if (markStatus != null)
            {
                if (monster.CompareTag("Boss"))
                {
                    float attackSpeed = weapon.Data.AttackSpeed + weapon.Data.AttackSpeed * Data.GetValue(3);
                    weapon.SetAttackDelay(attackSpeed);

                    weapon.AddAction(OnAttack);
                }
                else if (monster.CompareTag("Monster"))
                {
                    monster.Die();
                }
            }
            else
            {
                StatusEffect status = new Mark(monster.gameObject);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, status);
            }

            monster.HasAttacked(Data.GetValue(0));

            float amplification = Data.GetValue(1) / 100;
            StatusEffect devilBulletDamageAmplification =
                new DevilBulletDamageAmplification(monster.gameObject, amplification);
            StatusEffectManager.Instance.AddStatusEffect(monster.status, devilBulletDamageAmplification);
        }
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {
        if (weapon.passiveAuraSkill is Devil7 devil7)
        {
            devil7.AttackCount = 0;
        }
    }

    private void OnAttack()
    {
        _attackCount++;

        if (_attackCount >= Data.GetValue(2))
        {
            weapon.SetAttackDelay(weapon.Data.AttackSpeed);
            weapon.RemoveAction(OnAttack);
        }
    }

    #region Behavior Tree

    protected override List<INode> CoolTimeNodes()
    {
        return new List<INode>
        {
            new ActionNode(CheckCoolTimeState), new ActionNode(CoolTimeDown), new ActionNode(OnCoolTimeEnd)
        };
    }

    private new INode.ENodeState CoolTimeDown()
    {
        if (weapon.passiveAuraSkill is Devil7 devil7)
        {
            CurrentCoolTime = devil7.Data.GetValue(0) - devil7.AttackCount;
        }

        return CurrentCoolTime <= 0 ? INode.ENodeState.Success : INode.ENodeState.Running;
    }

    protected override INode.ENodeState OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();

        StartCoroutine(IE_CheckSkillInactivity());

        return INode.ENodeState.Success;
    }

    private IEnumerator IE_CheckSkillInactivity()
    {
        float checkTime = weapon.passiveAuraSkill.Data.GetValue(1);
        yield return new WaitForSeconds(checkTime);

        if (IsActive is false)
        {
            CancelSkill();
        }
    }

    #endregion
}