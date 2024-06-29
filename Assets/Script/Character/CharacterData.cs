using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    /*************************Sprite******************************/
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    public void FlipSprite(bool isRight)
    {
        spriteRenderer.flipX = isRight;
    }
    
    /*************************Movement******************************/
    [Header("Movement")]
    [SerializeField]
    private float _moveSpeed;

    public float MoveSpeed => _moveSpeed;

    private Vector3 _moveDir;
    public Vector3 MoveDir
    {
        get => _moveDir;
        set
        {
            _moveDir = value;
            FlipSprite(_moveDir.x > 0);
        }
    }


    /*************************Attack******************************/
    [Header("Weapon")]
    // 현재 무기 데이터
    [SerializeField] private WeaponBase _currentWeapon;
    public WeaponBase CurrentWeapon => _currentWeapon;
}