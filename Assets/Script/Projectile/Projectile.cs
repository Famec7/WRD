using System;
using UnityEngine;

public class Projectile : ProjectileBase
{
    public virtual Vector3 Target { get; set; }

    private float _startTime;
    private Vector3 _startPosition;
    private float _journeyLength;

    private void Start()
    {
        _startPosition = transform.position;
        _journeyLength = Vector3.Distance(_startPosition, Target);
        _startTime = Time.time;
    }

    protected override void MoveToTarget()
    {
        float timeSinceStart = Time.time - _startTime;
        float fractionOfJourney = timeSinceStart / _journeyLength;
        float easedFraction = curve.Evaluate(fractionOfJourney);
        
        transform.position = Vector3.Lerp(_startPosition, Target, easedFraction);
    }

    public override void GetFromPool()
    {
        ;
    }

    public override void ReturnToPool()
    {
        ;
    }
}