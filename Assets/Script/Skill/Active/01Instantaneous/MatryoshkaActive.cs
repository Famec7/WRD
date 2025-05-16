using System;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaActive : InstantaneousSkill
{
    #region Skill Data
    
    private float _damageAmplification = 0.0f;
    private float _amplificationDuration = 0.0f;
    private float _stunDuration = 0.0f;
    private float _damage = 0.0f;
    private float _range = 0.0f;

    private const int BaseStunIndex = 3;

    [SerializeField] private List<float> _ranges;

    [Space] [SerializeField] private MatryoshkaSpriteChanger _matryoshkaSpriteChanger;

    #endregion

    private int _stackLevel = 0;

    private int _levelDelta = 1;
    private const int MaxStackLevel = 3;

    protected override void Init()
    {
        base.Init();

        _amplificationDuration = Data.GetValue(6);
        _damageAmplification = Data.GetValue(7) / 100.0f;
        
        SetRange(_stackLevel);
    }

    public override void OnActiveEnter()
    {
        _damage = Data.GetValue(_stackLevel);
        _stunDuration = Data.GetValue(BaseStunIndex + _stackLevel);
        _range = _ranges[_stackLevel];
    }

    public override bool OnActiveExecute()
    {
        var monsters = RangeDetectionUtility.GetAttackTargets(transform.position, _range, default, targetLayer);

        foreach (var target in monsters)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(_damage);

                Stun(monster.status, _stunDuration);
                DamageAmplification(monster.status, _damageAmplification, _amplificationDuration);
            }
        }

        return true;
    }

    public override void OnActiveExit()
    {
        _stackLevel += _levelDelta;

        if (_stackLevel >= MaxStackLevel - 1)
        {
            _levelDelta = -1;
        }
        else if (_stackLevel <= 0)
        {
            _levelDelta = 1;
        }

        SetRange(_stackLevel);
    }

    private void Stun(Status status, float duration)
    {
        StatusEffect stun = new Stun(status.gameObject, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, stun);
    }

    private void DamageAmplification(Status status, float amplification, float duration)
    {
        StatusEffect damageAmplification = new DamageAmplification(status.gameObject, amplification, duration);
        StatusEffectManager.Instance.AddStatusEffect(status, damageAmplification);
    }

    public Action<int> SetSlowRange { get; set; }

    private void SetRange(int stack)
    {
        if (_stackLevel < 0 || _stackLevel >= MaxStackLevel)
        {
            Debug.LogError("Stack level out of range.");
            return;
        }

        SetSprite(stack);
        SetSlowRange?.Invoke(stack);
    }

    private void SetSprite(int stack)
    {
        if (_matryoshkaSpriteChanger == null)
        {
            Debug.LogError("MatryoshkaSpriteChanger is not assigned.");
            return;
        }

        _matryoshkaSpriteChanger.ChangeMatryoshikaSprite(stack + 1);
    }

    public override void ExecuteSkill()
    {
        base.ExecuteSkill();
        
        _stackLevel = 0;
        SetRange(0);
    }
}