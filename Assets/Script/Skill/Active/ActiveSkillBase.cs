using System;

public abstract class ActiveSkillBase : SkillBase
{
    private FSM<ActiveSkillBase> _fsm;
    private SkillState<ActiveSkillBase> _castingState;
    private SkillState<ActiveSkillBase> _coolTimeState;
    private SkillState<ActiveSkillBase> _activeState;
    public string skillType;
    public bool buttonClicked = false;
    protected override void Init()
    {
        base.Init();
        
        _fsm = new FSM<ActiveSkillBase>(this);
        _castingState = new SkillState<ActiveSkillBase>(OnCastingEnter, OnCastingExecute, OnCastingExit);
        _coolTimeState = new SkillState<ActiveSkillBase>(OnCoolTimeEnter, OnCoolTimeExecute, OnCoolTimeExit);
        _activeState = new SkillState<ActiveSkillBase>(OnActiveEnter, OnActiveExecute, OnActiveExit);
        
        _fsm.ChangeState(_castingState);
    }

    private void Update()
    {
        _fsm.Update();
    }
    
    /// <summary>
    /// 아래 3개의 메서드는 상속받은 클래스에서 구현 필수!!!
    /// </summary>
    /// <param name="skill"></param>
    # region Casting State

    protected abstract void OnCastingEnter(ActiveSkillBase skill);
    protected abstract void OnCastingExecute(ActiveSkillBase skill);
    protected abstract void OnCastingExit(ActiveSkillBase skill);

    # endregion
    
    # region CoolTime State
    
    protected abstract void OnCoolTimeEnter(ActiveSkillBase skill);
    protected abstract void OnCoolTimeExecute(ActiveSkillBase skill);
    protected abstract void OnCoolTimeExit(ActiveSkillBase skill);
    
    # endregion
    
    # region Active State
    
    protected abstract void OnActiveEnter(ActiveSkillBase skill);
    protected abstract void OnActiveExecute(ActiveSkillBase skill);
    protected abstract void OnActiveExit(ActiveSkillBase skill);
    
    # endregion
}