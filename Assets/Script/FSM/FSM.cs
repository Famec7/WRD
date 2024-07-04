

public class FSM<T>
{
    private IState<T> _currentState;
    private readonly T _owner;

    public FSM(T owner)
    {
        this._owner = owner;
    }

    public void ChangeState(IState<T> newState)
    {
        // currentState가 null이 아니면 Exit 호출
        _currentState?.Exit(_owner);

        _currentState = newState;
        _currentState.Enter(_owner);
    }

    public void Update()
    {
        _currentState?.Execute(_owner);
    }
}