using System;
using UnityEngine;

[Serializable]
public class Pivot
{
    private Transform _transform;
    
    [Header("피봇")]
    [SerializeField]
    private Vector3 _pivot;
    
    [SerializeField]
    private Vector3 _originRotation;
    
    [SerializeField]
    private Vector3 _originScale;
    
    public void Init(Transform transform)
    {
        _transform = transform;
    }

    #region Getter

    public Vector3 GetPivot()
    {
        return _pivot;
    }
    
    public Quaternion GetOriginRotation()
    {
        return Quaternion.Euler(_originRotation);
    }
    
    public Vector3 GetOriginScale()
    {
        return _originScale;
    }

    #endregion

    #region Reset

    public void ResetPivot()
    {
        if(_transform is null)
            return;
        
        ResetPosition();
        ResetScale();
        ResetRotation();
    }
    
    public void ResetPosition()
    {
        _transform.position = _pivot;
    }
    
    public void ResetScale()
    {
        _transform.localScale = _originScale;
    }
    
    public void ResetRotation()
    {
        _transform.rotation = Quaternion.Euler(_originRotation);
    }

    #endregion
    
}