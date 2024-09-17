using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class PassiveSkillBase : SkillBase
{
    private PassiveSkillData _data;
    public PassiveSkillData Data => _data;

    #region DEBUG

    private bool _isTest = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isTest = !_isTest;
            Debug.Log($"{GetType().Name} 테스트 모드 : {_isTest}");
        }
    }

    #endregion

    private void Awake()
    {
        Init();
    }

    protected virtual bool CheckTrigger()
    {
        float chance = Random.Range(0, 100);
        
#if UNITY_EDITOR
        if (chance <= _data.Chance || _isTest)
        {
            Debug.Log($"{GetType().Name} 발동");
            return true;
        }
#endif
        return chance <= _data.Chance;
    }

    /// <summary>
    /// 패시브 스킬 발동하면 true, 발동하지 않으면 false
    /// </summary>
    /// <param name="target"></param>
    public abstract bool Activate(GameObject target = null);

    protected override void Init()
    {
        base.Init();
        _data = SkillManager.Instance.GetPassiveSkillData(GetType().Name);
    }

    protected void SetStatusEffect(Monster target, StatusEffect statusEffect)
    {
        StatusEffectManager.Instance.AddStatusEffect(target.status, statusEffect);
    }
}