using System;
using UnityEngine;

public class Triaina : MeleeWeapon
{
    #region PassiveAura

    [Space] [Header("trident of poseidon")]
    [SerializeField] private float _passiveRange = 1.0f;
    PassiveAuraSkillData _passiveAuraSkillData;

    #endregion

    private void Start()
    {
        _passiveAuraSkillData = GetPassiveAuraSkill().Data;
    }

    protected override void Attack()
    {
        base.Attack();

        if (owner.Target.TryGetComponent(out Status status))
        {
            ApplySlowDown(status);
        }
        
        AttackNearbyMonsters();
    }
    
    private void AttackNearbyMonsters()
    {
        Vector3 targetPosition = owner.Target.transform.position;
        LayerMask targetLayer = LayerMaskProvider.MonsterLayerMask;
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, 1.0f, default, targetLayer);

        int count = (int)_passiveAuraSkillData.GetValue(0);
        
        if (targets.Count < count)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (targets[i].TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.AttackDamage);
                ApplySlowDown(monster.status);
            }
        }
    }

    private void ApplySlowDown(Status status)
    {
        if (status is null)
            return;

        SlowDown slow = new SlowDown(status.gameObject, _passiveAuraSkillData.GetValue(1));

        StatusEffectManager.Instance.AddStatusEffect(status, slow);
    }
}