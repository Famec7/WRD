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
    [Space] [SerializeField] private MatryoshkaSpriteChanger _matryoshkaSpriteChanger;

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
            OnStackChange = SetRange
        };

        SetRange(0);
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
        foreach (var target in IndicatorMonsters)
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
        StatusEffect stun = new Stun(status.gameObject, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, stun);
    }

    private void DamageAmplification(Status status, float amplification, float duration)
    {
        StatusEffect devilBulletDamageAmplification =
            new DamageAmplification(status.gameObject, amplification, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, devilBulletDamageAmplification);
    }

    public Action<float> SetSlowRange { get; set; }

    private void SetRange(int stack)
    {
        if (stack >= _coolTimes.Count)
        {
            return;
        }

        SetIndicatorRange(_ranges[stack]);
        SetSprite(stack);
        
        SetSlowRange?.Invoke(_ranges[stack]);
    }
    
    private void SetSprite(int stack)
    {
        if (stack >= _coolTimes.Count)
        {
            return;
        }
        
        _matryoshkaSpriteChanger.ChangeMatryoshikaSprite(stack + 1);
    }

    #region CoolTime Command

    [Space] [SerializeField] private List<float> _coolTimes;

    public override void ExecuteCoolTimeCommand()
    {
        commandInvoker.AddCommand(new StackedCooldownCommand(this, _coolTimes, _stackCoolTime));
    }

    #endregion
}