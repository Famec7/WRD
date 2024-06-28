using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    #region private variable
    private WaitForSeconds attackDelay;
    private WeaponData _data;
    private bool _isAttack = false;
    #endregion
    
    # region protected variable
    protected GameObject target = null;
    protected GameObject owner;
    # endregion
    
    [Header("Passive Skill")]
    public PassiveSkillBase passiveSkill;
    [Header("Active Skill")]
    public GameObject activeSkill; // 임시로 GameObject로 선언, 추후 스킬 구현 시 변경 필요
    
    public WeaponData Data
    {
        get => _data;
        set => _data = value;
    }

    #region Event Function
    protected virtual void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_isAttack is false)
            StartCoroutine(CoroutineAttack());
    }

    #endregion

    /// <summary>
    /// 무기 초기화 함수
    /// </summary>
    protected virtual void Init()
    {
        _data = WeaponDataManager.instance.GetWeaponData(GetType().Name);
        attackDelay = new WaitForSeconds(_data.attackSpeed);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 무기 공격 구현
    /// Ex. 총 구현 시 총알 생성 후 적에게 쏘기 / 탐지 범위 다르게 설정
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    /// 무기 장착
    /// </summary>
    /// <param name="ownerTransform"> 무기를 가지고 있는 주체의 transform </param>
    public void EquipWeapon(Transform ownerTransform)
    {
        owner = ownerTransform.gameObject;
        passiveSkill.SetOwnerTransform(ownerTransform);
    }
    
    public void DetachWeapon()
    {
        // 무기 해제
    }
    
    private IEnumerator CoroutineAttack()
    {
        _isAttack = true;
        Attack();
        yield return attackDelay;
        _isAttack = false;
    }
}
