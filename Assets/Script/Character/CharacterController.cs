using System;
using System.Linq;
using UnityEngine;

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
        this.transform.rotation = Quaternion.Euler(0, isRight ? 180 : 0, 0);
    }

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /*************************Find Target******************************/
    public void FindNearestTarget()
    {
        var colliders = RangeDetectionUtility.GetAttackTargets(transform.position, Vector2.zero,
            Data.CurrentWeapon.Data.AttackRange, default, LayerMask.GetMask("Monster", "Boss", "Mission"));

        if(colliders is null)
            return;
        
        foreach (var col in colliders)
        {
            var distanceFromEntityToCollider = Vector3.Distance(transform.position, col.transform.position);
            var distanceFromEntityToTarget = Target is null ? 0.0f : Vector3.Distance(transform.position, Target.transform.position);

            // 가장 가까운 적을 타겟으로 설정
            if (Target is null)
                Target = col.transform.gameObject;
            else if (distanceFromEntityToCollider < distanceFromEntityToTarget)
                Target = col.transform.gameObject;
        }
    }
    
    /*************************Weapon******************************/
    public abstract void AttachWeapon(WeaponBase weapon);
    public abstract void DetachWeapon();
}