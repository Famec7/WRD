using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Pool
{
    /// <summary>
    /// Pool을 생성하는 함수
    /// </summary>
    /// <param name="prefab"> 원본 프리펩 넣으면 됨 </param>
    /// <param name="count"> 원하는 개수 넣으면 됨 </param>
    /// <typeparam name="T"> 어떤 컴포넌트로 넣을지 정하면 됨 </typeparam>
    /// <returns> Pool 클래스를 반환 </returns>
    public static Pool<T> CreatePool<T>(T prefab, int count, string name) where T : Component
    {
        return Pool<T>.CreatePool(prefab, count, name);
    }
}

public class Pool<T> : IPool<T> where T : Component
{
    Component IPool.Component => Component;
    
    public string Name { get; private set; }
    
    public T Component { get; private set; }

    public int Count { get; private set; }

    private List<T> _clones = new List<T>();

    private readonly List<T> _activeClones = new List<T>();

    internal static Pool<T> CreatePool(T prefab, int count, string name)
    {
        count = Mathf.Max(count, 0);

        Pool<T> pool = new Pool<T>
        {
            Component = prefab,
            Count = count,
            _clones = new(count),
            Name = name
        };

        return pool;
    }

    public Pool<T> Create()
    {
        while (_clones.Count < Count)
        {
            T clone = Object.Instantiate(Component, Vector3.zero, Quaternion.identity);
            clone.gameObject.SetActive(false);
            clone.name = Name;
            
            _clones.Add(clone);
        }

        return this;
    }

    Component IPool.Get() => Get();
    
    public IPool<T> Clear()
    {
        foreach (var clone in _clones)
        {
            Object.Destroy(clone.gameObject);
        }

        _clones.Clear();

        return this;
    }

    public T Get()
    {
        T clone = null;

        // 비활성화된 오브젝트를 찾아서 반환
        foreach (var disabledClone in _clones)
        {
            if (!_activeClones.Contains(disabledClone))
            {
                clone = disabledClone;
                break;
            }
        }

        // 오브젝트가 부족하면 새로 생성 / 생성된 오브젝트가 최대치면 null 반환
        if (clone == null)
        {
            if (_clones.Count >= Count)
            {
                return null;
            }

            clone = Object.Instantiate(Component);
            clone.name = Name;
            _clones.Add(clone);
        }

        _activeClones.Add(clone);

        clone.gameObject.SetActive(true);
        if (clone is IPoolObject poolObject)
            poolObject.GetFromPool();

        return clone;
    }

    void IPool.Return(Component clone) => Return(clone as T);
    
    public void Return(T clone)
    {
        // 오브젝트가 풀에 없으면 예외 발생
        if (!_clones.Contains(clone))
        {
            throw new Exception("ObjectPool: Return(" + clone.name + ") - The object is not in the pool.");
            return;
        }

        // 오브젝트가 활성화되지 않았으면 예외 발생
        if (!_activeClones.Contains(clone))
        {
            Debug.LogWarning("ObjectPool: Return(" + clone.name + ") - The object is already inactive.");
            return;
        }

        _activeClones.Remove(clone);

        clone.gameObject.SetActive(false);
        if (clone is IPoolObject poolObject)
            poolObject.ReturnToPool();
    }
}