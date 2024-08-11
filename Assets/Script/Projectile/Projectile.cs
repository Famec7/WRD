using UnityEngine;

public class Projectile : ProjectileBase
{
    public virtual Vector3 Target { get; set; }
    
    protected override void MoveToTarget()
    {
        transform.position  = Vector3.MoveTowards(transform.position, Target, Speed * Time.deltaTime);
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