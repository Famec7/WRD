using UnityEngine;

public class Projectile : ProjectileBase
{
    public Vector3 Target { get; set; }
    
    protected override void MoveToTarget()
    {
        var direction = Target - transform.position;
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
}