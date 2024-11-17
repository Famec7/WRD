using System;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaActive : ClickTypeSkill
{
    #region Skill Data

    private int _stack = 0;

    private int Stack
    {
        get => _stack;
        set
        {
            _stack = value;

            if (_stack > 0)
            {
                OnButtonActivate?.Invoke(true);
            }
            else
            {
                CurrentCoolTime = Data.CoolTime;
                OnButtonActivate?.Invoke(false);
            }
            
            if (_stack >= _coolTimes.Count)
            {
                _stack = _coolTimes.Count;
                    
                if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
                {
                    IsActive = true;
                }
                    
                return;
            }
            
            SetSlowRange?.Invoke(_stack);
        }
    }

    #endregion
    
    protected override void OnActiveEnter()
    {
        FindTarget();
        
        if (target is null)
            return;
        
        var targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, Data.Range, default, targetLayer);
        
        if (targets.Count == 0)
            return;
        
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(Stack - 1));
                
                Stun(monster.status, Data.GetValue(2 + Stack));
                DamageAmplification(monster.status, Data.GetValue(7) / 100, Data.GetValue(6));
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
        Stack = -1;
    }
    
    private void Stun(Status status, float duration)
    {
        StatusEffect stun = new SlowDown(status.gameObject, 100f, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, stun);
    }
    
    private void DamageAmplification(Status status, float amplification, float duration)
    {
        StatusEffect devilBulletDamageAmplification = new DamageAmplification(status.gameObject, amplification, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, devilBulletDamageAmplification);
    }
    
    public Action<float> SetSlowRange { get; set; }

    #region Behavior Tree

    [Space]
    [SerializeField] private List<float> _coolTimes;

    protected override List<INode> CoolTimeNodes()
    {
        return new List<INode>
        {
            new ActionNode(CheckCoolTimeState),
            new ActionNode(CoolTimeDown),
            new ActionNode(OnCoolTimeEnd)
        };
    }
    
    private new INode.ENodeState OnCoolTimeEnd()
    {
        Stack++;
                
        if (Stack >= _coolTimes.Count)
        {
            IsCoolTime = false;
            return INode.ENodeState.Success;
        }
        
        CurrentCoolTime = _coolTimes[Stack];
        return INode.ENodeState.Success;
    }

    #endregion
}