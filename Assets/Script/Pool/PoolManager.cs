using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PoolData
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private Component _prefab;
    public Component Prefab => _prefab;

    [SerializeField] [Min(0)] private int _count;
    public int Count => _count;
}

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<PoolData> _poolDatas = new();
    
    private readonly List<Pool<Component>> _pools = new();
    
    private void Awake()
    {
        foreach (var poolData in _poolDatas)
        {
            var pool = Pool.CreatePool(poolData.Prefab, poolData.Count);
            pool.Create();
            _pools.Add(pool);
        }
    }

    #region GetPool

    /// <summary>
    /// T 타입의 Pool을 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Pool<T> GetPool<T>() where T : Component => _pools.Find(pool => pool.Prefab is T) as Pool<T>;
    
    /// <summary>
    /// name에 해당하는 Pool을 반환
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Pool<T> GetPool<T>(string name) where T : Component  => _pools[_poolDatas.FindIndex(poolData => poolData.Name == name)] as Pool<T>;
    
    /// <summary>
    /// index에 해당하는 Pool을 반환
    /// </summary>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Pool<T> GetPool<T>(int index) where T : Component => _pools[index] as Pool<T>;

    #endregion

    #region Get From Pool

    /// <summary>
    /// T 타입의 Pool에서 Component를 가져옴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetFromPool<T>() where T : Component => GetPool<T>().Get();
    
    /// <summary>
    /// name에 해당하는 Pool에서 Component를 가져옴
    /// </summary>
    /// <param name="name"> PoolData에 해당하는 이름 </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetFromPool<T>(string name) where T : Component => GetPool<T>(name).Get();
    
    /// <summary>
    /// index번째 Pool에서 Component를 가져옴
    /// </summary>
    /// <param name="index"> _pools의 index </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetFromPool<T>(int index) where T : Component => GetPool<T>(index).Get();

    #endregion

    #region Return To Pool

    /// <summary>
    /// T 타입의 Pool에 Component를 반환
    /// </summary>
    /// <param name="clone"></param>
    /// <typeparam name="T"></typeparam>
    public void ReturnToPool<T>(T clone) where T : Component => GetPool<T>().Return(clone);
    
    /// <summary>
    /// T 타입의 Pool이 여러개 있으면 name에 해당하는 Pool에 Component를 반환
    /// </summary>
    /// <param name="name"></param>
    /// <param name="clone"></param>
    /// <typeparam name="T"></typeparam>
    public void ReturnToPool<T>(string name, T clone) where T : Component => GetPool<T>(name).Return(clone);
    
    /// <summary>
    /// T 타입의 Pool을 index번 째 Pool에 Component를 반환
    /// </summary>
    /// <param name="index"></param>
    /// <param name="clone"></param>
    /// <typeparam name="T"></typeparam>
    public void ReturnToPool<T>(int index, T clone) where T : Component => GetPool<T>(index).Return(clone);

    #endregion
}