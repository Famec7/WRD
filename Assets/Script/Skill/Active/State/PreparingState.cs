public class PreparingState : IState<ActiveSkillBase>
{
    public void Enter(ActiveSkillBase entity)
    {
        entity.OnPreparingEnter();
    }

    public void Execute(ActiveSkillBase entity)
    {
        entity.OnPreparingExecute();
    }

    public void Exit(ActiveSkillBase entity)
    {
        entity.OnPreparingExit();
    }
}