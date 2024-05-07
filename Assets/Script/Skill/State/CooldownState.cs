public class CooldownState : IState<Skill>
{
    public void Enter(Skill skill)
    {
        skill.CooldownEnter();
    }

    public void Execute(Skill skill)
    {
        skill.CooldownUpdate();
    }

    public void Exit(Skill skill)
    {
        skill.CooldownExit();
    }
}