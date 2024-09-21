using System;
using System.Collections.Generic;
using UnityEngine;

public class HighDomainExpansion : PassiveAuraSkillBase
{
    private List<Status> _statusList = new List<Status>();
    
    protected override void Init()
    {
        base.Init();
        
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = Data.Range * 16;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, Data.GetValue(0)));
            
            _statusList.Add(status);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(SlowDown));
            
            _statusList.Remove(status);
        }
    }
    
    private void OnDisable()
    {
        foreach (var status in _statusList)
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(SlowDown));
        }
    }
}