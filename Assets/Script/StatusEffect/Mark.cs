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
        _markCoroutine = target.GetComponent<MonoBehaviour>().StartCoroutine(MarkCoroutine());

        //표식 이펙트 켜주기
        monsterEffecter.SetMarkEffect(true);
    }

    public override void RemoveEffect()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.MarkStack--;
        }
        
        if (_markCoroutine != null)
        {
            target.GetComponent<MonoBehaviour>().StopCoroutine(_markCoroutine);
        }

        //표식 이펙트 꺼주기
        if (status.MarkStack <= 0)
        {
            monsterEffecter.SetMarkEffect(false);
        }
        
        StatusEffectManager.Instance.RemoveValue(status, this);
    }
    
    private IEnumerator MarkCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.MarkStack++;
            
            if(Math.Abs(duration - 0f) > 0.01f)
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