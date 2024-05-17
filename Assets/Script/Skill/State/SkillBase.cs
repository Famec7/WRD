using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [HideInInspector]
    public string skillName;
    
    [HideInInspector]
    public WeaponBase weaponBase;
    
    // 스킬을 장착한 플레이어 또는 펫의 위치
    [SerializeField]
    protected Transform ownerTransform;
    public void SetOwnerTransform(Transform ownerTransform)
    {
        this.ownerTransform = ownerTransform;
    }
    
    protected virtual void Init()
    {
        skillName = GetType().Name;
        if (TryGetComponent(out weaponBase))
        {
            ;
        }
    }
}