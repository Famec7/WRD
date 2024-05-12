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
    
    public virtual bool CheckTrigger()
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
    
    /// <summary>
    ///  무기 공격 범위 내의 적 탐지
    /// </summary>
    /// <param name="position"> 탐지 범위 중심 </param>
    /// <param name="size"> 가로 세로 사이즈 (벡터)</param>
    /// <returns></returns>
    protected virtual List<Collider2D> GetAttackTargets(Vector2 position, Vector2 size)
    {
        LayerMask layerMask = LayerMask.GetMask("Monster", "Boss");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, size, layerMask);

        return colliders.ToList();
    }
}