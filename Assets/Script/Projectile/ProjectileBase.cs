using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public abstract class ProjectileBase : MonoBehaviour, IPoolObject
{
    [SerializeField] private float _speed;
    
    public float Speed => _speed;
    public float Damage { get; set; }
    public Vector3 Direction { get; set; }
    public Vector3 TargetPosition { get; set; }
    
    private void Update()
    {
        MoveToTarget();
    }
    
    private void MoveToTarget()
    {
        Direction = TargetPosition - transform.position;
        Direction.Normalize();
        transform.position += Direction * (Speed * Time.deltaTime);
    }

    public abstract void GetFromPool();

    public abstract void ReturnToPool();
}