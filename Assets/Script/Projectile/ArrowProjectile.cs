using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : ProjectileBase
{
    private CircleCollider2D _collider;
    private float _startTime;

    public float JourneyTime = 1.5f; // 목표 지점에 도달할 총 시간 (초)
    public Vector3 TargetPosition;
    public void SetArrow(Vector2 _TargetPosition)
    {
        _startTime = Time.time;
        TargetPosition = _TargetPosition;
    }

    private void Start()
    {
        _startTime = Time.time ;
    }
    protected override void MoveToTarget()
    {
        float timeSinceStart = Time.time - _startTime;
        float fractionOfJourney = timeSinceStart / JourneyTime;

        if (fractionOfJourney >= 1.0f)
        {
            transform.position = TargetPosition;
            ProjectileManager.Instance.ReturnProjectileToPool(this);
            return;
        }

        float easedFraction = curve.Evaluate(fractionOfJourney);
        transform.position = Vector3.Lerp(transform.position, TargetPosition, easedFraction);
    }

    public override void GetFromPool()
    {
        ;
    }

    public override void ReturnToPool()
    {

        TargetPosition = Vector2.zero;
    }
}
