using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ClickTypeSkill : ActiveSkillBase
{
    public override void UseSkill()
    {
        if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
        {
            commandInvoker.Undo();
        }

        commandInvoker.AddCommand(new CheckUsableRangeCommand(this));
    }

    /// <summary>
    /// 클릭한 위치에 있는 몬스터를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    protected Monster SelectMonsterAtClickPosition()
    {
        const float range = 100f;
        
        LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
        Collider2D collider = Physics2D.OverlapCircleAll(ClickPosition, range, layerMask)
                .OrderBy(c => Vector2.Distance(ClickPosition, c.transform.position))
                .FirstOrDefault();

        if (collider is null)
        {
#if UNITY_EDITOR
            Debug.Log("No monster found at click position");
#endif
            return null;
        }

        if (collider.TryGetComponent(out Monster monster))
        {
            return monster;
        }

#if UNITY_EDITOR
        Debug.Log("No monster found at click position");
#endif
        return null;
    }
}