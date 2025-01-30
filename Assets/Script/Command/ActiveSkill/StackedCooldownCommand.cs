using System.Collections.Generic;

[System.Serializable]
public class StackCoolTime
{
    private readonly ActiveSkillBase _skill;

    public StackCoolTime(ActiveSkillBase skill, int maxStack)
    {
        _skill = skill;
        _maxStack = maxStack;
    }

    private readonly int _maxStack;
    private int _stack;

    public System.Action OnStackMax; // 스택이 최대치에 도달했을 때
    public System.Action<int> OnStackChange; // 스택이 변할 때

    public int Stack
    {
        get => _stack;
        set
        {
            _stack = value;

            if (_stack > 0)
            {
                _skill.OnButtonActivate?.Invoke(true);
            }
            else
            {
                _skill.CurrentCoolTime = _skill.Data.CoolTime;
                _skill.OnButtonActivate?.Invoke(false);
            }

            if (_stack >= _maxStack)
            {
                _stack = _maxStack;
                OnStackMax?.Invoke();
            }

            OnStackChange?.Invoke(_stack);
        }
    }
}

public class StackedCooldownCommand : CooldownCommand
{
    private readonly List<float> _coolTimes;
    private readonly StackCoolTime _stackCoolTime;

    public StackedCooldownCommand(ActiveSkillBase skill, List<float> coolTimes, StackCoolTime stackCoolTime) :
        base(skill)
    {
        _coolTimes = coolTimes;
        _stackCoolTime = stackCoolTime;
        
        skill.Data.CoolTime = _coolTimes[_stackCoolTime.Stack];
        skill.CurrentCoolTime = skill.Data.CoolTime;
    }

    public override void OnComplete()
    {
        _stackCoolTime.Stack++;
        
        if (_stackCoolTime.Stack >= _coolTimes.Count)
        {
            return;
        }

        skill.CurrentCoolTime = _coolTimes[_stackCoolTime.Stack];
        skill.AddCommand(new StackedCooldownCommand(skill, _coolTimes, _stackCoolTime));
    }
}