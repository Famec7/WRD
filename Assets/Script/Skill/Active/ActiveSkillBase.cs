using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ActiveSkillBase : SkillBase
{
    // indicator의 중심
    public Vector2 PivotPosition { get; set; } = Vector2.zero;

    // 클릭 위치
    public Vector2 ClickPosition { get; set; } = Vector2.zero;

    public float CurrentCoolTime { get; set; } = 0f;

    protected override void Init()
    {
        base.Init();
        DataInit();
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        IndicatorInit();

        if (commandInvoker.IsEmpty)
            ExecuteCoolTimeCommand();
    }

    #region Data

    public ActiveSkillData Data { get; private set; }

    private void DataInit()
    {
        Data = SkillManager.Instance.GetActiveSkillData(skillId);
    }

    #endregion

    # region Command System

    protected CommandInvoker commandInvoker = new CommandInvoker();

    private void Update()
    {
        commandInvoker.Execute();
    }

    public virtual void ExecuteCoolTimeCommand()
    {
        commandInvoker.AddCommand(new CooldownCommand(this));
    }

    public void CancelSkill()
    {
        commandInvoker.Reset();
        ExecuteCoolTimeCommand();
    }

    public virtual void ExecuteSkill()
    {
        commandInvoker.Reset();
        CurrentCoolTime = 0.0f;
        ExecuteCoolTimeCommand();
    }

    public void AddCommand(ICommand command)
    {
        commandInvoker.AddCommand(command);
    }

    public void Undo()
    {
        commandInvoker.Undo();
    }

    #endregion

    #region Active Skill

    public bool IsActive { get; set; } = false;

    /// <summary>
    /// 스킬 사용 시 호출되는 함수
    /// </summary>
    public abstract void UseSkill();

    /// <summary>
    /// 스킬 사용 시 한 번만 호출되는 함수
    /// </summary>
    public abstract void OnActiveEnter();

    /// <summary>
    /// 스킬 사용 중 매 프레임마다 호출되는 함수
    /// </summary>
    /// <returns> 스킬 사용이 끝나지 않았다면 false, 끝났다면 true </returns>
    public abstract bool OnActiveExecute();

    /// <summary>
    /// 스킬 종료 시 한 번만 호출되는 함수
    /// </summary>
    public abstract void OnActiveExit();

    #endregion

    #region Indicator

    [SerializeField] protected IndicatorManager.Type indicatorType;

    public IndicatorManager.Type IndicatorType => indicatorType;

    [SerializeField] private bool _isFixedPosition = false;

    public SkillIndicator Indicator { get; private set; }

    protected virtual void IndicatorInit()
    {
        Indicator = IndicatorManager.Instance.GetIndicator(indicatorType);
        Indicator.SetSkill(this);
    }

    public void ShowIndicator(Vector2 position, bool isRender = true)
    {
        if (_isFixedPosition)
        {
            Vector3 ownerPos = weapon.owner.transform.position;

            Vector2 direction = (Vector2)ownerPos - position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Indicator.transform.rotation = Quaternion.Euler(0, 0, angle);
            Indicator.ShowIndicator(ownerPos, isRender);
        }
        else
        {
            Indicator.ShowIndicator(position, isRender);
        }
    }

    protected void SetIndicatorRange(float range)
    {
        Indicator.transform.localScale = new Vector3(range, range, 1);
    }

    #endregion

    #region Target Monster

    // 스킬이 타겟팅하는 몬스터들 (클릭형 스킬에서 사용)
    [HideInInspector]
    public List<Monster> IndicatorMonsters = new List<Monster>();

    public void ClearTargetMonsters()
    {
        if (IsActive)
            return;

        IndicatorMonsters.Clear();
    }

    #endregion
}