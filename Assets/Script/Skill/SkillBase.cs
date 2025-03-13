using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("스킬 아이디")]
    [SerializeField]
    protected int skillId;
    
    [Header("스킬 아이콘")]
    [SerializeField] private Sprite _skillIcon;

    public Sprite SkillIcon => _skillIcon;
    
    [HideInInspector]
    public WeaponBase weapon;

    protected LayerMask targetLayer;
    
    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
    }

    protected virtual void Init()
    {
        targetLayer = LayerMaskProvider.MonsterLayerMask;
    }
}