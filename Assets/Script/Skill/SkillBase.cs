using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SkillBase : MonoBehaviour
{
    [HideInInspector]
    public  string skillName;
    
    [FormerlySerializedAs("currentWeapon")] [HideInInspector]
    public WeaponBase weapon;

    protected LayerMask targetLayer;
    
    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
    }

    protected virtual void Init()
    {
        skillName = GetType().Name;
        targetLayer = LayerMaskManager.Instance.MonsterLayerMask;
    }
}