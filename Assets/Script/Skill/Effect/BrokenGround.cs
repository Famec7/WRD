using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BrokenGround : EffectBase
{
    private float _brokenGroundTime;
    private float _damageAmplification;

    private CircleCollider2D _collider;
    private Coroutine _coroutine;

    public void SetData(float brokenGroundTime, float damageAmplification, float radius)
    {
        _brokenGroundTime = brokenGroundTime;
        _damageAmplification = damageAmplification;
        
        SetScale(new Vector3(radius, radius, 1));
    }

    protected override void Init()
    {
        _collider = GetComponent<CircleCollider2D>();
        
        if (_collider is null)
        {
            Debug.LogError("CircleCollider2D is null");
            return;
        }
    }

    public override void PlayEffect()
    {
        transform.gameObject.SetActive(true);
        _coroutine = StartCoroutine(IE_PlayEffect());
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
        yield return new WaitForSeconds(_brokenGroundTime);

        StopEffect();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            StatusEffect damageAmplification = new DamageAmplification(monster.gameObject, _damageAmplification);
            StatusEffectManager.Instance.AddStatusEffect(monster.status, damageAmplification);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(DamageAmplification));
        }
    }
}