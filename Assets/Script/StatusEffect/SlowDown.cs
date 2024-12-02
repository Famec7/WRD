using System;
using System.Collections;
using UnityEngine;

public class SlowDown : StatusEffect
{
    // 이속감소의 최대 감속률
    private static float maxSlowDownRate = 70.0f;
    
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
            status.ResetSpeed();
        }
        
        if (_slowDownCoroutine != null)
        {
            CoroutineHandler.Instance.StopCoroutine(_slowDownCoroutine);
        }
        
#if STATUS_EFFECT_LOG
        Debug.Log("${SlowDown Effect Removed} - SlowDown Rate: {_slowDownRate}");
#endif
    }

    public override void CombineEffect(StatusEffect statusEffect)
    {
        base.CombineEffect(statusEffect);

        if (statusEffect is SlowDown slowDown)
        {
            SlowDownRate = Mathf.Min(SlowDownRate + slowDown.SlowDownRate, maxSlowDownRate);
            
            if (target.TryGetComponent(out Status status))
            {
                status.moveSpeed = status.originalSpeed * (1 - _slowDownRate / 100);
            }
        }
    }

    private IEnumerator SlowDownCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.moveSpeed = status.originalSpeed * (1 - _slowDownRate / 100);

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