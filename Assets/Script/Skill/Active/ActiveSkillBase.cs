using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ActiveSkillBase : SkillBase
{
    #region State

    private FSM<ActiveSkillBase> _fsm;

    public enum ActiveStateType
    {
        Preparing,
        Casting,
        CoolTime,
        Active,
    }

    #endregion

    #region Data

    private float _currentCoolTime;
    public float CurrentCoolTime => _currentCoolTime;

    public ActiveSkillData Data { get; private set; }

    private void DataInit()
    {
        Data = SkillManager.Instance.GetActiveSkillData(GetType().Name);
    }

    #endregion

    protected List<Monster> targetMonsters = new List<Monster>();
    protected Vector2 pivotPosition = default;

    protected override void Init()
    {
        base.Init();
        DataInit();
        StateInit();
        IndicatorInit();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _fsm.Update();
    }

    public abstract void UseSkill();

    public void CancelSkill()
    {
        ChangeState(new CoolTimeState());
    }

    public void AddTargetMonster(Monster monster)
    {
        targetMonsters.Add(monster);
    }

    protected Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    /***************************State Method***************************/

    #region State Method

    protected virtual void StateInit()
    {
        _fsm = new FSM<ActiveSkillBase>(this);

        ChangeState(new CoolTimeState());
    }

    protected void ChangeState(IState<ActiveSkillBase> state)
    {
        _fsm.ChangeState(state);
    }

    # region CoolTime State

    public virtual void OnCoolTimeEnter()
    {
        SkillUIManager.Instance.ClosePopupPanel();
    }

    public virtual void OnCoolTimeExecute()
    {
        _currentCoolTime -= Time.deltaTime;

        if (_currentCoolTime <= 0)
        {
            _currentCoolTime = Data.CoolTime;

            if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
            {
                SkillUIManager.Instance.ShowPopupPanel((int)ActiveStateType.Active);
                ChangeState(new ActiveState());
            }
        }
    }

    public virtual void OnCoolTimeExit()
    {
        ;
    }

    # endregion

    #region Preparing

    private float _preparingTime;

    public virtual void OnPreparingEnter()
    {
        SkillUIManager.Instance.ShowPopupPanel((int)ActiveStateType.Preparing);
        
        _preparingTime = 3f;
        pivotPosition = default;

        ShowUsableRange();
    }

    public virtual void OnPreparingExecute()
    {
        // 준비 시간 3초 이상 넘기면 취소
        _preparingTime -= Time.deltaTime;
        if (_preparingTime <= 0f)
        {
            CancelSkill();
        }

        if (Input.GetMouseButtonUp(0))
        {
            // UI 터치 시 스킬 취소
            if (EventSystem.current.IsPointerOverGameObject())
            {
                CancelSkill();
            }

            pivotPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);

            float distanceFromPlayerToPivot = Vector2.Distance(owner.transform.position, pivotPosition);

            // 스킬 범위 벗어나면 취소
            if (distanceFromPlayerToPivot > Data.AvailableRange)
            {
                CancelSkill();
            }
            else
            {
                ChangeState(new CastingState());
            }
        }
    }

    public virtual void OnPreparingExit()
    {
        HideUsableRange();
        SkillUIManager.Instance.NextPhase();
    }

    #endregion

    #region Casting State

    private float _castingTime = 3f;

    public virtual void OnCastingEnter()
    {
        SkillUIManager.Instance.ShowPopupPanel((int)ActiveStateType.Casting);
        
        _castingTime = 3f;

        indicator.ShowIndicator(pivotPosition);
        targetMonsters.Clear();
    }

    public virtual void OnCastingExecute()
    {
        // 캐스팅 시간 3초이상 넘기면 취소
        _castingTime -= Time.deltaTime;
        if (_castingTime <= 0f)
        {
            ChangeState(new CoolTimeState());
        }

        if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
        {
            ChangeState(new CoolTimeState());
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                // UI 터치 시 스킬 취소
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    ChangeState(new CoolTimeState());
                }

                ChangeState(new ActiveState());
            }
        }
    }

    public virtual void OnCastingExit()
    {
        indicator.HideIndicator();
        SkillUIManager.Instance.NextPhase();
    }

    #endregion

    # region Active State

    public abstract void OnActiveEnter();
    public abstract void OnActiveExecute();
    public abstract void OnActiveExit();

    # endregion

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