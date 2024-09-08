using System.Collections;
using UnityEngine;

public class ThrustAnimation : AnimationBase
{
    [SerializeField]
    private Vector3 _endPosition;
    
    public override void PlayAnimation()
    {
        StartCoroutine(IE_Thrust());
    }

    public override void StopAnimation()
    {
        StopCoroutine(IE_Thrust());
    }
    
    private IEnumerator IE_Thrust()
    {
        var elapsedTime = 0.0f;
        var startPosition = Owner.localPosition;
        
        float breakTime = endTime / 4.0f;
        
        Debug.Log(transform.right);
        
        while (elapsedTime < breakTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            float newX = Mathf.Lerp(startPosition.x, _endPosition.x, t);
            Owner.localPosition = new Vector3(newX, startPosition.y, startPosition.z);
            yield return null;
        }
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            float newX = Mathf.Lerp(_endPosition.x, startPosition.x, t);
            Owner.localPosition = new Vector3(newX, startPosition.y, startPosition.z);
            yield return null;
        }
        
        Owner.localPosition = startPosition;
    }
    
    private float CalculateElapsedTime(ref float elaspseTime)
    {
        elaspseTime += Time.deltaTime;
        float t = elaspseTime / endTime;
        
        return animationSpeed.Evaluate(t);
    }
}