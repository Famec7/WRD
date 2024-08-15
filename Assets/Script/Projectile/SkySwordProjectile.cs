using System.Collections.Generic;
using UnityEngine;

public class SkySwordProjectile : FallingSwordProjectile
{
    /********************************Target********************************/
    [Header("Offset")]
    [SerializeField] private float _radius;
    [SerializeField] private float _degree;

    public void SetPosition(Vector3 ownerPosition, Vector3 targetPosition)
    {
        Target = targetPosition;
        
        Vector2 direction = (targetPosition - ownerPosition).normalized;
        Vector2 offset = new Vector2(Mathf.Cos(_degree * Mathf.Deg2Rad), Mathf.Sin(_degree * Mathf.Deg2Rad)) * _radius;
        
        transform.position = targetPosition + (Vector3)offset;

        transform.Rotate(direction.x < 0 ? Vector3.back : Vector3.forward, _degree);
    }
    
    /********************************Data********************************/
    private float _slowRate = 0.0f;
    
    public override void SetData(SkillData data)
    {
        base.SetData(data);
        
        Damage = data.GetValue(0);
        _slowRate = data.GetValue(2);
        
        Dealy ??= new WaitForSeconds(data.GetValue(1));
    }
    
    /********************************Sword Impact********************************/

    protected override void OnSwordImpact()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Vector2.zero, Range, default, LayerMaskManager.Instance.MonsterLayerMask);
        
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Damage);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new Wound(target.gameObject));
            }
        }
    }
    
    /********************************SlowDown Effect********************************/

    #region SlowDown Effect
    
    private List<Monster> _monsters = new List<Monster>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            // monster에게 slowEffect를 적용
            StatusEffectManager.Instance.AddStatusEffect(monster.status, new SlowDown(other.gameObject, _slowRate));
            _monsters.Add(monster);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            // monster에게 slowEffect를 해제
            StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(SlowDown));
            _monsters.Remove(monster);
        }
    }
    
    #endregion
    
    /*************************Enable and Disable*************************/
    
    public override void GetFromPool()
    {
        base.GetFromPool();
        _monsters.Clear();
    }
    
    public override void ReturnToPool()
    {
        base.ReturnToPool();
        
        foreach (var monster in _monsters)
        {
            StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(SlowDown));
        }
    }
}