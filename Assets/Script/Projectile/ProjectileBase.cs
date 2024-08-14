using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public abstract class ProjectileBase : MonoBehaviour, IPoolObject
{
    [SerializeField]
    protected AnimationCurve curve;

    protected float Threshold { get; } = 0.1f;

    public float Damage { get; set; }
    public Vector3 Direction { get; set; }
    
    protected virtual void Update()
    {
        MoveToTarget();
    }

    /// <summary>
    /// 타겟 이동하는 방식 투사체마다 다르게 처리
    /// Update에서 호출 중
    /// </summary>
    protected abstract void MoveToTarget();
    
    public abstract void GetFromPool();

    public abstract void ReturnToPool();
}