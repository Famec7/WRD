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
        ;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Damage);
            
            ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>(_hitEffectName);
            particleEffect.SetPosition(transform.position);
            
            ProjectileManager.Instance.ReturnProjectileToPool(this);
        }
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
        StringBuilder sb = new StringBuilder();
        
        switch (type)
        {
            case RangedWeapon.Type.Bow:
                sb.Append("NormalHit");
                break;
            case RangedWeapon.Type.Gun:
                sb.Append("BulletHit");
                break;
            case RangedWeapon.Type.Orb:
                sb.Append("OrbHit");
                break;
            case RangedWeapon.Type.HighOrb:
                sb.Append("HighOrbHit");
                break;
            default:
                break;
        }
        
        _hitEffectName = sb.ToString();
    }
}