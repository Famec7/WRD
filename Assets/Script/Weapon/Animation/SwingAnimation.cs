using System.Collections;
using UnityEngine;

public class SwingAnimation : AnimationBase
{
    [Space] [Header("휘두르는 각도")]
    [SerializeField]
    private float _degree = 60.0f;
    
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
        Quaternion startRotation = Owner.localRotation;
        
        if (startRotation.eulerAngles.y == 180.0f)
        {
            Debug.Log("180도 이상");
            _degree = -Mathf.Abs(_degree);
        }

        Quaternion targetRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y, _degree);
        
        float hitdownTime = endTime / 4.0f;
        
        while (elapsedTime < hitdownTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            if (float.IsNaN(t))
            {
                Debug.LogError("Invalid elapsed time calculation");
                yield break;
            }
            
            Owner.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            if (IsInvalidQuaternion(Owner.localRotation))
            {
                Debug.LogError("Invalid rotation detected");
                yield break;
            }
            
            yield return null;
        }
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            if (float.IsNaN(t))
            {
                Debug.LogError("Invalid elapsed time calculation");
                yield break;
            }
            
            Owner.localRotation = Quaternion.Lerp(targetRotation, startRotation, t);
            if (IsInvalidQuaternion(Owner.localRotation))
            {
                Debug.LogError("Invalid rotation detected");
                yield break;
            }
            
            yield return null;
        }
        
        Owner.localRotation = startRotation;
    }
    
    private float CalculateElapsedTime(ref float elapsedTime)
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / endTime;
        
        return animationSpeed.Evaluate(t);
    }
    
    private bool IsInvalidQuaternion(Quaternion ownerLocalRotation)
    {
        return float.IsNaN(ownerLocalRotation.x) || float.IsNaN(ownerLocalRotation.y) || float.IsNaN(ownerLocalRotation.z) || float.IsNaN(ownerLocalRotation.w);
    }
}
