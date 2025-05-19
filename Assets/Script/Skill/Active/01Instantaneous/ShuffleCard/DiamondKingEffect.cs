using UnityEngine;

public class DiamondKingEffect : CardEffectBase
{
    private float _time;
    private readonly Vector3 _range = new Vector3(3, 5, 0);
    
    public DiamondKingEffect(WeaponBase weapon) : base(weapon)
    {
        Data = SkillManager.Instance.GetActiveSkillData(13);
        _time = Data.GetValue(0);
    }

    public override void OnEnter()
    {
        Weapon.OnAttack += OnAttack;
    }

    public override bool OnUpdate()
    {
        _time -= Time.deltaTime;
        return _time <= 0 ? true : false;
    }

    public override void OnExit()
    {
        Weapon.OnAttack -= OnAttack;
    }
    
    private void OnAttack()
    {
        Vector3 targetPosition = Weapon.owner.Target.transform.position;
        var layer = LayerMaskProvider.MonsterLayerMask;

        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, _range, default, layer);
        
        var effect = EffectManager.Instance.CreateEffect<ParticleEffect>("DiamondKing");
        effect.SetPosition(Weapon.owner.transform.position);
        //effect.SetScale(_range);
        effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, targetPosition)));
        effect.PlayEffect();
        
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