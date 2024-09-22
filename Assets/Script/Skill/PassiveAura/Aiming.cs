using System;
using System.Collections;
using UnityEngine;

public class Aiming : PassiveAuraSkillBase
{
    private WaitForSeconds _waitTime;
    private float _markDuration;
    private float _range;
    
    protected override void Init()
    {
        base.Init();
        
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = Data.Range;
        
        _waitTime = new WaitForSeconds(Data.GetValue(0));
        _markDuration = Data.GetValue(2);
        _range = Data.Range;
    }

    private IEnumerator IE_Aim()
    {
        while (true)
        {
            yield return _waitTime;
            
            ApplyMarkBuff();
        }
    }

    private void ApplyMarkBuff()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, _range, 360f, targetLayer);
        
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Status status))
            {
                StatusEffectManager.Instance.AddStatusEffect(status, new Mark(status.gameObject, _markDuration));
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(IE_Aim());
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}