using System.Collections;
using UnityEngine;

public class SwingAnimation : AnimationBase
{
    [SerializeField]
    private float _degree = 60.0f;
    
    public override void PlayAnimation()
    {
        StartCoroutine(IE_Swing());
    }

    public override void StopAnimation()
    {
        StopCoroutine(IE_Swing());
    }

    private IEnumerator IE_Swing()
    {
        var elapsedTime = 0.0f;
        var startRotation = Owner.localRotation;
        var targetRotation = Quaternion.Euler(0.0f, 0.0f, _degree);
        
        while (elapsedTime < endTime / 2.0f)
        {
            elapsedTime += Time.deltaTime * animationSpeed.Evaluate(elapsedTime);
            elapsedTime = Mathf.Clamp01(elapsedTime);
            
            Owner.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime);
            yield return null;
        }
        
        elapsedTime = 0.0f;
        
        while (elapsedTime < endTime / 2.0f)
        {
            elapsedTime += Time.deltaTime * animationSpeed.Evaluate(elapsedTime);
            elapsedTime = Mathf.Clamp01(elapsedTime);
            
            Owner.localRotation = Quaternion.Lerp(targetRotation, startRotation, elapsedTime);
            yield return null;
        }
        
        Owner.localRotation = startRotation;
    }
}