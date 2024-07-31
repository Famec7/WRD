using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IObserver
{
    #region private variable

    private WaitForSeconds attackDelay;
    
    public WaitForSeconds AttackDelay
    {
        get => attackDelay;
        set => attackDelay = value;
    }
    
    private bool _isAttack = false;

    #endregion

    public CharacterController owner;

    # region Skill

    [Header("Passive Skill")] public PassiveSkillBase passiveSkill = null;
    [Header("Active Skill")] public ActiveSkillBase activeSkill = null; // 임시로 GameObject로 선언, 추후 스킬 구현 시 변경 필요

    public bool IsPassiveSkillNull => passiveSkill == null;
    public bool IsActiveSkillNull => activeSkill == null;

    #endregion

    public WeaponData Data { get; private set; }

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
        Data = WeaponDataManager.Instance.GetWeaponData(GetType().Name);
        attackDelay = new WaitForSeconds(1 / Data.AttackSpeed);
        /*this.gameObject.SetActive(false);*/

        if (IsPassiveSkillNull is false)
            passiveSkill.SetWeapon(this);
        if(IsActiveSkillNull is false)
            activeSkill.SetWeapon(this);
    }

    /// <summary>
    /// 무기 공격 구현
    /// Ex. 총 구현 시 총알 생성 후 적에게 쏘기 / 탐지 범위 다르게 설정
    /// </summary>
    protected virtual void Attack()
    {
        if (IsTargetNullOrNotInRange())
            return;

        if (IsPassiveSkillNull) return;

        if (passiveSkill.Activate(owner.Target))
            return;
    }

    /// <summary>
    /// 무기 장착
    /// </summary>
    /// <param name="owner"> 무기를 가지고 있는 주체의 transform </param>
    public void EquipWeapon(CharacterController owner)
    {
        this.owner = owner;
        this.transform.SetParent(this.owner.transform);

        if (passiveSkill != null)
            passiveSkill.SetWeapon(this);
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
            Data.AttackRange;
    }

    public void OnNotify()
    {
        if (owner.Target is not null)
            StartCoroutine(CoroutineAttack());
    }
}