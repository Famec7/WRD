using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class WeaponBase : MonoBehaviour, IObserver, IPoolObject
{
    public CharacterController owner;

    private bool _isAttack = false;

    #region pivot

    [Header("피봇")] [SerializeField] private Pivot _pivot;

    public Pivot Pivot => _pivot;

    #endregion

    #region Data

    [Space] [SerializeField] private int weaponId;

    public WaitForSeconds AttackDelay { get; private set; }

    public WeaponData Data { get; private set; }

    public void SetAttackDelay(float attackSpeed)
    {
        AttackDelay = new WaitForSeconds(1 / attackSpeed);

        if (anim != null)
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

    [Space] [SerializeField] protected AnimationBase anim;

    /// <summary>
    /// 무기 초기화 함수
    /// </summary>
    protected virtual void Init()
    {
        Data = WeaponDataManager.Instance.GetWeaponData(weaponId);
        SetAttackDelay(Data.AttackSpeed);
        _originalAttackDamage = Data.AttackDamage;
        _originalAttackSpeed = Data.AttackSpeed;

        _pivot.Init(this.transform);
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
        {
            _isAttack = false;
            StopCoroutine(CoroutineAttack());
        }

        if (anim != null)
        {
            anim.PlayAnimation();
        }
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
        if (anim != null)
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
        if (owner.Target is null || owner.Target.gameObject.activeSelf is false)
            return true;

        float distance = Vector3.Distance(owner.Target.transform.position, owner.transform.position);

        return distance > Data.AttackRange;
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

    public void GetFromPool()
    {
        _pivot.ResetPivot();
    }

    public void ReturnToPool()
    {
        StopAllCoroutines();
        ResetStats();
    }
}