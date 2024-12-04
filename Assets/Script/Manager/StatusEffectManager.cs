using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : Singleton<StatusEffectManager>
{
    private readonly Dictionary<Status, List<StatusEffect>> _statusEffects = new();
    
    protected override void Init()
    {
        ;
    }
    
    public void AddStatusEffect(Status status, StatusEffect statusEffect)
    {
        if (_statusEffects.ContainsKey(status))
        {
            var existingStatusEffect = GetStatusEffect(status, statusEffect.GetType());

            // 이미 존재하는 상태이상이라면 지속시간만 추가
            if (existingStatusEffect != null)
            {
                existingStatusEffect.CombineEffect(statusEffect);
            }
            // 존재하지 않는 상태이상이라면 리스트에 추가
            else
            {
                _statusEffects[status].Add(statusEffect);
                statusEffect.ApplyEffect();
            }
        }
        else
        {
            _statusEffects.Add(status, new List<StatusEffect> {statusEffect});
            statusEffect.ApplyEffect();
        }
    }
    
    public void RemoveStatusEffect(Status status, Type statusEffectType)
    {
        if (_statusEffects.ContainsKey(status))
        {
            StatusEffect removedStatusEffect  = GetStatusEffect(status, statusEffectType);
            if (removedStatusEffect != null)
            {
                removedStatusEffect.RemoveEffect();
                _statusEffects[status].Remove(removedStatusEffect);
            }
        }
    }
    
    public StatusEffect GetStatusEffect(Status status, Type statusEffectType)
    {
        if (_statusEffects.ContainsKey(status))
        {
            return _statusEffects[status].Find(effect => effect.GetType() == statusEffectType);
        }

        return null;
    }
}