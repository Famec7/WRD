using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponManager : Singleton<WeaponManager>
{
    private PoolManager _poolManager;
    private WeaponBase[] _equippedWeapons;
    
    public event Action<CharacterController> OnWeaponEquipped;
    public event Action<CharacterController> OnWeaponDetached; 
    
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
            for (int i = 0; i < weapon.GetActiveSkillCount(); i++)
            {
                ActiveSkillBase activeSkill = weapon.GetActiveSkill(i);
                activeSkill.enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < weapon.GetActiveSkillCount(); i++)
            {
                ActiveSkillBase activeSkill = weapon.GetActiveSkill(i);
                activeSkill.enabled = false;
            }
        }
        
        SkillManager.Instance.AddSkill(weapon);
        OnWeaponEquipped?.Invoke(owner);
    }
    
    public void RemoveWeapon(int characterIndex)
    {
        OnWeaponDetached?.Invoke(CharacterManager.Instance.GetCharacter(characterIndex));
        
        WeaponBase detachedWeapon = _equippedWeapons[characterIndex];
        detachedWeapon.DetachWeapon();
        
        // 플레이어 장착 해제면 액티브 스킬 해제
        if (characterIndex == (int)CharacterManager.CharacterType.Player)
        {
            for (int i = 0; i < detachedWeapon.GetActiveSkillCount(); i++)
            {
                ActiveSkillBase activeSkill = detachedWeapon.GetActiveSkill(i);
                activeSkill.enabled = false;
            }
        }
        
        SkillManager.Instance.RemoveAllSkill();
        _poolManager.ReturnToPool(detachedWeapon.Data.WeaponName, detachedWeapon);
        
        _equippedWeapons[characterIndex] = null;
    }
    
    public void ChangeWeapon(int fromCharacterIndex, int toCharacterIndex)
    {
        WeaponBase fromWeapon = _equippedWeapons[fromCharacterIndex];
        WeaponBase toWeapon = _equippedWeapons[toCharacterIndex];
        
        // 무기 교체
        fromWeapon?.DetachWeapon();
        toWeapon?.DetachWeapon();
        
        fromWeapon?.EquipWeapon(CharacterManager.Instance.GetCharacter(toCharacterIndex));
        toWeapon?.EquipWeapon(CharacterManager.Instance.GetCharacter(fromCharacterIndex));
        
        _equippedWeapons[fromCharacterIndex] = toWeapon;
        _equippedWeapons[toCharacterIndex] = fromWeapon;
        
        // 플레이어 교체면 액티브 스킬 교체
        if (fromCharacterIndex == (int)CharacterManager.CharacterType.Player)
        {
            if(fromWeapon != null && fromWeapon.TryGetComponent(out ActiveSkillBase fromActiveSkill))
            {
                fromActiveSkill.enabled = false;
            }
            
            if(toWeapon != null && toWeapon.TryGetComponent(out ActiveSkillBase toActiveSkill))
            {
                toActiveSkill.enabled = true;
            }
        }
        
        if (toCharacterIndex == (int)CharacterManager.CharacterType.Player)
        {
            if(fromWeapon != null && fromWeapon.TryGetComponent(out ActiveSkillBase fromActiveSkill))
            {
                fromActiveSkill.enabled = true;
            }
            
            if(toWeapon != null && toWeapon.TryGetComponent(out ActiveSkillBase toActiveSkill))
            {
                toActiveSkill.enabled = false;
            }
        }
        
        SkillManager.Instance.RemoveAllSkill();
        SkillManager.Instance.AddSkill(toWeapon);
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