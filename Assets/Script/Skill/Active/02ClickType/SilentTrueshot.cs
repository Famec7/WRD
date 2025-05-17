using UnityEngine;

public class SilentTrueshot : ClickTypeSkill
{
    private float _damageDelay = 0.0f;
    private float _damage = 0.0f;

    protected override void Init()
    {
        base.Init();
        
        _damage = Data.GetValue(1);
    }

    public override void OnActiveEnter()
    {
        _damageDelay = Data.GetValue(0);
        ClearTargetMonsters();
        
        weapon.enabled = false;
        weapon.owner.enabled = false;
    }

    public override bool OnActiveExecute()
    {
        if (_damageDelay > 0.0f)
        {
            _damageDelay -= Time.deltaTime;
            return false;
        }
        
        LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
        IndicatorMonsters = RangeDetectionUtility.GetAttackTargets(Indicator.Collider, layerMask);
        
        foreach (var monster in IndicatorMonsters)
        {
            if (HasTargetMark(monster))
            {
                monster.HasAttacked(_damage);
            }
        }

        return true;
    }

    public override void OnActiveExit()
    {
        weapon.enabled = true;
        weapon.owner.enabled = true;
    }

    private bool HasTargetMark(Monster monster)
    {
        StatusEffect mark = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

        return mark != null;
    }
}