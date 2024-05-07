

public class FSM<T>
{
    private IState<T> currentState;
    private T owner;

    public FSM(T owner)
    {
        this.owner = owner;
    }

    public void ChangeState(IState<T> newState)
    {
        if (currentState != null)
        {
            currentState.Exit(owner);
        }

        currentState = newState;
        currentState.Enter(owner);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute(owner);
        }
    }
}