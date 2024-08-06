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
    public static Pool<T> CreatePool<T>(T prefab, int count) where T : Component
    {
        return Pool<T>.CreatePool(prefab, count);
    }
}

public class Pool<T> where T : Component
{
    private T _prefab;

    public T Prefab => _prefab;

    private int _count;

    public int Count => _count;

    private List<T> _clones = new List<T>();

    private readonly List<T> _activeClones = new List<T>();

    internal static Pool<T> CreatePool(T prefab, int count)
    {
        count = Mathf.Max(count, 0);

        Pool<T> pool = new Pool<T>
        {
            _prefab = prefab,
            _count = count,
            _clones = new(count)
        };

        return pool;
    }

    public Pool<T> Create()
    {
        while (_clones.Count < _count)
        {
            T clone = Object.Instantiate(_prefab);
            clone.gameObject.SetActive(false);
            
            _clones.Add(clone);
        }

        return this;
    }

    public Pool<T> Clear()
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
        for (int i = 0; i < _clones.Count; i++)
        {
            if (!_activeClones.Contains(_clones[i]))
            {
                clone = _clones[i];
                break;
            }
        }

        // 오브젝트가 부족하면 새로 생성 / 생성된 오브젝트가 최대치면 null 반환
        if (clone == null)
        {
            if (_clones.Count >= _count)
            {
                return null;
            }

            clone = Object.Instantiate(_prefab);
            _clones.Add(clone);
        }

        _activeClones.Add(clone);

        clone.gameObject.SetActive(true);
        if (clone is IPoolObject poolObject)
            poolObject.GetFromPool();

        return clone;
    }

    public void Return(T clone)
    {
        // 오브젝트가 풀에 없으면 예외 발생
        if (!_clones.Contains(clone))
        {
            throw new Exception("ObjectPool: Return() - The object is not in the pool.");
            return;
        }

        // 오브젝트가 활성화되지 않았으면 예외 발생
        if (!_activeClones.Contains(clone))
        {
            throw new Exception("ObjectPool: Return() - The object is already inactive.");
        }

        _activeClones.Remove(clone);

        clone.gameObject.SetActive(false);
        if (clone is IPoolObject poolObject)
            poolObject.ReturnToPool();
    }
}