public class ReadyState : IState<Skill>
{
    public void Enter(Skill skill)
    {
        skill.ReadyEnter();
    }

    public void Execute(Skill skill)
    {
        skill.ReadyUpdate();
    }

    public void Exit(Skill skill)
    {
        skill.ReadyExit();
    }
}