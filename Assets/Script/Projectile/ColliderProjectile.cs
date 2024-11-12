using System;
using System.Text;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ColliderProjectile : ProjectileBase
{
    public float _damage;
    private float _startTime;
    protected string _hitEffectName;

    [SerializeField]
    private float _speed;

    public Action OnHit { get; set; }

    private void Start()
    {
        _damage = WeaponDataManager.Instance.Database.GetWeaponDataByNum(409).AttackDamage;
        _startTime = Time.time;

    }

    protected override void MoveToTarget()
    {
        transform.position += Direction * _speed * Time.deltaTime;

        if (!IsVisibleInCamera())
            ProjectileManager.Instance.ReturnProjectileToPool(this);
    }


    public override void GetFromPool()
    {
        ;
    }

    public override void ReturnToPool()
    {
        ;
    }

    private bool IsVisibleInCamera()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            OnHit?.Invoke();
            monster.HasAttacked(_damage);
            ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>(_hitEffectName);
            particleEffect.SetPosition(monster.transform.position);
        }
    }

    public void Init(float damage ,float moveAngle)
    {
        float radianAngle = moveAngle * Mathf.Deg2Rad;
        Direction = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0f);

        _damage = damage;

    }

    public void SetType(RangedWeapon.Type type)
    {
        StringBuilder sb = new StringBuilder(type + "Hit");
        _hitEffectName = sb.ToString();
    }
}
