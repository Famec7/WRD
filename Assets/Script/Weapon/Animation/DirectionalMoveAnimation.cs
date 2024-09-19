using System.Collections;
using UnityEngine;

public class DirectionalMoveAnimation : AnimationBase
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
        float elapsedTime = 0.0f;
        Vector3 startPosition = Owner.localPosition;
        
        Vector3 adjustedEndPosition = _endPosition / Owner.parent.localScale.x;
        
        float breakTime = endTime / 4.0f;
        
        while (elapsedTime < breakTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            float newX = Mathf.Lerp(startPosition.x, adjustedEndPosition.x, t);
            Owner.localPosition = new Vector3(newX, startPosition.y, startPosition.z);
            yield return null;
        }
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            float newX = Mathf.Lerp(adjustedEndPosition.x, startPosition.x, t);
            Owner.localPosition = new Vector3(newX, startPosition.y, startPosition.z);
            yield return null;
        }
        
        Owner.localPosition = startPosition;
    }
}