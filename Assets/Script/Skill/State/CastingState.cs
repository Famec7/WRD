public class CastingState : IState<Skill>
{
    public void Enter(Skill skill)
    {
        skill.CastingEnter();
    }

    public void Execute(Skill skill)
    {
        skill.CastingUpdate();
    }

    public void Exit(Skill skill)
    {
        skill.CastingExit();
    }
}