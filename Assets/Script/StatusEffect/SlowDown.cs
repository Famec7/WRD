using System;
using System.Collections;
using UnityEngine;

public class SlowDown : StatusEffect
{
    private Coroutine _slowDownCoroutine;
    private float _slowDownRate;

    public float SlowDownRate
    {
        get => _slowDownRate;
        private set => _slowDownRate = value;
    }

    public SlowDown(GameObject target, float slowDownRate, float duration = 0f) : base(target, duration)
    {
        SlowDownRate = slowDownRate;
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
        if(target.TryGetComponent(out Status status))
        {
            status.moveSpeedMultiplier -= (1 - _slowDownRate / 100.0f);
        }
        
        if (_slowDownCoroutine != null)
        {
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
            status.moveSpeedMultiplier += (1 - _slowDownRate / 100.0f);

            if(Math.Abs(duration) > 0.01f)
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