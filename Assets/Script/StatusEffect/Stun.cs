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
        _stunCoroutine = target.GetComponent<MonoBehaviour>().StartCoroutine(StunCoroutine());
        
#if STATUS_EFFECT_LOG
        Debug.Log("${Stun Effect Applied} - Duration: {duration}");
#endif
    }

    public override void RemoveEffect()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.stunStack--;
            
            if (status.stunStack <= 0)
            {
                status.stunStack = 0;
                status.ResetSpeed();
            }
        }
        
        if (_stunCoroutine != null)
        {
            target.GetComponent<MonoBehaviour>().StopCoroutine(_stunCoroutine);
        }
        
#if STATUS_EFFECT_LOG
    Debug.Log("${Stun Effect Removed} - Duration: {duration}");
#endif
        
        StatusEffectManager.Instance.RemoveValue(status, this);
    }
    
    private IEnumerator StunCoroutine()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.stunStack++;
            status.MoveSpeed = 0.0f;
            
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