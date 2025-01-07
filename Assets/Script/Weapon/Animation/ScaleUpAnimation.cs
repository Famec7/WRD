using System.Collections;
using UnityEngine;

public class ScaleUpAnimation : AnimationBase
{
    [SerializeField]
    private float scaleFactor = 1.05f;
    
    private Vector3 _originalScale;
    
    public override void PlayAnimation()
    {
        StartCoroutine(IE_ScaleUp());
    }

    public override void StopAnimation()
    {
        StopCoroutine(IE_ScaleUp());
        transform.localScale = _originalScale;
    }
    
    private IEnumerator IE_ScaleUp()
    {
        _originalScale = transform.localScale;
        Vector3 targetScale = _originalScale * scaleFactor;
        
        float elapsedTime = 0f;
        float breakTime = endTime / 4.0f;

        while (elapsedTime < breakTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            transform.localScale = Vector3.Lerp(_originalScale, targetScale, t);
            yield return null;
        }
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            transform.localScale = Vector3.Lerp(targetScale, _originalScale, t);
            yield return null;
        }
        
        transform.localScale = _originalScale;
    }
}