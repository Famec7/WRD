using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
    /*************Methods that need to be implemented*************/
    protected abstract void Init();
    public abstract void PlayEffect();
    public abstract void StopEffect();

    #region Unity Methods 

    private void Awake()
    {
        Init();
        StopEffect();
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    #endregion
}