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
        var targets = RangeDetectionUtility.GetAttackTargets(clickPosition, Data.Range, default, targetLayer);

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

    private float _currentCoolTime = 0;
    
    public override float CurrentCoolTime
    {
        get => base.CurrentCoolTime;
        set
        {
            _currentCoolTime = value;
            
            if (_currentCoolTime <= 0)
            {
                Stack++;
                
                _currentCoolTime = _coolTimes[Stack];
            }
        }
    }

    #endregion
}