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
        if (status.gameObject.activeSelf == false)
        {
            return;
        }
        
        if (_statusEffects.ContainsKey(status))
        {
            _statusEffects[status].Add(statusEffect);
        }
        else
        {
            _statusEffects.Add(status, new List<StatusEffect> { statusEffect });
        }

        statusEffect.ApplyEffect();
    }

    public void RemoveStatusEffect(Status status, Type statusEffectType)
    {
        if (_statusEffects.ContainsKey(status))
        {
            StatusEffect removedStatusEffect = GetStatusEffect(status, statusEffectType);
            if (removedStatusEffect != null)
            {
                removedStatusEffect.RemoveEffect();
                RemoveValue(status, removedStatusEffect);
            }
        }
    }
    
    public void RemoveStatusEffect(Status status, StatusEffect statusEffect)
    {
        if (_statusEffects.ContainsKey(status))
        {
            statusEffect.RemoveEffect();
            RemoveValue(status, statusEffect);
        }
    }

    public void RemoveValue(Status status, StatusEffect statusEffect)
    {
        if (_statusEffects.TryGetValue(status, out List<StatusEffect> effect))
        {
            effect.Remove(statusEffect);
            
            if (effect.Count == 0)
            {
                _statusEffects.Remove(status);
            }
        }
    }
    
    public void RemoveAllStatusEffects(Status status)
    {
        if (_statusEffects.ContainsKey(status))
        {
            _statusEffects.Remove(status);
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
    
    public List<Status> GetAllStatusEffects(Type statusEffectType)
    {
        List<Status> statusList = new();
        foreach (var statusEffect in _statusEffects)
        {
            if (statusEffect.Value.Exists(effect => effect.GetType() == statusEffectType))
            {
                statusList.Add(statusEffect.Key);
            }
        }

        return statusList;
    }
}