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

    [Header("스킬 이펙트")] [SerializeField] private GameObject _effect;

    [SerializeField] private AudioClip sfx;

    private int _stackLevel = 0;

    private int _levelDelta = 1;
    private const int MaxStackLevel = 3;

    protected override void Init()
    {
        base.Init();

        _amplificationDuration = Data.GetValue(6);
        _damageAmplification = Data.GetValue(7);

        SetRange(_stackLevel);
    }

    public override void OnActiveEnter()
    {
        _damage = Data.GetValue(_stackLevel);
        _stunDuration = Data.GetValue(BaseStunIndex + _stackLevel);

        if (_effect.TryGetComponent(out Animator animator))
        {
            animator.Play("ActiveEffect", -1, 0f);
        }

        SoundManager.Instance.PlaySFX(sfx);
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
        _stackLevel++;

        if (_stackLevel >= MaxStackLevel)
        {
            _stackLevel = 0;
        }

        SetRange(_stackLevel);

        _range = _ranges[_stackLevel];

        _effect.transform.localScale = new Vector3(_range, _range, 1);
        _effect.SetActive(true);
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

        if (_effect.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            switch (_stackLevel)
            {
                case 0:
                    spriteRenderer.color = Color.blue;
                    break;
                case 1:
                    spriteRenderer.color = Color.yellow;
                    break;
                case 2:
                    spriteRenderer.color = Color.red;
                    break;
                default:
                    break;
            }
        }
    }

    public override void ExecuteSkill()
    {
        base.ExecuteSkill();

        _stackLevel = 0;
        SetRange(0);
    }

    private void OnDisable()
    {
        _effect.SetActive(false);
    }
}