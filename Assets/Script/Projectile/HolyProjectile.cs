using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyProjectile : FallingProjectile
{
    #region Data

    private float _stunTime = 0.0f; // 스턴 시간
    private float _damageIncreaseRate = 0.0f; // 데미지증가율
    private WaitForSeconds _interval = null; // 디버프 간격
    private float _lightningDamage = 0.0f; // 빛 데미지
    private int _maxTargetCount = 0; // 최대 타겟 수
    
    public override void SetData(SkillData data)
    {
        base.SetData(data);
        
        Damage = data.GetValue(0);
        _stunTime = data.GetValue(1);
        
        Dealy ??= new WaitForSeconds(data.GetValue(2));
        
        _damageIncreaseRate = data.GetValue(3);
        _interval ??= new WaitForSeconds(data.GetValue(4));
        _lightningDamage = data.GetValue(5);
        _maxTargetCount = (int)data.GetValue(6);
    }

    #endregion

    private EffectBase _auraEffect;
    
    protected override void OnSwordImpact()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Range, default, LayerMaskManager.Instance.MonsterLayerMask);

        foreach (var target in targets)
        {
            if(target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Damage);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new SlowDown(monster.gameObject, 100f, _stunTime));

                StartCoroutine(IE_Lightning());
            }
        }
        
        _auraEffect = EffectManager.Instance.CreateEffect<EffectBase>("HolySwordAura");
        _auraEffect.SetPosition(transform.position);
        _auraEffect.PlayEffect();
    }

    #region Damage Increase Effect

    private List<Monster> _monsters = new List<Monster>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            _monsters.Add(monster);
            
            StatusEffectManager.Instance.AddStatusEffect(monster.status, new DamageAmplification(monster.gameObject, _damageIncreaseRate));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            _monsters.Remove(monster);
            
            StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(DamageAmplification));
        }
    }
    
    #endregion

    #region Lightning Attack

    private IEnumerator IE_Lightning()
    {
        while (true)
        {
            var targets = RangeDetectionUtility.GetAttackTargets(transform.position, 0.5f, default, LayerMaskManager.Instance.MonsterLayerMask);

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
            StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(DamageAmplification));
        }
        
        _auraEffect.StopEffect();
    }
}
