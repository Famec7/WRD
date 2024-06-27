public abstract class ControlState : IState<PlayerController>
{
    public abstract void Enter(PlayerController entity);

    public abstract void Execute(PlayerController entity);

    public abstract void Exit(PlayerController entity);
}