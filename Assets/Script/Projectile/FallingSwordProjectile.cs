using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class FallingSwordProjectile : Projectile
{
    [SerializeField] private string _projectileName;
    
    #region Data
    
    protected float Range { get; set; }
    protected WaitForSeconds Dealy { get; set; }

    public virtual void SetData(SkillData data)
    {
        Range = data.Range;
        _collider.radius = data.Range;
    }

    #endregion

    /********************************Event Function********************************/
    
    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, Target) < Threshold && _collider.enabled is false)
        {
            _collider.enabled = true;
            
            OnSwordImpact();
            StartCoroutine(IE_Destroy());
        }
    }
    
    // 충돌 처리를 위한 콜라이더
    private CircleCollider2D _collider = null;
    
    /********************************Sword Impact********************************/
    protected abstract void OnSwordImpact();
    
    
    /*************************Enable and Disable*************************/
    
    private IEnumerator IE_Destroy()
    {
        yield return Dealy;
        
        ProjectileManager.Instance.ReturnProjectileToPool(this, _projectileName);
    }

    public override void GetFromPool()
    {
        base.GetFromPool();
        _collider.enabled = false;
    }
}