using UnityEngine;

public class Targeting : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
        {
            return false;
        }

        if (target.TryGetComponent(out Status status))
        {
            Mark mark = new Mark(status.gameObject, Data.GetValue(0));
            StatusEffectManager.Instance.AddStatusEffect(status, mark);
        }

        return true;
    }
}