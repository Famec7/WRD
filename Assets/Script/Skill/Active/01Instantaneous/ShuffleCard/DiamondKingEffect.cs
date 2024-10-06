using UnityEngine;

public class DiamondKingEffect : CardEffectBase
{
    private float _time;
    private readonly Vector3 _range = new Vector3(3, 5, 0);
    
    public DiamondKingEffect(WeaponBase weapon) : base(weapon)
    {
        Data = SkillManager.Instance.GetActiveSkillData("shuffle - diamond king");
        _time = Data.GetValue(0);
    }

    public override void OnEnter()
    {
        Weapon.OnAttack += OnAttack;
    }

    public override INode.ENodeState OnUpdate()
    {
        _time -= Time.deltaTime;
        return _time <= 0 ? INode.ENodeState.Success : INode.ENodeState.Running;
    }

    public override void OnExit()
    {
        Weapon.OnAttack -= OnAttack;
    }
    
    private void OnAttack()
    {
        Vector3 targetPosition = Weapon.owner.Target.transform.position;
        var layer = LayerMaskManager.Instance.MonsterLayerMask;

        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, _range, default, layer);
        
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(1));

                float amplificationValue = Data.GetValue(3) / 100;
                StatusEffect amplification = new DamageAmplification(monster.gameObject, amplificationValue, Data.GetValue(2));
                StatusEffectManager.Instance.AddStatusEffect(monster.status, amplification);
            }
        }
    }
}