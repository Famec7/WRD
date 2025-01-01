using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    /*************************Movement******************************/
    [Header("Movement")]
    [SerializeField]
    private float _moveSpeed;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    /*************************Attack******************************/
    [Header("Weapon")]
    // 현재 무기 데이터
    [SerializeField]
    private WeaponBase _currentWeapon = null;
    public WeaponBase CurrentWeapon => _currentWeapon;
    
    public void SetCurrentWeapon(WeaponBase weapon)
    {
        _currentWeapon = weapon;
    }
}