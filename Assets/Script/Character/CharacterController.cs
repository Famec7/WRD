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
        LayerMask layerMask = LayerMaskManager.Instance.MonsterLayerMask;
        float attackRange = Data.CurrentWeapon.Data.AttackRange;
        
        var colliders = RangeDetectionUtility.GetAttackTargets(transform.position, attackRange, default, layerMask);

        if (colliders is null)
            return;
        
        GameObject nearestTarget = null;
        float minDistance = Data.CurrentWeapon.Data.AttackRange;

        foreach (var col in colliders)
        {
            if(col.gameObject.activeSelf is false)
                continue;
            
            float distance = Vector3.Distance(transform.position, col.transform.position);
            
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = col.gameObject;
            }
            
        }
        
        Target = nearestTarget;
    }

    public bool IsTargetNullOrInactive()
    {
        return Target == null || !Target.activeSelf;
    }

    /*************************Weapon******************************/
    public abstract void AttachWeapon(WeaponBase weapon);
    public abstract void DetachWeapon();
}