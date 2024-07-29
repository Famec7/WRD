public class CoolTimeState : IState<ActiveSkillBase>
{
    public void Enter(ActiveSkillBase entity)
    {
        entity.OnCoolTimeEnter();
    }

    public void Execute(ActiveSkillBase entity)
    {
        entity.OnCoolTimeExecute();
    }

    public void Exit(ActiveSkillBase entity)
    {
        entity.OnCoolTimeExit();
    }
}