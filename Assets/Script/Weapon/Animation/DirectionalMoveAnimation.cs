using System.Collections;
using UnityEngine;

public class DirectionalMoveAnimation : AnimationBase
{
    [SerializeField]
    private Vector3 _endPosition;
    
    private Coroutine _coroutine;
    
    public override void PlayAnimation()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(IE_Thrust());
    }

    public override void StopAnimation()
    {
        if (_coroutine == null)
        {
            return;
        }
        
        StopCoroutine(_coroutine);
        this.transform.localPosition = Vector3.zero;
    }
    
    private IEnumerator IE_Thrust()
    {
        float elapsedTime = 0.0f;
        Vector3 originalPosition = this.transform.localPosition;
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            float newX = Mathf.Lerp(originalPosition.x, _endPosition.x, t);
            this.transform.localPosition = new Vector3(newX, originalPosition.y, originalPosition.z);
            yield return null;
        }
        
        this.transform.localPosition = Vector3.zero;
        OnEnd?.Invoke();
    }
}