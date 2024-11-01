using System;
using System.Text;
using UnityEngine;

public class GuidedProjectile : ProjectileBase
{
    private GameObject _target;

    public GameObject Target
    {
        get => _target;
        set
        {
            _target = value;
            LookAtTarget(value.transform.position);
        }
    }

    public Action OnHit { get; set; }

    private float _startTime;
    private Vector3 _startPosition;
    private float _journeyLength;

    private string _hitEffectName;

    private void Start()
    {
        _startPosition = transform.position;
        _journeyLength = Vector3.Distance(_startPosition, Target.transform.position);
        _startTime = Time.time;
    }

    protected override void MoveToTarget()
    {
        if (Target is null || !Target.activeSelf)
        {
            ProjectileManager.Instance.ReturnProjectileToPool(this);
            return;
        }

        float distance = Vector3.Distance(transform.position, Target.transform.position);
        if (distance < 0.1f)
        {
            OnHit?.Invoke();
            
            ProjectileManager.Instance.ReturnProjectileToPool(this);
            
            ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>(_hitEffectName);
            particleEffect.SetPosition(Target.transform.position);
        }


        float timeSinceStart = Time.time - _startTime;
        float fractionOfJourney = timeSinceStart / _journeyLength;
        float easedFraction = curve.Evaluate(fractionOfJourney);

        transform.position = Vector3.Lerp(this.transform.position, Target.transform.position, easedFraction);
    }

    public override void GetFromPool()
    {
        ;
    }

    public override void ReturnToPool()
    {
        OnHit = null;
    }

    protected void LookAtTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = rotation;
    }

    public void SetType(RangedWeapon.Type type)
    {
        StringBuilder sb = new StringBuilder(type + "Hit");
        _hitEffectName = sb.ToString();
    }
}