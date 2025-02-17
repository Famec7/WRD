using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class DebuffZone : EffectBase
{
    private Coroutine _coroutine;
    private float _effectTime;

    protected override void Init()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    protected void SetData(float effectTime, float radius)
    {
        this.transform.SetParent(null);
        _effectTime = effectTime;
        SetScale(new Vector3(radius, radius, 1));
    }

    public override void PlayEffect()
    {
        transform.gameObject.SetActive(true);

        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(IE_PlayEffect());
        }
    }

    public override void StopEffect()
    {
        transform.gameObject.SetActive(false);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator IE_PlayEffect()
    {
        if (_effectTime > 0)
        {
            yield return new WaitForSeconds(_effectTime);
        }
        else
        {
            yield break;
        }

        StopEffect();
    }

    private readonly Dictionary<Status, StatusEffect> _statusEffects = new Dictionary<Status, StatusEffect>();

    /// <summary>
    /// 적용할 상태이상을 반환
    /// </summary>
    /// <param name="status"> 적용 대상 </param>
    /// <returns> 적용할 상태이상 </returns>
    protected abstract StatusEffect ApplyStatusEffect(Status status);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffect statusEffect = ApplyStatusEffect(status);
            if (statusEffect is null)
            {
                return;
            }

            StatusEffectManager.Instance.AddStatusEffect(status, statusEffect);

            _statusEffects.Add(status, statusEffect);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            if (_statusEffects.TryGetValue(status, out StatusEffect effect) == false)
            {
                return;
            }

            StatusEffectManager.Instance.RemoveStatusEffect(status, effect);
            _statusEffects.Remove(status);
        }
    }
}