using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public string skillName;
    
    [HideInInspector]
    public SkillData skillData;
    [HideInInspector]
    public WeaponBase weaponBase;
    
    // 스킬을 장착한 플레이어 또는 펫의 위치
    protected Transform ownerTransform;
    public void SetOwnerTransform(Transform ownerTransform)
    {
        this.ownerTransform = ownerTransform;
    }
    
    protected virtual void Start()
    {
        /*skillData = SkillDataManager.Instance.GetPassiveSkillData(skillName);*/
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