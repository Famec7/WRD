using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SkillBase : MonoBehaviour
{
    [HideInInspector]
    public  string skillName;
    
    [HideInInspector]
    public WeaponBase weaponBase;
    
    // 스킬을 장착한 플레이어 또는 펫
    [SerializeField]
    protected CharacterController owner;
    public void SetOwner(CharacterController owner)
    {
        this.owner = owner;
        this.transform.SetParent(owner.gameObject.transform);
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