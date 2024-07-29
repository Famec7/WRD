public class CastingState : IState<ActiveSkillBase>
{
    public void Enter(ActiveSkillBase entity)
    {
        entity.OnCastingEnter();
    }

    public void Execute(ActiveSkillBase entity)
    {
        entity.OnCastingExecute();
    }

    public void Exit(ActiveSkillBase entity)
    {
        entity.OnCastingExit();
    }
}