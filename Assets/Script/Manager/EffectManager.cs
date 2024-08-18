using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    private PoolManager _poolManager;
    
    protected override void Init()
    {
        _poolManager = GetComponent<PoolManager>();
    }
    
    public T CreateEffect<T>(string effectName) where T : EffectBase
    {
        var effect = _poolManager.GetFromPool<T>(effectName);
        
        if (effect is null)
        {
            Debug.LogError($"{effectName} is not enough");
            return null;
        }
        
        return effect as T;
    }
    
    public void ReturnEffectToPool(EffectBase effect, string effectName = default)
    {
        if (effectName == default)
        {
            _poolManager.ReturnToPool(effect);
        }
        else
        {
            _poolManager.ReturnToPool(effectName, effect);
        }
    }
}