using System;
using System.Collections;
using UnityEngine;

public class Mark : StatusEffect
{
    private Coroutine _markCoroutine;
    
    public Mark(GameObject target, float duration = 0) : base(target, duration)
    {
    }

    public override void ApplyEffect()
    {
        _markCoroutine = CoroutineHandler.Instance.StartCoroutine(MarkCoroutine());
    }

    public override void RemoveEffect()
    {
        if (_markCoroutine != null)
        {
            if(target.TryGetComponent(out Status status))
            {
                status.IsMark = false;
            }
            CoroutineHandler.Instance.StopCoroutine(_markCoroutine);
        }
    }
    
    private IEnumerator MarkCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.IsMark = true;
            
            if(Math.Abs(duration - 0f) > 0.01f)
            {
                yield return waitTime;

                status.IsMark = false;
            }
            else
            {
                yield return null;
            }
        }
    }
}