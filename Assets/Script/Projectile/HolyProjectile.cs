using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyProjectile : FallingProjectile
{
    #region Data
    private float _stunTime = 0.0f; // 스턴 시간
    private WaitForSeconds _interval = null; // 디버프 간격
    private float _lightningDamage = 0.0f; // 빛 데미지
    private int _maxTargetCount = 0; // 최대 타겟 수
    
    public override void SetData(SkillData data)
    {
        base.SetData(data);
        
        Damage = data.GetValue(0);
        _stunTime = data.GetValue(1);
        
        Dealy ??= new WaitForSeconds(data.GetValue(2));
        
        float damageIncreaseRate = data.GetValue(3);
        _interval ??= new WaitForSeconds(data.GetValue(4));
        _lightningDamage = data.GetValue(5);
        _maxTargetCount = (int)data.GetValue(6);

        _damageAmplificationZone.SetData(0, data.Range, damageIncreaseRate);
    }

    #endregion

    private EffectBase _auraEffect;
    
    [SerializeField]
    private DamageAmplificationZone _damageAmplificationZone;
    
    [SerializeField]
    private AnimationClip _animationClip;
    
    protected override void OnSwordImpact()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Range, default, LayerMaskProvider.MonsterLayerMask);

        foreach (var target in targets)
        {
            if(target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Damage);
                Stun stun = new Stun(monster.gameObject, _stunTime);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);
                
                StartCoroutine(IE_Lightning());
            }
        }
        
        _auraEffect = EffectManager.Instance.CreateEffect<EffectBase>("HolySwordAura");
        _auraEffect.SetPosition(transform.position);
        _auraEffect.PlayEffect();
        
        _damageAmplificationZone.SetPosition(transform.position);
        _damageAmplificationZone.PlayEffect();
    }

    #region Lightning Attack

    private IEnumerator IE_Lightning()
    {
        while (true)
        {
            var targets = RangeDetectionUtility.GetAttackTargets(transform.position, 0.5f, default, LayerMaskProvider.MonsterLayerMask);

            _maxTargetCount = Mathf.Min(_maxTargetCount, targets.Count);
            for(int i = 0; i < _maxTargetCount; i++)
            {
                if (targets[i].TryGetComponent(out Monster monster))
                {
                    var wound = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));

                    if (wound is null) continue;
                    
                    StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(Wound));
                    monster.HasAttacked(_lightningDamage);
                }
            }
            
            yield return _interval;
        }
    }

    #endregion

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        
        _auraEffect.StopEffect();
        _damageAmplificationZone.StopEffect();
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
