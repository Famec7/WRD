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

    protected LayerMask targetLayer;
    
    protected CharacterController owner;
    public void SetWeapon(WeaponBase weapon)
    {
        weaponBase = weapon;
        owner = weapon.owner;
    }

    protected virtual void Init()
    {
        skillName = GetType().Name;
        targetLayer = LayerMaskManager.Instance.MonsterLayerMask;
    }
}