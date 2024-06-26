public interface IState<in T>
{
    void Enter(T entity);
    void Execute(T entity);
    void Exit(T entity);
}