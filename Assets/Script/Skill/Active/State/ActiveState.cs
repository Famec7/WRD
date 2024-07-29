public class ActiveState : IState<ActiveSkillBase>
{
    public void Enter(ActiveSkillBase entity)
    {
        entity.OnActiveEnter();
    }

    public void Execute(ActiveSkillBase entity)
    {
        entity.OnActiveExecute();
    }

    public void Exit(ActiveSkillBase entity)
    {
        entity.OnActiveExit();
    }
}