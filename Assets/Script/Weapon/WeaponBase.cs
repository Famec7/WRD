using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class WeaponBase : MonoBehaviour, IObserver
{
    public CharacterController owner;

    private bool _isAttack = false;


    #region Data

    [SerializeField] private int weaponId;

    public WaitForSeconds AttackDelay { get; private set; }

    public WeaponData Data { get; private set; }
    
    public void SetAttackDelay(float attackSpeed)
    {
        AttackDelay = new WaitForSeconds(1 / attackSpeed);
        anim.SetTime(1 / attackSpeed);
    }

    private float _originalAttackDamage;
    private float _originalAttackSpeed;

    #endregion

    # region Skill

    [Header("Passive Skill")] public PassiveSkillBase passiveSkill = null;
    [Header("Active Skill")] public ActiveSkillBase activeSkill = null; // 임시로 GameObject로 선언, 추후 스킬 구현 시 변경 필요

    public bool IsPassiveSkillNull => passiveSkill == null;
    public bool IsActiveSkillNull => activeSkill == null;

    #endregion

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

    [Space] [SerializeField]
    protected AnimationBase anim;

    /// <summary>
    /// 무기 초기화 함수
    /// </summary>
    protected virtual void Init()
    {
        Data = WeaponDataManager.Instance.GetWeaponData(weaponId);
        AttackDelay = new WaitForSeconds(1 / Data.AttackSpeed);
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
        owner.AttachWeapon(this);

        // 스킬 설정 (owner도 같이 설정됨)
        if (IsPassiveSkillNull is false)
            passiveSkill.SetWeapon(this);
        if (IsActiveSkillNull is false)
            activeSkill.SetWeapon(this);

        // 애니메이션 설정
        anim.Owner = this.transform.parent;
    }

    public void DetachWeapon()
    {
        // 무기 해제
        owner.DetachWeapon();

        this.owner = null;
        _isAttack = false;

        ResetStats();

        StopAllCoroutines();
    }

    private void ResetStats()
    {
        Data.AttackDamage = _originalAttackDamage;
        SetAttackDelay(_originalAttackSpeed);
    }

    private IEnumerator CoroutineAttack()
    {
        _isAttack = true;
        Attack();
        yield return AttackDelay;
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

    #region Observer Action (Skill)

    protected UnityAction notifyAction;

    public void AddAction(UnityAction action)
    {
        notifyAction += action;
    }

    public void RemoveAction(UnityAction action)
    {
        notifyAction -= action;
    }

    #endregion
}