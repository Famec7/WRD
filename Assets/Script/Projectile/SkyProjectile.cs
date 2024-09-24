using System.Collections.Generic;
using UnityEngine;

public class SkyProjectile : FallingProjectile
{
    /********************************Target********************************/
    [Header("Offset")]
    [SerializeField] private float _radius;
    [SerializeField] private float _degree;
    
    /********************************Effect********************************/
    private EffectBase _auraEffect;
    

    public void SetPosition(Vector3 ownerPosition, Vector3 targetPosition)
    {
        Target = targetPosition;
        
        // 장착한 캐릭터의 위치를 기준으로 타겟 위치를 계산
        Vector2 direction = (targetPosition - ownerPosition).normalized;
        
        // 캐릭터와 타겟 사이의 축이 사분면에 따라 방향을 조절
        int directionX = direction.x > 0 ? -1 : 1;
        
        Vector2 offset = new Vector2(Mathf.Cos(_degree * Mathf.Deg2Rad) * directionX, Mathf.Sin(_degree * Mathf.Deg2Rad)) * _radius;
        
        // SpriteRenderer의 flip을 이용하여 방향을 조절
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = direction.x > 0;
        
        // 위치와 회전을 조절
        transform.position = targetPosition + (Vector3)offset;
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
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Range, default, LayerMaskManager.Instance.MonsterLayerMask);
        
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Damage);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new Wound(target.gameObject));
            }
        }
        
        // 슬로우 장판 생성
        _auraEffect = EffectManager.Instance.CreateEffect<EffectBase>("SkySwordAura");
        _auraEffect.SetPosition(transform.position);
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
        
        _auraEffect.StopEffect();
    }
}