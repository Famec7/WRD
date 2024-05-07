using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PassiveSkillBase : SkillBase
{
    protected float triggetChance;

    public virtual bool CheckTrigger()
    {
        return true;
        return Random.Range(0, 100) <= triggetChance;
    }

    /// <summary>
    /// 패시브 스킬 발동하면 true, 발동하지 않으면 false
    /// </summary>
    /// <param name="target"></param>
    public abstract bool Activate(GameObject target = null);
}