using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillBase : SkillBase
{
    #region Data

    public ActiveSkillData Data { get; private set; }

    private void DataInit()
    {
        Data = SkillManager.Instance.GetActiveSkillData(skillId);
    }

    #endregion

    protected readonly HashSet<Monster> targetMonsters = new HashSet<Monster>();
    protected Vector2 pivotPosition = Vector2.zero;
    protected Vector2 clickPosition = Vector2.zero;
    
    protected override void Init()
    {
        base.Init();
        DataInit();
        BTInit();
    }

    private void Awake()
    {
        Init();
        _currentCoolTime = 0;
    }

    private void Start()
    {
        IsCoolTime = true;
        IndicatorInit();
    }

    private void Update()
    {
        _btRunner.Operator();
    }

    public abstract void UseSkill();

    public virtual void CancelSkill()
    {
        IsActive = false;
        IsIndicatorState = false;
    }

    public void AddTargetMonster(Monster monster)
    {
        targetMonsters.Add(monster);
    }
    
    public void RemoveTargetMonster(Monster monster)
    {
        targetMonsters.Remove(monster);
    }

    /***************************Behaviour Tree***************************/

    #region Behaviour Tree

    private BehaviourTreeRunner _btRunner;

    private void BTInit()
    {
        _btRunner = new BehaviourTreeRunner(SettingBT());
    }

    /// <summary>
    /// 스킬 타입마다 다른 행동트리를 설정
    /// </summary>
    /// <returns></returns>
    protected abstract INode SettingBT();

    /*******************행동 노드*******************/
    /*
     * 행동 노드는 스킬의 행동을 정의하는 노드
     * 각 노드마다 CheckState함수들을 만들어 노드 상태 체크 -> 안하면 피곤해짐
     * 쿭타임과 실제 스킬범위를 표시하는 노드는 기본적으로 구현되어 있음
     * 액티브 노드는 스킬에서 직접 구현해야 함
     */

    #region CoolTime Node

    /// <summary>
    /// 스킬 쿨타임 노드
    /// 쿨타임 관련 노드들을 재설정할 수 있음
    /// </summary>
    /// <returns></returns>
    protected virtual List<INode> CoolTimeNodes()
    {
        return new List<INode> { new ActionNode(CheckCoolTimeState), new ActionNode(CoolTimeDown), };
    }

    // 스킬 버튼 활성화 이벤트
    public Action<bool> OnButtonActivate;

    private bool _isCoolTime = true;

    public bool IsCoolTime
    {
        set
        {
            if (_isCoolTime == value)
                return;
            
            _isCoolTime = value;

            if (value is true)
            {
                if (weapon.owner != null)
                    weapon.owner.enabled = true;

                SkillUIManager.Instance.ClosePopupPanel();
            }
            else
            {
                OnButtonActivate?.Invoke(true);
            }
        }
        get => _isCoolTime;
    }

    private float _currentCoolTime = 0f;

    public float CurrentCoolTime
    {
        get => _currentCoolTime;
        set
        {
            _currentCoolTime = value;

            if (value <= 0)
            {
                _currentCoolTime = 0;
                IsCoolTime = false;

                if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
                {
                    IsActive = true;
                }
            }
        }
    }

    private INode.ENodeState CheckCoolTimeState()
    {
        return IsCoolTime is true ? INode.ENodeState.Success : INode.ENodeState.Failure;
    }

    private INode.ENodeState CoolTimeDown()
    {
        CurrentCoolTime -= Time.deltaTime;

#if UNITY_EDITOR

        // T 입력 시 쿨타임 초기화
        if (Input.GetKeyDown(KeyCode.T))
        {
            CurrentCoolTime = 1f;
        }

#endif

        return CurrentCoolTime <= 0 ? INode.ENodeState.Success : INode.ENodeState.Running;
    }

    #endregion

    #region Indicator Node

    /// <summary>
    /// 스킬 실제 사용 범위 표시 노드
    /// 재정의 가능
    /// </summary>
    /// <returns></returns>
    protected virtual List<INode> IndicatorNodes()
    {
        return new List<INode> { new ActionNode(CheckIndicatorState), new ActionNode(TouchIndicator), };
    }

    protected float preparingTime;
    private bool _isIndicatorState = false;

    protected bool IsIndicatorState
    {
        set
        {
            _isIndicatorState = value;

            if (_isIndicatorState is false)
            {
                IndicatorManager.Instance.HideIndicator(indicatorType);
            }
            else
            {
                preparingTime = 3f;

                IndicatorManager.Instance.ShowIndicator(pivotPosition, indicatorType, _isFixedPosition);
                targetMonsters.Clear();

                SkillUIManager.Instance.ShowPopupPanel(1);
            }
        }

        get => _isIndicatorState;
    }

    private INode.ENodeState CheckIndicatorState()
    {
        return IsIndicatorState is true ? INode.ENodeState.Success : INode.ENodeState.Failure;
    }

    private INode.ENodeState TouchIndicator()
    {
        preparingTime -= Time.deltaTime;
        if (preparingTime <= 0f)
        {
            CancelSkill();
            return INode.ENodeState.Failure;
        }

        if (UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("SkillUI")))
        {
            return INode.ENodeState.Running;
        }


        if (Input.GetMouseButtonUp(0))
        {
            // UI 터치 시 스킬 취소
            if (UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("UI")))
            {
                CancelSkill();
                return INode.ENodeState.Failure;
            }

            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var indicator = Physics2D.OverlapPoint(clickPosition, LayerMask.GetMask("Indicator"));

            if (this is ClickTypeSkill)
            {
                if(indicator is null)
                {
                    CancelSkill();
                    return INode.ENodeState.Failure;
                }
            }

            IsIndicatorState = false;
            IsActive = true;

            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Running;
    }

    #endregion

    #region Active Nodes

    /// <summary>
    /// 액티브 노드 재정의 가능
    /// OnActiveExecute에서 코드가 너무 많으면
    /// 다른 함수로 나눠서 구현하는 것이 좋을듯
    /// </summary>
    /// <returns></returns>
    protected virtual List<INode> ActiveNodes()
    {
        return new List<INode> { new ActionNode(CheckActiveState), new ActionNode(OnActiveExecute), };
    }

    private bool _isActive = false;

    protected bool IsActive
    {
        set
        {
            _isActive = value;

            if (_isActive is true)
            {
                SkillUIManager.Instance.ShowPopupPanel(3);

                OnButtonActivate?.Invoke(false);

                OnActiveEnter();
                
                _currentCoolTime = Data.CoolTime;
            }
            else
            {
                OnActiveExit();
                
                IsCoolTime = true;
            }
        }
        get => _isActive;
    }

    private INode.ENodeState CheckActiveState()
    {
        return IsActive is true ? INode.ENodeState.Success : INode.ENodeState.Failure;
    }

    /// <summary>
    /// 스킬 실행 시 한 번만 호출되는 진입 함수
    /// </summary>
    protected abstract void OnActiveEnter();

    /// <summary>
    /// 스킬 Update함수
    /// INode.Running 반환 시 계속 실행
    /// 스킬 종료 시 아래 조건 지켜야함
    /// 1. IsActive = false
    /// 2. INode.ENodeState.Success 반환
    /// </summary>
    /// <returns> Running과 Success 중 하나 반환</returns>
    protected abstract INode.ENodeState OnActiveExecute();

    /// <summary>
    /// 스킬 종료 시 한 번만 호출되는 함수
    /// </summary>
    protected abstract void OnActiveExit();

    #endregion

    #endregion

    #region Indicator
    
    [SerializeField]
    protected IndicatorManager.Type indicatorType;
    
    public IndicatorManager.Type IndicatorType => indicatorType;
    
    [SerializeField]
    private bool _isFixedPosition = false;

    private void IndicatorInit()
    {
        SkillIndicator indicator = IndicatorManager.Instance.GetIndicator(indicatorType);
        indicator.SetSkill(this);
    }

    #endregion
}