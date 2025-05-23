using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class WeaponBase : MonoBehaviour, IPoolObject
{
    public CharacterController owner;

    private bool _isAttack = false;
    private bool _attackFinished = false;
    public Action OnAttack;

    #region pivot

    [Header("피봇")] [SerializeField] private Pivot _pivot;

    public Pivot Pivot => _pivot;

    #endregion

    #region Data
    [Space][SerializeField] private int weaponNum;


    public WaitForSeconds AttackDelay { get; private set; }

    public WeaponData Data { get; private set; }

    public void SetAttackDelay(float attackSpeed)
    {
        AttackDelay = new WaitForSeconds(1 / attackSpeed);
        Data.AttackSpeed = attackSpeed;

        if (anim != null)
            anim.SetTime(1 / attackSpeed);
    }

    private float _originalAttackDamage;
    private float _originalAttackSpeed;

    #endregion

    # region Skill

    [Header("Passive Skill")] [SerializeField]
    private List<PassiveSkillBase> passiveSkillList;
    
    [Header("Passive Aura Skill")] [SerializeField]
    private List<PassiveAuraSkillBase> passiveAuraSkillList;
    
    [Header("Active Skill")] [SerializeField]
    private List<ActiveSkillBase> activeSkillList;

    public bool IsPassiveSkillNull => passiveSkillList == null;
    public bool IsPassiveAuraSkillNull => passiveAuraSkillList == null;
    public bool IsActiveSkillNull => activeSkillList == null;
    
    public PassiveSkillBase GetPassiveSkill(int index = 0)
    {
        return passiveSkillList[index];
    }
    
    public PassiveAuraSkillBase GetPassiveAuraSkill(int index = 0)
    {
        return passiveAuraSkillList[index];
    }
    
    public ActiveSkillBase GetActiveSkill(int index = 0)
    {
        return activeSkillList[index];
    }
    
    public int GetActiveSkillCount()
    {
        return activeSkillList.Count;
    }
    
    public int GetPassiveSkillCount()
    {
        return passiveSkillList.Count;
    }
    
    public int GetPassiveAuraSkillCount()
    {
        return passiveAuraSkillList.Count;
    }

    #endregion

    #region Event Function

    protected void Awake()
    {
        Init();
    }

    #endregion

    [Space] [SerializeField] protected AnimationBase anim;

    /// <summary>
    /// 무기 초기화 함수
    /// </summary>
    protected virtual void Init()
    {
        Data = WeaponDataManager.Instance.Database.GetWeaponDataByNum(weaponNum);
        SetAttackDelay(Data.AttackSpeed);
        _originalAttackDamage = Data.AttackDamage;
        _originalAttackSpeed = Data.AttackSpeed;

        _pivot.Init(this.transform);
        _pivot.ResetPivot();
        
        SkillInit();
    }

    private void SkillInit()
    {
        // 스킬 설정 (owner도 같이 설정됨)
        if (IsPassiveSkillNull is false)
        {
            foreach (var skill in passiveSkillList)
                skill.SetWeapon(this);
        }

        if (IsPassiveAuraSkillNull is false)
        {
            foreach (var skill in passiveAuraSkillList)
                skill.SetWeapon(this);
        }

        if (IsActiveSkillNull is false)
        {
            foreach (var skill in activeSkillList)
                skill.SetWeapon(this);
        }
    }

    public bool UpdateAttack()
    {
        if (_attackFinished)
        {
            _attackFinished = false;
            return true;
        }
        
        if (_isAttack is false)
        {
            StartCoroutine(CoroutineAttack());
        }

        return false;
    }

    private void AttackBase()
    {
        if (owner.IsTargetNullOrInactive())
        {
            return;
        }
        
        if (anim != null)
        {
            anim.PlayAnimation();
        }

        _notifyAction?.Invoke();
        
        if (OnAttack != null)
        {
            OnAttack.Invoke();
        }
        else
        {
            Attack();
        }
        
        if (!IsPassiveSkillNull)
        {
            foreach (var passiveSkill in passiveSkillList)
            {
                passiveSkill.Activate(owner.Target);
            }
        }
    }

    /// <summary>
    /// 무기 공격 구현
    /// Ex. 총 구현 시 총알 생성 후 적에게 쏘기 / 탐지 범위 다르게 설정
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    /// 무기 장착
    /// </summary>
    /// <param name="owner"> 무기를 가지고 있는 주체의 transform </param>
    public void EquipWeapon(CharacterController owner)
    {
        _pivot.ResetPivot();
        
        this.owner = owner;
        owner.AttachWeapon(this);
    }

    public void DetachWeapon(bool isResetCoolTime = true)
    {
        anim?.StopAnimation();
        
        if (!IsActiveSkillNull)
        {
            foreach (var skill in activeSkillList)
            {
                if (isResetCoolTime)
                    skill.ExecuteSkill();
                else
                    skill.CancelSkill();
            }
        }
        
        // 무기 해제
        owner?.DetachWeapon();
        this.owner = null;
        
        if (isResetCoolTime)
        {
            ResetStats();
            _isAttack = false;
            StopAllCoroutines();
        }
    }

    private void ResetStats()
    {
        Data.AttackDamage = _originalAttackDamage;
        SetAttackDelay(_originalAttackSpeed);
    }

    private IEnumerator CoroutineAttack()
    {
        _isAttack = true;
        AttackBase();
        yield return AttackDelay;
        _isAttack = false;
        _attackFinished = true;
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

    private UnityAction _notifyAction;

    public void AddAction(UnityAction action)
    {
        _notifyAction += action;
    }

    public void RemoveAction(UnityAction action)
    {
        _notifyAction -= action;
    }

    #endregion

    public void GetFromPool()
    {
        ;
    }

    public void ReturnToPool()
    {
        StopAllCoroutines();
        ResetStats();
    }
}