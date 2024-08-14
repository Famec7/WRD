using UnityEngine;

public class Projectile : ProjectileBase
{
    public virtual Vector3 Target { get; set; }
    
    protected override void MoveToTarget()
    {
        transform.position = Vector3.Lerp(this.transform.position, Target, curve.Evaluate(Time.time));
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