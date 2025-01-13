using UnityEngine;

public class CloverJackEffect : CardEffectBase
{
    private bool _isAttack;
    
    public CloverJackEffect(WeaponBase weapon) : base(weapon)
    {
        Data = SkillManager.Instance.GetActiveSkillData(11);
    }

    public override void OnEnter()
    {
        Weapon.OnAttack += OnAttack;
        _isAttack = false;
    }

    public override INode.ENodeState OnUpdate()
    {
        return _isAttack ? INode.ENodeState.Success : INode.ENodeState.Running;
    }

    public override void OnExit()
    {
        Weapon.OnAttack -= OnAttack;
    }

    private void OnAttack()
    {
        Vector3 targetPosition = Weapon.owner.Target.transform.position;
        Vector3 range = new Vector3(3, 5, 0);
        var layer = LayerMaskProvider.MonsterLayerMask;

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
        
        _isAttack = true;
    }
}