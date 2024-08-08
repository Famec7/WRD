using UnityEngine;

public class GuidedProjectile : ProjectileBase
{
    public GameObject Target { get; set; }
    
    protected override void MoveToTarget()
    {
        if (Target is null || !Target.activeSelf)
        {
            return;
        }

        var direction = Target.transform.position - transform.position;
        direction.Normalize();
        transform.position += direction.normalized * (Speed * Time.deltaTime);
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
            ProjectileManager.Instance.ReturnProjectileToPool(this);
        }
    }
}