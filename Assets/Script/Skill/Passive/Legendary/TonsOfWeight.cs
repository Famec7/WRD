using UnityEngine;

public class TonsOfWeight : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger())
        {
            return false;
        }

        Vector2 pivotPos = weapon.owner.transform.position;
        LayerMask monsterlayer = LayerMaskProvider.MonsterLayerMask;
        var targets = RangeDetectionUtility.GetAttackTargets(pivotPos, Data.Range, 360.0f, monsterlayer);

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(1));
                ApplyStun(monster.status);
                ApplyWound(monster.status);
            }
        }
        
        return true;
    }
    
    private void ApplyStun(Status status)
    {
        Stun stun = new Stun(status.gameObject, Data.GetValue(0));
        StatusEffectManager.Instance.AddStatusEffect(status, stun);
    }
    
    private void ApplyWound(Status status)
    {
        Wound wound = new Wound(status.gameObject);
        StatusEffectManager.Instance.AddStatusEffect(status, wound);
    }
}