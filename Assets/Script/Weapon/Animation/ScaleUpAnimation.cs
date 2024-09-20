using System.Collections;
using UnityEngine;

public class ScaleUpAnimation : AnimationBase
{
    [SerializeField]
    private float scaleFactor = 1.05f;
    
    public override void PlayAnimation()
    {
        StartCoroutine(IE_ScaleUp());
    }

    public override void StopAnimation()
    {
        StopCoroutine(IE_ScaleUp());
    }
    
    private IEnumerator IE_ScaleUp()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;
        
        float elapsedTime = 0f;
        float breakTime = endTime / 4.0f;

        while (elapsedTime < breakTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
}