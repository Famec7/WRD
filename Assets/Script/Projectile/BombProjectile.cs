using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombProjectile : ProjectileBase
{
    public bool IsAttached = false;
    public float damage;
    protected float _startTime;
    protected float _journeyLength;
    protected Vector3 _startPosition;

    public GameObject Target;
    private void Start()
    {
        _startPosition = transform.position;
        _startTime = Time.time;
        _journeyLength = Vector3.Distance(_startPosition, Target.transform.position);
    }

    protected override void MoveToTarget()
    {   
        if (Target is null || !Target.activeSelf)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if (distance < 0.1f) 
        {
            IsAttached = true;

            if (Target == null)
            {
                ProjectileManager.Instance.ReturnProjectileToPool<BombProjectile>(this, "Bomb");
            }
        }

        float timeSinceStart = Time.time - _startTime;
        float fractionOfJourney = timeSinceStart / _journeyLength;
        float easedFraction = curve.Evaluate(fractionOfJourney);
        if (!IsAttached)
            transform.position = Vector3.Lerp(this.transform.position, Target.transform.position, 0.1f);
        else
            transform.position = Target.transform.position;
    }

    public void Explosion()
    {
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
        effect.SetPosition(transform.position);
        Target.GetComponent<Monster>().HasAttacked(damage);
        IsAttached = false;
        Target = null;
    }

    public override void GetFromPool()
    {
        ;
    }

    public override void ReturnToPool()
    {
        Target = null;
        IsAttached = false;
    }
}
