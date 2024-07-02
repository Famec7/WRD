using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class CharacterController : MonoBehaviour
{
    /***********************Variables*****************************/

    #region Data

    [SerializeField] private CharacterData _data;
    public CharacterData Data => _data;

    #endregion

    #region Target

    public virtual GameObject Target { get; set; }

    #endregion

    #region direction

    private Vector3 _moveDir;

    public Vector3 MoveDir
    {
        get => _moveDir;
        set
        {
            _moveDir = value;
            FlipSprite(_moveDir.x > 0);
        }
    }

    #endregion

    /*************************Sprite******************************/
    private SpriteRenderer _spriteRenderer;

    public void FlipSprite(bool isRight)
    {
        _spriteRenderer.flipX = !isRight;
    }

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /*************************Find Target******************************/
    public void FindNearestTarget()
    {
        var colliders = RangeDetectionUtility.GetAttackTargets(transform.position, Vector2.zero,
            Data.CurrentWeapon.Data.attackRange, LayerMask.GetMask("Monster", "Boss", "Mission"));

        if(colliders is null)
            return;
        
        foreach (var collider in colliders)
        {
            var distanceFromEntityToCollider = Vector3.Distance(transform.position, collider.transform.position);
            var distanceFromEntityToTarget =
                Target == null ? 0.0f : Vector3.Distance(transform.position, Target.transform.position);

            // 가장 가까운 적을 타겟으로 설정
            if (Target == null)
                Target = collider.transform.gameObject;
            else if (distanceFromEntityToCollider < distanceFromEntityToTarget)
                Target = collider.transform.gameObject;
        }
    }
}