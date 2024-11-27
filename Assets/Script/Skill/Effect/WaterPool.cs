using System;
using System.Collections;
using UnityEngine;

public class WaterPool : MonoBehaviour
{
    private float _duration = 0.0f;
    private float _slowRate = 0.0f;
    private float _damageAmplification = 0.0f;
    
    private Coroutine _effectCoroutine;
    
    public void Init(float duration, float slowRate, float damageAmplification)
    {
        _duration = duration;
        _slowRate = slowRate;
        _damageAmplification = damageAmplification;
    }

    public void PlayEffect()
    {
        this.gameObject.SetActive(true);
        _effectCoroutine = StartCoroutine(IE_PlayEffect());
    }

    public void StopEffect()
    {
        this.gameObject.SetActive(false);
        
        if (_effectCoroutine != null)
            StopCoroutine(_effectCoroutine);
    }
    
    private IEnumerator IE_PlayEffect()
    {
        yield return new WaitForSeconds(_duration);
        StopEffect();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffect slow = new SlowDown(status.gameObject, _slowRate, _duration);
            StatusEffectManager.Instance.AddStatusEffect(status, slow);
            
            StatusEffect amplifyDamage = new DamageAmplification(status.gameObject, _damageAmplification / 100.0f, _duration);
            StatusEffectManager.Instance.AddStatusEffect(status, amplifyDamage);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(SlowDown));
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(DamageAmplification));
        }
    }
}