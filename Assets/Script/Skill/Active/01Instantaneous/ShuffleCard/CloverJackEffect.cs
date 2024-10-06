using UnityEngine;

public class CloverJackEffect : CardEffectBase
{
    protected CloverJackEffect(WeaponBase weapon) : base(weapon)
    {
        Data = SkillManager.Instance.GetActiveSkillData("shuffle - clover jack");
    }

    public override void OnEnter()
    {
        Weapon.enabled = false;
    }

    public override INode.ENodeState OnUpdate()
    {
        if (Weapon.owner.Target is null)
        {
            return INode.ENodeState.Running;
        }
        
        Vector3 targetPosition = Weapon.owner.Target.transform.position;
        Vector3 range = new Vector3(3, 5, 0);
        var layer = LayerMaskManager.Instance.MonsterLayerMask;

        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, range, default, layer);
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));

                float damageAmplification = Data.GetValue(2);
                float duration = Data.GetValue(1);
                StatusEffect amplification = new DamageAmplification(monster.gameObject, damageAmplification, duration);
                
                StatusEffectManager.Instance.AddStatusEffect(monster.status, amplification);
            }
        }
        
        return INode.ENodeState.Success;
    }

    public override void OnExit()
    {
        Weapon.enabled = true;
    }
}