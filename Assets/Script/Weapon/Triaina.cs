using UnityEngine;

public class Triaina : MeleeWeapon
{
    #region PassiveAura

    [Space] [Header("trident of poseidon")]
    [SerializeField] private int _skillId = 0;
    [SerializeField] private float _passiveRange = 1.0f;
    private PassiveAuraSkillData _passiveAuraSkillData;

    #endregion

    protected override void Init()
    {
        base.Init();
        _passiveAuraSkillData = SkillManager.Instance.GetPassiveAuraSkillData(_skillId);
    }

    protected override void Attack()
    {
        base.Attack();

        if (owner.Target.TryGetComponent(out Status status))
        {
            ApplyMark(status);
        }
        
        AttackNearbyMonsters();
    }
    
    private void AttackNearbyMonsters()
    {
        Vector3 targetPosition = owner.Target.transform.position;
        LayerMask targetLayer = LayerMaskManager.Instance.MonsterLayerMask;
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
                ApplyMark(monster.status);
            }
        }
    }

    private void ApplyMark(Status status)
    {
        if (status is null)
            return;

        Mark mark = new Mark(status.gameObject);

        StatusEffectManager.Instance.AddStatusEffect(status, mark);
    }
}