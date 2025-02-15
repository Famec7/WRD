using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrowRain : ClickTypeSkill
{
    private float _damage;
    private float _damageAmplification;
    private float _duration;

    [SerializeField]
    private float _arrowJourneyTime = 1.0f;

    [SerializeField]
    private Vector2 _offset;

    [SerializeField]
    private int _arrowCount;

    protected override void Init()
    {
        base.Init();
        _damage = Data.GetValue(0);
        _duration = Data.GetValue(1);
        _damageAmplification = Data.GetValue(2);
    }

    public override void OnActiveEnter()
    {
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("ArrowRain");
        effect.SetPosition(ClickPosition);
        effect.PlayEffect();
        StartCoroutine(Damage(ClickPosition));
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }

    private Vector2 RandomPointInCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;

        return new Vector2(x, y);
    }

    IEnumerator Damage(Vector3 position)
    {
        yield return new WaitForSeconds(_arrowJourneyTime);
        var targets = RangeDetectionUtility.GetAttackTargets(position, Data.Range / 2f, default, targetLayer);

        if (targets.Count == 0)
            yield break;

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(_damage);
                var mark = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

                if (mark != null)
                {
                    StatusEffect amplification = new DamageAmplification(monster.gameObject, _damageAmplification, _duration);
                    StatusEffectManager.Instance.AddStatusEffect(monster.status, amplification);
                }
            }
        }
    }

}
