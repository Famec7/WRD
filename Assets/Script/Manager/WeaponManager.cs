using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField] private List<CharacterController> _characters;
    
    private PoolManager _poolManager;
    private readonly List<WeaponBase> _equippedWeapons = new List<WeaponBase>();
    
    protected override void Init()
    {
        _poolManager = GetComponent<PoolManager>();
        
        // 캐릭터 수만큼 무기 리스트 초기화
        _equippedWeapons.Capacity = _characters.Count;
    }
    
    public void AddWeapon(int characterIndex, int weaponId)
    {
        // 장착할 캐릭터와 무기 찾기
        CharacterController owner = _characters[characterIndex];
        WeaponBase weapon = FindWeapon(weaponId);
        
        // 무기 장착
        weapon.EquipWeapon(owner);
        _equippedWeapons[characterIndex] = weapon;
    }
    
    public void RemoveWeapon(int characterIndex)
    {
        _equippedWeapons[characterIndex].DetachWeapon();
    }
    
    private WeaponBase FindWeapon(int weaponId)
    {
        return _poolManager.GetFromPool<WeaponBase>(weaponId);
    }
}