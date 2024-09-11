using System.Collections;
using UnityEngine;

public class SwingAnimation : AnimationBase
{
    [Space] [Header("휘두르는 각도")]
    [SerializeField]
    private float _degree = 60.0f;
    
    public override void PlayAnimation()
    {
        Owner.localRotation = Quaternion.Euler(0, 0, 0);
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
        
        if(startRotation.eulerAngles.y >= 180.0f || startRotation.eulerAngles.y <= -180.0f)
            _degree *= -Mathf.Abs(_degree);
        
        var targetRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y, _degree);
        
        float hitdownTime = endTime / 4.0f;
        
        while (elapsedTime < hitdownTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            Owner.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            Owner.localRotation = Quaternion.Lerp(targetRotation, startRotation, t);
            yield return null;
        }
        
        Owner.localRotation = startRotation;
    }

    private float CalculateElapsedTime(ref float elaspseTime)
    {
        elaspseTime += Time.deltaTime;
        float t = elaspseTime / endTime;
        
        return animationSpeed.Evaluate(t);
    }
}