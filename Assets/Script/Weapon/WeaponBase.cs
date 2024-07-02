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

    [SerializeField] protected CharacterController owner;

    # endregion

    # region Skill

    [Header("Passive Skill")] public PassiveSkillBase passiveSkill = null;
    [Header("Active Skill")] public GameObject activeSkill = null; // 임시로 GameObject로 선언, 추후 스킬 구현 시 변경 필요

    public bool IsPassiveSkillNull => passiveSkill == null;
    public bool IsActiveSkillNull => activeSkill == null;

    #endregion

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
        if (_isAttack is false && !IsTargetNullOrNotInRange())
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
        /*this.gameObject.SetActive(false);*/
    }

    /// <summary>
    /// 무기 공격 구현
    /// Ex. 총 구현 시 총알 생성 후 적에게 쏘기 / 탐지 범위 다르게 설정
    /// </summary>
    protected virtual void Attack()
    {
        if (IsTargetNullOrNotInRange())
            return;

        if (IsPassiveSkillNull)
        {
            if (passiveSkill.Activate())
                return;
        }
    }

    /// <summary>
    /// 무기 장착
    /// </summary>
    /// <param name="owner"> 무기를 가지고 있는 주체의 transform </param>
    public void EquipWeapon(CharacterController owner)
    {
        this.owner = owner;

        if (passiveSkill != null)
            passiveSkill.SetOwner(owner);
        /*if(activeSkill != null)
            activeSkill.GetComponent<SkillBase>().SetOwner(owner);*/
    }

    public void DetachWeapon()
    {
        // 무기 해제
        this.owner = null;
        _isAttack = false;
    }

    private IEnumerator CoroutineAttack()
    {
        _isAttack = true;
        Attack();
        yield return attackDelay;
        _isAttack = false;
    }

    private bool IsTargetNullOrNotInRange()
    {
        return owner.Target is null || Vector3.Distance(owner.Target.transform.position, owner.transform.position) >
            Data.attackRange;
    }
}