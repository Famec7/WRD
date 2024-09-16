using UnityEngine;

public abstract class EffectBase : MonoBehaviour, IPoolObject
{
    /*************Methods that need to be implemented*************/
    
    /// <summary>
    /// Initialize the effect. Call this method in Awake
    /// </summary>
    protected abstract void Init();
    
    /// <summary>
    /// Play the effect
    /// </summary>
    public abstract void PlayEffect();
    
    /// <summary>
    /// Stop the effect
    /// </summary>
    public abstract void StopEffect();

    #region Unity Methods 

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Set the position of the effect
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    /// <summary>
    /// Set the rotation of the effect
    /// </summary>
    /// <param name="rotation"></param>
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    
    /// <summary>
    /// Set the scale of the effect
    /// </summary>
    /// <param name="scale"></param>
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    #endregion

    public virtual void GetFromPool()
    {
        ;
    }

    public virtual void ReturnToPool()
    {
        ;
    }
}