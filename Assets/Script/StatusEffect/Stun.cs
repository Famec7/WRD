using System.Collections;
using UnityEngine;

public class Stun : StatusEffect
{
    private Coroutine _stunCoroutine;
    
    public Stun(GameObject target, float duration = 0) : base(target, duration)
    {
    }

    public override void ApplyEffect()
    {
        _stunCoroutine = CoroutineHandler.Instance.StartCoroutine(StunCoroutine());
        
#if STATUS_EFFECT_LOG
        Debug.Log("${Stun Effect Applied} - Duration: {duration}");
#endif
    }

    public override void RemoveEffect()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.ResetSpeed();
        }
        
        if (_stunCoroutine != null)
        {
            CoroutineHandler.Instance.StopCoroutine(_stunCoroutine);
        }
        
#if STATUS_EFFECT_LOG
    Debug.Log("${Stun Effect Removed} - Duration: {duration}");
#endif
    }
    
    private IEnumerator StunCoroutine()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.moveSpeed = 0.0f;
            
            if (Mathf.Abs(duration) > 0.01f)
            {
                yield return waitTime;
                
                RemoveEffect();
            }
            else
            {
                yield return null;
            }
        }
    }
}