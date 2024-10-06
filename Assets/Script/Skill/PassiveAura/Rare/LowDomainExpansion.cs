using System;
using System.Collections.Generic;
using UnityEngine;

public class LowDomainExpansion : PassiveAuraSkillBase
{
    private CircleCollider2D _collider;
    private float _slowDownValue;

    private List<Status> _statusList = new List<Status>();

    protected override void Init()
    {
        base.Init();

        // 범위 조정
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
        _collider.radius = Data.Range * 16;

        // 슬로우 비율 값
        _slowDownValue = Data.GetValue(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, _slowDownValue));

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