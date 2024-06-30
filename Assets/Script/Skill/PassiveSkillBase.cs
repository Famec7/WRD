using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PassiveSkillBase : SkillBase
{
    private PassiveSkillData _data;
    public PassiveSkillData Data => _data;

    private void Awake()
    {
        Init();
    }

    protected virtual bool CheckTrigger()
    {
#if PASSIVE_SKILL_DEBUG
        Debug.Log($"{GetType().Name} 발동 확률: {_data.Chance}");
        if (Random.Range(0, 100) <= _data.Chance)
        {
            Debug.Log($"{GetType().Name} 발동");
            return true;
        }
#endif
        return Random.Range(0, 100) <= _data.Chance;
    }

    /// <summary>
    /// 패시브 스킬 발동하면 true, 발동하지 않으면 false
    /// </summary>
    /// <param name="target"></param>
    public abstract bool Activate(GameObject target = null);

    protected override void Init()
    {
        base.Init();
        _data = SkillDataManager.Instance.GetPassiveSkillData(GetType().Name);
    }
}