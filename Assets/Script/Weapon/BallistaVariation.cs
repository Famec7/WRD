using System;
using UnityEngine;

public class BallistaVariation : Ballista
{
    private float _markDuration;

    private void Start()
    {
        PassiveAuraSkillData data = GetPassiveAuraSkill(0).Data;
        
        if (data != null)
        {
            _markDuration = data.GetValue(0);
        }
        else
        {
            Debug.LogError("Passive Aura Skill Data not found.");
        }
    }

    protected override void OnHit(Monster monster, float damage)
    {
        base.OnHit(monster, damage);
        
        Mark mark = new Mark(monster.gameObject, _markDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, mark);
    }
}