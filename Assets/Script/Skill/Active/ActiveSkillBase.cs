using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class ActiveSkillBase : SkillBase
{
    #region Data
    
    public ActiveSkillData Data { get; private set; }

    private void DataInit()
    {
        Data = SkillManager.Instance.GetActiveSkillData(GetType().Name);
        CurrentCoolTime = Data.CoolTime;
    }

    #endregion

    protected List<Monster> targetMonsters = new List<Monster>();
    protected Vector2 pivotPosition = default;

    protected override void Init()
    {
        base.Init();
        DataInit();
        BTInit();
        IndicatorInit();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _btRunner.Operator();
    }

    public abstract void UseSkill();

    public void CancelSkill()
    {
        IsCoolTime = true;
    }

    public void AddTargetMonster(Monster monster)
    {
        targetMonsters.Add(monster);
    }

    /***************************Behaviour Tree***************************/

    #region Behaviour Tree
    
    private BehaviourTreeRunner _btRunner;

    private void BTInit()
    {
        _btRunner = new BehaviourTreeRunner(SettingBT());
    }

    protected abstract INode SettingBT();

    #region CoolTime Node

    protected virtual List<INode> CoolTimeNodes()
    {
        return new List<INode>
        {
            new ActionNode(CheckCoolTimeState),
            new ActionNode(CheckCoolTime),
        };
    }

    private bool _isCoolTime = true;

    public bool IsCoolTime
    {
        set
        {
            _isCoolTime = value;
            
            if (value is true)
            {
                SkillUIManager.Instance.ClosePopupPanel();
            }
        }
        get => _isCoolTime;
    }
    
    private float _currentCoolTime;

    public float CurrentCoolTime
    {
        get => _currentCoolTime;
        set
        {
            _currentCoolTime = value;
            if (value <= 0)
            {
                IsCoolTime = false;
                _currentCoolTime = Data.CoolTime;
                
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

    private INode.ENodeState CheckCoolTime()
    {
        CurrentCoolTime -= Time.deltaTime;

        return CurrentCoolTime <= 0 ? INode.ENodeState.Failure : INode.ENodeState.Running;
    }

    #endregion
    
    #region Indicator Node
    protected float preparingTime;

    protected virtual List<INode> IndicatorNodes()
    {
        return new List<INode>
        {
            new ActionNode(CheckIndicatorState),
            new ActionNode(TouchIndicator),
        };
    }

    private bool _isIndicatorState = false;

    protected bool IsIndicatorState
    {
        set
        {
            _isIndicatorState = value;

            if (_isIndicatorState is false)
            {
                indicator.HideIndicator();
            }
            else
            {
                preparingTime = 3f;

                indicator.ShowIndicator(pivotPosition);
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

        if (Input.GetMouseButtonUp(0))
        {
            // UI 터치 시 스킬 취소
            if (EventSystem.current.IsPointerOverGameObject())
            {
                CancelSkill();
                return INode.ENodeState.Failure;
            }

            IsIndicatorState = false;
            IsActive = true;

            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Running;
    }

    #endregion

    #region Active Nodes

    protected virtual List<INode> ActiveNodes()
    {
        return new List<INode>
        {
            new ActionNode(CheckActiveState),
            new ActionNode(OnActiveEnter),
            new ActionNode(OnActiveExecute),
            new ActionNode(OnActiveExit),
        };
    }

    private bool _isActive = false;
    protected bool IsActive
    {
        set
        {
            _isActive = value;
            
            if(_isActive is true)
            {
                SkillUIManager.Instance.ShowPopupPanel(3);
            }
            else
            {
                IsCoolTime = true;
            }
        }
        get => _isActive;
    }
    
    private INode.ENodeState CheckActiveState()
    {
        return IsActive is true ? INode.ENodeState.Success : INode.ENodeState.Failure;
    }

    protected abstract INode.ENodeState OnActiveEnter();
    protected abstract INode.ENodeState OnActiveExecute();
    protected abstract INode.ENodeState OnActiveExit();

    #endregion

    #endregion

    #region Indicator

    // 실제 스킬 사용 범위
    [SerializeField] protected SkillIndicator indicator;

    // 스킬 사용 가능한 범위 표시하는 오브젝트
    [SerializeField] protected GameObject usableRange;

    private void IndicatorInit()
    {
        if (indicator != null)
        {
            indicator.gameObject.SetActive(false);
            indicator.transform.localScale = new Vector3(Data.Range, Data.Range, 0);
            indicator.SetSkill(this);
        }

        if (usableRange != null)
        {
            usableRange.SetActive(false);
            usableRange.transform.localScale = new Vector3(Data.AvailableRange, Data.AvailableRange, 0);
        }
    }

    public void ShowUsableRange(Vector2 position = default, Quaternion rotation = default)
    {
        if (position == default)
        {
            usableRange.transform.position = owner.transform.position;
        }
        else
        {
            usableRange.transform.position = position;
        }

        usableRange.SetActive(true);
    }

    public void HideUsableRange()
    {
        usableRange.SetActive(false);
    }

    #endregion
}