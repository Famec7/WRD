using System;
using System.Collections;
using UnityEngine;

public class JokerMark : StatusEffect
{
    private Action<Monster> _action;
    private Coroutine _jokerMarkCoroutine;
    
    public JokerMark(GameObject target, Action<Monster> onEndDebuff,float duration = 2f) : base(target, duration)
    {
        _action = onEndDebuff;
    }

    public override void ApplyEffect()
    {
        _jokerMarkCoroutine = target.GetComponent<MonoBehaviour>().StartCoroutine(JokerMarkCoroutine());
        
#if STATUS_EFFECT_LOG
        Debug.Log($"{Joker Mark Effect Applied} - Duration: {duration}");
#endif
    }

    public override void RemoveEffect()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.IsJokerMark = false;
            _action?.Invoke(target.GetComponent<Monster>());
        }
        
        if (_jokerMarkCoroutine != null)
        {
            target.GetComponent<MonoBehaviour>().StopCoroutine(_jokerMarkCoroutine);
        }
        
#if STATUS_EFFECT_LOG
        Debug.Log($"{Joker Mark Effect Removed} - Duration: {duration}");
#endif
    }
    
    private IEnumerator JokerMarkCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.IsJokerMark = true;
            
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