using System;
using System.Collections;
using UnityEngine;

public class SlowDown : StatusEffect
{
    private Coroutine _slowDownCoroutine;
    private float _slowDownRate;
    private float _originalSpeed;

    public SlowDown(GameObject target, float slowDownRate, float duration = 0f) : base(target, duration)
    {
        _slowDownRate = slowDownRate;
    }

    public override void ApplyEffect()
    {
        _slowDownCoroutine = CoroutineHandler.Instance.StartCoroutine(SlowDownCoroutine());
#if DEBUG
        Debug.Log("SlowDown Effect Applied");
#endif
    }

    public override void RemoveEffect()
    {
        if (_slowDownCoroutine != null)
        {
            if(target.TryGetComponent(out Status status))
            {
                status.moveSpeed = _originalSpeed;
            }
            CoroutineHandler.Instance.StopCoroutine(_slowDownCoroutine);
        }
        
#if DEBUG
        Debug.Log("SlowDown Effect Removed");
#endif
    }

    private IEnumerator SlowDownCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            _originalSpeed = status.moveSpeed;
            status.moveSpeed *= _slowDownRate / 100;

            if(Math.Abs(duration - 0f) > 0.01f)
            {
                yield return new WaitForSeconds(duration);
                
                RemoveEffect();
            }
            else
            {
                yield return null;
            }
        }
    }
}