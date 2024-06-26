using System;

public class SkillState<T> : IState<T>
{
    private readonly Action<T> _enterAction;
    private readonly Action<T> _executeAction;
    private readonly Action<T> _exitAction;
    
    public SkillState(Action<T> enterAction, Action<T> executeAction, Action<T> exitAction)
    {
        _enterAction = enterAction;
        _executeAction = executeAction;
        _exitAction = exitAction;
    }
    
    public void Enter(T entity)
    {
        _enterAction?.Invoke(entity);
    }

    public void Execute(T entity)
    {
        _executeAction?.Invoke(entity);
    }

    public void Exit(T entity)
    {
        _exitAction?.Invoke(entity);
    }
}