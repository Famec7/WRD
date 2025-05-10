using System.Collections.Generic;
using UnityEngine;

public class SkyProjectile : FallingProjectile
{
    /********************************Target********************************/
    [Header("Offset")]
    [SerializeField] private float _offset = 0.5f;
    
    /********************************Effect********************************/
    private EffectBase _auraEffect;
    
    [SerializeField]
    private SlowZone _slowZone;
    
    /********************************Animation********************************/
    [SerializeField]
    private AnimationClip _animationClip;

    public void SetPosition(Vector3 ownerPosition, Vector3 targetPosition)
    {
        Target = targetPosition;
        
        // 장착한 캐릭터의 위치를 기준으로 타겟 위치를 계산
        Vector2 direction = (targetPosition - ownerPosition).normalized;
        
        // 캐릭터와 타겟 사이의 축이 사분면에 따라 방향을 조절
        int directionX = direction.x > 0 ? -1 : 1;
        
        /*Vector2 offset = new Vector2(Mathf.Cos(_degree * Mathf.Deg2Rad) * directionX, Mathf.Sin(_degree * Mathf.Deg2Rad)) * _radius;*/
        
        // SpriteRenderer의 flip을 이용하여 방향을 조절
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = direction.x > 0;
        
        // 위치와 회전을 조절
        Vector3 position = targetPosition + new Vector3(-direction.x * _offset, _offset, 0);
        transform.position = position;
        
        InitPosition(position);
    }
    
    /********************************Data********************************/
    public override void SetData(SkillData data)
    {
        base.SetData(data);
        
        Damage = data.GetValue(0);
        float slowRate = data.GetValue(2);
        
        Dealy ??= new WaitForSeconds(data.GetValue(1));
        
        _slowZone.SetData(0, data.Range, slowRate);
    }
    
    /********************************Sword Impact********************************/

    protected override void OnSwordImpact()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Range, default, LayerMaskProvider.MonsterLayerMask);
        
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
        
        _slowZone.SetPosition(transform.position);
        _slowZone.PlayEffect();
    }
    
    /*************************Enable and Disable*************************/
    
    public override void ReturnToPool()
    {
        base.ReturnToPool();
        _auraEffect.StopEffect();
        
        transform.position = Vector3.zero;
    }

    public override void GetFromPool()
    {
        base.GetFromPool();

        if (TryGetComponent(out Animator animator))
        {
            animator.Play(_animationClip.name);
        }
    }
}