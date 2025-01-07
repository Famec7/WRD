using System.Collections;
using UnityEngine;

public class DirectionalMoveAnimation : AnimationBase
{
    [SerializeField]
    private Vector3 _endPosition;
    
    private Vector3 _originalPosition;
    
    public override void PlayAnimation()
    {
        StartCoroutine(IE_Thrust());
    }

    public override void StopAnimation()
    {
        Owner.localPosition = _originalPosition;
        StopCoroutine(IE_Thrust());
    }
    
    private IEnumerator IE_Thrust()
    {
        float elapsedTime = 0.0f;
        _originalPosition = Owner.localPosition;
        
        Vector3 adjustedEndPosition = _endPosition;
        
        while (elapsedTime < endTime)
        {
            float t = CalculateElapsedTime(ref elapsedTime);
            
            float newX = Mathf.Lerp(_originalPosition.x, adjustedEndPosition.x, t);
            Owner.localPosition = new Vector3(newX, _originalPosition.y, _originalPosition.z);
            yield return null;
        }
        
        Owner.localPosition = _originalPosition;
    }
}