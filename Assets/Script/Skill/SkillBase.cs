using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [SerializeField]
    protected int skillId;
    
    [HideInInspector]
    public WeaponBase weapon;

    protected LayerMask targetLayer;
    
    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
    }

    protected virtual void Init()
    {
        targetLayer = LayerMaskManager.Instance.MonsterLayerMask;
    }
}