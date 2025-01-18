using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilBullet : ClickTypeSkill
{
    [SerializeField] private AudioClip _bulletSound;
    
    private float _attackDamage = 0.0f;
    private float _amplification = 0.0f;
    private int _attackCount = 0;
    private float _attackSpeedIncrease = 0.0f;

    public override void OnActiveEnter()
    {
        _attackDamage = Data.GetValue(0);
        _amplification = Data.GetValue(1) / 100.0f;
        _attackCount = (int)Data.GetValue(2);
        _attackSpeedIncrease = weapon.Data.AttackSpeed + weapon.Data.AttackSpeed * Data.GetValue(3);
    }

    public override bool OnActiveExecute()
    {
        Monster target = SelectMonsterAtClickPosition();

        if (target is null)
        {
            return true;
        }
        
        StatusEffect markStatus = StatusEffectManager.Instance.GetStatusEffect(target.status, typeof(Mark));

        if (markStatus != null)
        {
            if (target.CompareTag("Boss"))
            {
                weapon.SetAttackDelay(_attackSpeedIncrease);
                weapon.AddAction(OnAttack);
            }
            else if (target.CompareTag("Monster"))
            {
                target.Die();
            }
        }
        else
        {
            StatusEffect status = new Mark(target.gameObject);
            StatusEffectManager.Instance.AddStatusEffect(target.status, status);
        }

        target.HasAttacked(_attackDamage);
        
        StatusEffect devilBulletDamageAmplification = new DevilBulletDamageAmplification(target.gameObject, _amplification);
        StatusEffectManager.Instance.AddStatusEffect(target.status, devilBulletDamageAmplification);

        SoundManager.Instance.PlaySFX(_bulletSound);
        
        return true;
    }

    public override void OnActiveExit()
    {
        if (weapon.GetPassiveAuraSkill() is Devil7 devil7)
        {
            devil7.AttackCount = 0;
        }
    }

    private void OnAttack()
    {
        _attackCount--;

        if (_attackCount <= 0)
        {
            weapon.SetAttackDelay(weapon.Data.AttackSpeed);
            weapon.RemoveAction(OnAttack);
        }
    }

    #region Behavior Tree

    /*protected override List<INode> CoolTimeNodes()
    {
        return new List<INode>
        {
            new ActionNode(CheckCoolTimeState), new ActionNode(CoolTimeDown), new ActionNode(OnCoolTimeEnd)
        };
    }

    private new INode.ENodeState CoolTimeDown()
    {
        if (weapon.GetPassiveAuraSkill() is Devil7 devil7)
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
        float checkTime = weapon.GetPassiveAuraSkill().Data.GetValue(1);
        yield return new WaitForSeconds(checkTime);

        if (IsActive is false)
        {
            CancelSkill();
        }
    }*/

    #endregion

    public override void ExecuteCoolTimeCommand()
    {
        commandInvoker.AddCommand(new DevilCooldownCommand(this));
    }
}