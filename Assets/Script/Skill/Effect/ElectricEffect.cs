using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ElectricEffect : EffectBase
{
    #region Data
    
    private float _slowRate = 0f;
    private float _shockDuration = 0f;
    private float _damageAmplification = 0f;
    
    public void SetData(float slowRate, float shockDuration, float damageAmplification)
    {
        _slowRate = slowRate;
        _shockDuration = shockDuration;
        _damageAmplification = damageAmplification;
    }
    
    #endregion
    
    protected override void Init()
    {
        ;
    }

    public override void PlayEffect()
    {
        ;
    }

    public override void StopEffect()
    {
        RemoveAllStatus();
        
        EffectManager.Instance.ReturnEffectToPool(this, "ThunderEffect");
    }

    #region StatusEffect

    private readonly List<Status> _statusList = new List<Status>();
    
    private void AddStatus(Status status, StatusEffect statusEffect)
    {
        _statusList.Add(status);
        
        StatusEffectManager.Instance.AddStatusEffect(status, statusEffect);
    }

    private void RemoveStatus(Status status, Type type)
    {
        _statusList.Remove(status);
        
        StatusEffectManager.Instance.RemoveStatusEffect(status, type);
    }

    private void RemoveAllStatus()
    {
        foreach (var status in _statusList)
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(SlowDown));
        }
        
        _statusList.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Status status))
        {
            AddStatus(status, new SlowDown(other.gameObject, _slowRate));

            if (status.WoundStack > 0)
            {
                StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(Wound));
                StatusEffectManager.Instance.AddStatusEffect(status, new ElectricShock(other.gameObject, _damageAmplification, _shockDuration));
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent(out Status status))
        {
            RemoveStatus(status, typeof(SlowDown));
        }
    }

    #endregion
}