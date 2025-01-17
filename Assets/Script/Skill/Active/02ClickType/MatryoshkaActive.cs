using System;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaActive : ClickTypeSkill
{
    #region Skill Data

    private StackCoolTime _stackCoolTime;

    #endregion
    
    public override void OnActiveEnter()
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
                monster.HasAttacked(Data.GetValue(_stackCoolTime.Stack - 1));
                
                Stun(monster.status, Data.GetValue(2 + _stackCoolTime.Stack));
                DamageAmplification(monster.status, Data.GetValue(7) / 100, Data.GetValue(6));
            }
        }
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        _stackCoolTime.Stack = -1;
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

    #region CoolTime Command

    [Space]
    [SerializeField] private List<float> _coolTimes;

    public override void ExecuteCoolTimeCommand()
    {
        _stackCoolTime = new StackCoolTime(this, _coolTimes.Count) { OnStackMax = () =>
            {
                if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
                {
                    _commandInvoker.AddCommand(new CheckForEnemiesCommand(this as ClickTypeSkill));
                }
            },
            OnStackChange = SetSlowRange
        };

        _commandInvoker.AddCommand(new StackedCooldownCommand(this, _coolTimes, _stackCoolTime));
    }

    #endregion
}