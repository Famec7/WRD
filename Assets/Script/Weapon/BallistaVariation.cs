using UnityEngine;

public class BallistaVariation : Ballista
{
    [SerializeField]
    private int _passiveAuraSkillId;

    private float _markDuration;

    protected override void Init()
    {
        base.Init();
        
        PassiveAuraSkillData data = SkillManager.Instance.GetPassiveAuraSkillData(_passiveAuraSkillId);
        if (data != null)
        {
            _markDuration = data.GetValue(0);
        }
    }

    protected override void OnHit(Monster monster, float damage)
    {
        base.OnHit(monster, damage);
        
        Mark mark = new Mark(monster.gameObject, _markDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, mark);
    }
}