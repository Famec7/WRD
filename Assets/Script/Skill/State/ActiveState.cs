public class ActiveState : IState<Skill>
{
    public void Enter(Skill skill)
    {
        skill.ActiveEnter();
    }

    public void Execute(Skill skill)
    {
        skill.ActiveUpdate();
    }

    public void Exit(Skill skill)
    {
        skill.ActiveExit();
    }
}