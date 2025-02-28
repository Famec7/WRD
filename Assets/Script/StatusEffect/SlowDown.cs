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
        _slowDownCoroutine = target.GetComponent<MonoBehaviour>().StartCoroutine(SlowDownCoroutine());
        
#if STATUS_EFFECT_LOG
        Debug.Log("${SlowDown Effect Applied} - SlowDown Rate: {_slowDownRate}");
#endif
    }

    public override void RemoveEffect()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.moveSpeedMultiplier += _slowDownRate / 100.0f;
        }
        
        if (_slowDownCoroutine != null)
        {
            target.GetComponent<MonoBehaviour>().StopCoroutine(_slowDownCoroutine);
        }
        
#if STATUS_EFFECT_LOG
        Debug.Log("${SlowDown Effect Removed} - SlowDown Rate: {_slowDownRate}");
#endif
    }

    private IEnumerator SlowDownCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            float newSlowDownRate = Mathf.Round((status.moveSpeedMultiplier - _slowDownRate / 100.0f) * 100.0f) / 100.0f;
            status.moveSpeedMultiplier = newSlowDownRate;

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