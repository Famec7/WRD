using System;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaActive : ClickTypeSkill
{
    #region Skill Data

    private StackCoolTime _stackCoolTime;

    private float _damageAmplification = 0.0f;
    private float _amplificationDuration = 0.0f;
    private float _stunDuration = 0.0f;
    private float _damage = 0.0f;
    private readonly int baseStunIndex = 2;

    [SerializeField] private List<float> _ranges;

    #endregion

    protected override void Init()
    {
        base.Init();
        _stackCoolTime = new StackCoolTime(this, _coolTimes.Count)
        {
            OnStackMax = () =>
            {
                if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
                {
                    commandInvoker.AddCommand(new CheckForEnemiesCommand(this as ClickTypeSkill));
                }
            },
            OnStackChange = SetSlowRange + SetIndicatorRange
        };
        
        SetIndicatorRange(_ranges[0]);
    }

    public override void OnActiveEnter()
    {
        _damage = Data.GetValue(_stackCoolTime.Stack - 1);
        _damageAmplification = Data.GetValue(7) / 100.0f;
        _amplificationDuration = Data.GetValue(6);
        _stunDuration = Data.GetValue(baseStunIndex + _stackCoolTime.Stack);
    }

    public override bool OnActiveExecute()
    {
        List<Monster> targets = GetTargetMonsters();
        
        foreach (var target in targets)
        {
            target.HasAttacked(_damage);

            Stun(target.status, _stunDuration);
            DamageAmplification(target.status, _damageAmplification, _amplificationDuration);
        }

        return true;
    }

    public override void OnActiveExit()
    {
        _stackCoolTime.Stack = 0;
    }

    private void Stun(Status status, float duration)
    {
        StatusEffect stun = new SlowDown(status.gameObject, 100f, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, stun);
    }

    private void DamageAmplification(Status status, float amplification, float duration)
    {
        StatusEffect devilBulletDamageAmplification =
            new DamageAmplification(status.gameObject, amplification, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, devilBulletDamageAmplification);
    }

    public Action<float> SetSlowRange { get; set; }
    
    private void SetIndicatorRange()
    {
        if (_stackCoolTime.Stack >= _coolTimes.Count)
        {
            return;
        }
        
        SetIndicatorRange(_ranges[_stackCoolTime.Stack]);
    }

    #region CoolTime Command

    [Space] [SerializeField] private List<float> _coolTimes;

    public override void ExecuteCoolTimeCommand()
    {
        commandInvoker.AddCommand(new StackedCooldownCommand(this, _coolTimes, _stackCoolTime));
    }

    #endregion
}