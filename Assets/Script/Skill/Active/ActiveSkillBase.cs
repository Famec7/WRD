using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ActiveSkillBase : SkillBase
{
    public Vector2 PivotPosition { get; set; } = Vector2.zero;
    public Vector2 ClickPosition { get; set; } = Vector2.zero;

    // 스킬 버튼 활성화 이벤트
    public Action<bool> OnButtonActivate;

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
        if (_commandInvoker.IsEmpty)
            _commandInvoker.SetCommand(new CooldownCommand(this));
        
        IndicatorInit();
    }
    
    #region Data

    public ActiveSkillData Data { get; private set; }
    
    [SerializeField]
    private Sprite _skillIcon;
    
    public Sprite SkillIcon => _skillIcon;

    private void DataInit()
    {
        Data = SkillManager.Instance.GetActiveSkillData(skillId);
    }

    #endregion
    
    # region Command System

    protected CommandInvoker _commandInvoker = new CommandInvoker();
    
    private void Update()
    {
        _commandInvoker.Execute();
    }
    
    public virtual void CancelSkill()
    {
        _commandInvoker.Reset();
    }
    
    public void AddCommand(ICommand command)
    {
        _commandInvoker.SetCommand(command);
    }
    
    #endregion
    
    #region Active Skill
    
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

    private void IndicatorInit()
    {
        SkillIndicator indicator = IndicatorManager.Instance.GetIndicator(indicatorType);
        indicator.SetSkill(this);
    }
    
    public void ShowIndicator(Vector2 position)
    {
        if (_isFixedPosition)
        {
            Vector3 ownerPos = weapon.owner.transform.position;

            Vector2 direction = (Vector2)ownerPos - position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            IndicatorManager.Instance.ShowIndicator(ownerPos, IndicatorType, angle);
        }
        else
        {
            IndicatorManager.Instance.ShowIndicator(position, IndicatorType);
        }
    }
    
    #endregion

    #region Target Monster
    
    // 스킬이 타겟팅하는 몬스터들 (클릭형 스킬에서 사용)
    protected readonly HashSet<Monster> targetMonsters = new HashSet<Monster>();
    
    public void AddTargetMonster(Monster monster)
    {
        targetMonsters.Add(monster);
    }

    public void RemoveTargetMonster(Monster monster)
    {
        targetMonsters.Remove(monster);
    }
    
    public void ClearTargetMonsters()
    {
        targetMonsters.Clear();
    }

    #endregion
}