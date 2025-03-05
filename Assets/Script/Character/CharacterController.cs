using System;
using System.Linq;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    /***********************Variables*****************************/

    #region Data

    [SerializeField] private CharacterData _data = new CharacterData();
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
            SetFlip(_moveDir.x > 0);
        }
    }

    #endregion

    /*************************Sprite******************************/
    private SpriteRenderer _spriteRenderer;

    public void SetFlip(bool isRight)
    {
        this.transform.rotation = Quaternion.Euler(0, isRight ? 180 : 0, 0);
    }

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /*************************Find Target******************************/
    public GameObject FindNearestTarget()
    {
        LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
        float attackRange = Data.CurrentWeapon.Data.AttackRange;
        
        var colliders = RangeDetectionUtility.GetAttackTargets(transform.position, attackRange, default, layerMask);

        if (colliders is null)
            return null;
        
        GameObject nearestTarget = null;
        float minDistance = attackRange;

        foreach (var col in colliders)
        {
            if(col.gameObject.activeSelf is false)
                continue;
            if (col.gameObject.GetComponent<Monster>().status.unitCode >= UnitCode.BOSS1 && col.gameObject.GetComponent<Monster>().status.unitCode <= UnitCode.BOSS6)
            {
                nearestTarget = col.gameObject;
                break;
            }
            float distance = Vector3.Distance(transform.position, col.transform.position);
            
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = col.gameObject;
            }
            
        }
        
        return nearestTarget;
    }

    public bool IsTargetNullOrInactive()
    {
        return Target == null || !Target.activeSelf;
    }

    /*************************Weapon******************************/
    public abstract void AttachWeapon(WeaponBase weapon);
    public abstract void DetachWeapon();
}