using UnityEngine;

public class SilentTrueshot : ClickTypeSkill
{
    private float _focusTime = 0.0f;
    private float _minDamage = 0.0f;
    private float _maxDamage = 0.0f;

    private float _damage = 0.0f;

    protected override void Init()
    {
        base.Init();

        _focusTime = Data.GetValue(0);
        _minDamage = Data.GetValue(1);
        _maxDamage = Data.GetValue(2);
    }

    protected override void IndicatorInit()
    {
        base.IndicatorInit();
        
        ShrinkingTriangleIndicator indicator = Indicator as ShrinkingTriangleIndicator;
        if (indicator != null)
        {
            indicator.OnShrinkEnd = () =>
            {
                Undo();
                AddCommand(new ActiveSkillCommand(this));
            };
        }
    }

    public override void OnActiveEnter()
    {
        ShrinkingTriangleIndicator indicator = Indicator as ShrinkingTriangleIndicator;

        if (indicator == null)
        {
            Debug.LogError("Indicator is null");
            return;
        }

        _damage = CalculateDamage(indicator.ElapsedTime);
    }

    public override bool OnActiveExecute()
    {
        foreach (var monster in IndicatorMonsters)
        {
            monster.HasAttacked(_damage);
        }

        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }

    private float CalculateDamage(float elapsedTime)
    {
        float step = (_maxDamage - _minDamage) / _focusTime;

        return _minDamage + step * elapsedTime;
    }

    private bool HasTargetMark(Monster monster)
    {
        StatusEffect mark = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

        return mark != null;
    }
}