using System.Collections;
using UnityEngine;

public class PreventWoundConsumption : StatusEffect
{
    private Coroutine _preventWoundConsumptionCoroutine;
    
    public PreventWoundConsumption(GameObject target, float duration = 0) : base(target, duration)
    {
    }

    public override void ApplyEffect()
    {
        _preventWoundConsumptionCoroutine = target.GetComponent<MonoBehaviour>().StartCoroutine(PreventWoundConsumptionCoroutine());
    }

    public override void RemoveEffect()
    {
        if (_preventWoundConsumptionCoroutine != null)
        {
            if(target.TryGetComponent(out Status status))
            {
                status.PreventWoundConsumption = false;
                StatusEffectManager.Instance.RemoveValue(status, this);
            }
            target.GetComponent<MonoBehaviour>().StopCoroutine(_preventWoundConsumptionCoroutine);
        }
    }
    
    private IEnumerator PreventWoundConsumptionCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.PreventWoundConsumption = true;
            
            if(Mathf.Abs(duration - 0f) > 0.01f)
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