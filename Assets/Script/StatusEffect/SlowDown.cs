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
#if STATUS_EFFECT_LOG
        Debug.Log("${SlowDown Effect Applied} - SlowDown Rate: {_slowDownRate}");
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
        
#if STATUS_EFFECT_LOG
        Debug.Log("${SlowDown Effect Removed} - SlowDown Rate: {_slowDownRate}");
#endif
    }

    private IEnumerator SlowDownCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            _originalSpeed = status.moveSpeed;
            status.moveSpeed *= 1 - _slowDownRate / 100;

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