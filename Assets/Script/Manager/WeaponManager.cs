﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponManager : Singleton<WeaponManager>
{
    private PoolManager _poolManager;
    private WeaponBase[] _equippedWeapons;
    
    protected override void Init()
    {
        _poolManager = GetComponent<PoolManager>();
        
        _equippedWeapons = new WeaponBase[CharacterManager.Instance.CharacterCount];
    }
    
    public void AddWeapon(int characterIndex, int weaponId)
    {
        // 장착할 캐릭터와 무기 찾기
        CharacterController owner = CharacterManager.Instance.GetCharacter(characterIndex);
        WeaponBase weapon = FindWeapon(weaponId);
        
        // 무기 장착
        weapon.EquipWeapon(owner);
        _equippedWeapons[characterIndex] = weapon;
        
        // 액티브 스킬 UI 추가
        if (characterIndex == (int)CharacterManager.CharacterType.Player)
        {
            if(weapon.TryGetComponent(out ActiveSkillBase activeSkill))
            {
                activeSkill.enabled = true;
                SkillManager.Instance.AddActiveSkill(activeSkill);
            }
        }
    }
    
    public void RemoveWeapon(int characterIndex)
    {
        WeaponBase detachedWeapon = _equippedWeapons[characterIndex];
        detachedWeapon.DetachWeapon();
        
        // 플레이어 장착 해제면 액티브 스킬 해제
        if (characterIndex == (int)CharacterManager.CharacterType.Player)
        {
            if(detachedWeapon.TryGetComponent(out ActiveSkillBase activeSkill))
            {
                activeSkill.enabled = false;
                SkillManager.Instance.RemoveActiveSkill(activeSkill);
            }
        }
        
        _poolManager.ReturnToPool(detachedWeapon.Data.WeaponName, detachedWeapon);
        
        _equippedWeapons[characterIndex] = null;
    }
    
    private WeaponBase FindWeapon(int weaponId)
    {
        var weaponData = WeaponDataManager.Instance.GetWeaponData(weaponId);

        if (weaponData is null)
        {
            Debug.LogError($"WeaponData is null. WeaponId: {weaponId}");
            return null;
        }

        var weapon = _poolManager.GetFromPool<WeaponBase>(weaponData.WeaponName);
        
        if (weapon is null)
        {
            Debug.LogError($"Weapon is null. WeaponName: {weaponData.WeaponName}");
            return null;
        }

        return weapon;
    }
}