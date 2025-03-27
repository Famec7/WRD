using System.Collections;
using UnityEngine;

public class SwingAnimation : AnimationBase
{
    [Space] [Header("휘두르는 각도")]
    [SerializeField]
    private float _degree = 60.0f;
    
    private Coroutine _coroutine;
    
    public override void PlayAnimation()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(IE_Swing());
    }

    public override void StopAnimation()
    {
        if (_coroutine == null)
        {
            return;
        }
        
        StopCoroutine(_coroutine);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator IE_Swing()
    {
        float elapsedTime = 0.0f;
        Quaternion originalRotation = this.transform.localRotation;
        
        if (Mathf.Approximately(originalRotation.eulerAngles.y, 180.0f))
        {
            _degree = -Mathf.Abs(_degree);
        }

        Quaternion targetRotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, _degree);
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            if (float.IsNaN(t))
            {
                Debug.LogError("Invalid elapsed time calculation");
                yield break;
            }
            
            this.transform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, t);
            if (IsInvalidQuaternion(this.transform.localRotation))
            {
                Debug.LogError("Invalid rotation detected");
                yield break;
            }
            
            yield return null;
        }
        
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        OnEnd?.Invoke();
    }
    
    private bool IsInvalidQuaternion(Quaternion ownerLocalRotation)
    {
        return float.IsNaN(ownerLocalRotation.x) || float.IsNaN(ownerLocalRotation.y) || float.IsNaN(ownerLocalRotation.z) || float.IsNaN(ownerLocalRotation.w);
    }
}
