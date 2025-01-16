using System.Collections.Generic;
using UnityEngine;

public abstract class ClickTypeSkill : ActiveSkillBase
{
    public override void UseSkill()
    {
        _commandInvoker.AddCommand(new CheckUsableRangeCommand(this));
    }

    /***************************Behaviour Tree***************************/
    #region Targeting

    protected GameObject target;
    
    public bool FindTarget()
    {
        if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
        {
            if (weapon.owner.Target is null)
            {
                target = weapon.owner.FindNearestTarget();
            }
            else if (weapon.owner.Target.TryGetComponent(out Monster monster))
            {
                target = monster.gameObject;
            }
            else
            {
                target = null;
            }
            
            if (target != null)
                ClickPosition = target.transform.position;
        }
        else
        {
            LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
            Collider2D collider = Physics2D.OverlapPoint(ClickPosition, layerMask);
            target = collider != null ? collider.gameObject : null;
        }
        
        return target != null;
    }

    #endregion
}