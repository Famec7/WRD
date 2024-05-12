using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _id;
    
    private WeaponData _data;
    protected GameObject target = null;
    
    protected bool isAttack = false;
    
    [Header("Passive Skill")]
    public PassiveSkillBase passiveSkill;
    
    public WeaponData Data
    {
        get => _data;
        set => _data = value;
    }

    #region Callback Function

    /// <summary>
    /// Callback Function
    /// </summary>
    protected virtual void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isAttack)
            StartCoroutine(CoroutineAttack());
        PassiveAura();
    }

    #endregion

    /// <summary>
    /// 무기 초기화 함수
    /// </summary>
    protected virtual void Init()
    {
        _data = WeaponDataManager.instance.GetWeaponData(_id);
    }
    
    private IEnumerator CoroutineAttack()
    {
        isAttack = true;
        Attack();
        yield return new WaitForSeconds(Data.attackSpeed);
        isAttack = false;
    }

    /// <summary>
    /// 무기 공격 구현
    /// Ex. 총 구현 시 총알 생성 후 적에게 쏘기 / 탐지 범위 다르게 설정
    /// </summary>
    protected abstract void Attack();
    
    /// <summary>
    /// 버프 / 디버프 효과
    /// </summary>
    protected abstract void PassiveAura();
    
    public void EquipWeapon(Transform ownerTransform)
    {
        // 무기 장착
        passiveSkill.SetOwnerTransform(ownerTransform);
    }
    
    public void UnequipWeapon()
    {
        // 무기 해제
    }
}