using UnityEngine;

public interface IPool
{
    Component Component { get; }
    
    int Count { get; }
    
    Component Get();
    
    void Return(Component clone);
}

public interface IPool<out T> : IPool where T : Component
{
    new T Component { get; }
    
    new int Count { get; }

    new T Get();

    IPool<T> Clear();
}