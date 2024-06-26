using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PassiveSkillBase : SkillBase
{
    [HideInInspector] public PassiveSkillData data;

    private void Awake()
    {
        Init();
    }

    protected virtual bool CheckTrigger()
    {
        return Random.Range(0, 100) <= data.values[0];
    }

    /// <summary>
    /// 패시브 스킬 발동하면 true, 발동하지 않으면 false
    /// </summary>
    /// <param name="target"></param>
    public abstract bool Activate(GameObject target = null);

    protected override void Init()
    {
        base.Init();
        data = SkillDataManager.Instance.GetPassiveSkillData(GetType().Name);
    }
}