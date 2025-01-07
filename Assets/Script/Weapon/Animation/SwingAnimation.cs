using System.Collections;
using UnityEngine;

public class SwingAnimation : AnimationBase
{
    [Space] [Header("휘두르는 각도")]
    [SerializeField]
    private float _degree = 60.0f;
    
    Quaternion _originalRotation;
    
    public override void PlayAnimation()
    {
        Owner.localRotation = Quaternion.Euler(Owner.localRotation.eulerAngles.x, Owner.localRotation.eulerAngles.y, 0.0f);
        StartCoroutine(IE_Swing());
    }

    public override void StopAnimation()
    {
        StopCoroutine(IE_Swing());
    }

    private IEnumerator IE_Swing()
    {
        float elapsedTime = 0.0f;
        _originalRotation = Owner.localRotation;
        
        if (Mathf.Approximately(_originalRotation.eulerAngles.y, 180.0f))
        {
            _degree = -Mathf.Abs(_degree);
        }

        Quaternion targetRotation = Quaternion.Euler(_originalRotation.eulerAngles.x, _originalRotation.eulerAngles.y, _degree);
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            if (float.IsNaN(t))
            {
                Debug.LogError("Invalid elapsed time calculation");
                yield break;
            }
            
            Owner.localRotation = Quaternion.Lerp(_originalRotation, targetRotation, t);
            if (IsInvalidQuaternion(Owner.localRotation))
            {
                Debug.LogError("Invalid rotation detected");
                yield break;
            }
            
            yield return null;
        }
        
        Owner.localRotation = _originalRotation;
    }
    
    private bool IsInvalidQuaternion(Quaternion ownerLocalRotation)
    {
        return float.IsNaN(ownerLocalRotation.x) || float.IsNaN(ownerLocalRotation.y) || float.IsNaN(ownerLocalRotation.z) || float.IsNaN(ownerLocalRotation.w);
    }
}
