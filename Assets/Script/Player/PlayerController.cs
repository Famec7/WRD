using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerController : CharacterController
{
    /***********************Variables*****************************/
    private FSM<PlayerController> _fsm;

    public Vector3 TouchPos { get; private set; }

    private Camera _mainCam;

    #region State

    protected enum State
    {
        IDLE,
        CHASE,
        MOVE,
        HOLD,
    }

    protected State currentState;

    private readonly IdleState _idleState = new();
    private readonly ChaseState _chaseState = new();
    private readonly MoveState _moveState = new();
    private readonly HoldState _holdState = new();

    #endregion

    #region Offset

    private const float _characterFootOffset = 0.3f;
    private const float _distanceThreshold = 0.05f;

    #endregion

    #region Target

    public override GameObject Target
    {
        get => base.Target;
        set
        {
            base.Target = value;

            if(value == null || !value.activeSelf)
                return;
            
            ChangeState(State.CHASE);
            _targetUI.Target = value.transform;
        }
    }

    [Header("Target UI")] [SerializeField] private TargetUI _targetUI;

    #endregion

    /***********************Method*****************************/
    private void Start()
    {
        _fsm = new FSM<PlayerController>(this);
        ChangeState(State.IDLE);

        _mainCam = Camera.main;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                break;
            case State.CHASE:
                if (IsTargetNullOrInactive())
                    ChangeState(State.IDLE);
                break;
            case State.MOVE:
                if (IsPlayerAtTouchPos())
                    ChangeState(State.IDLE);
                break;
            case State.HOLD:
                break;
            default:
                break;
        }

        _fsm.Update();
        OnClickMove();
    }


    private void OnClickMove()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TouchPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            var pos = TouchPos;
            pos.z = 0f;
            TouchPos = pos;

            Data.MoveDir = (TouchPos - transform.position).normalized;

            Collider2D col = Physics2D.OverlapPoint(TouchPos);
            // 몬스터 클릭 시 추적
            if (col && (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission")))
            {
                Target = col.gameObject;
            }
            else
            {
                Target = null;
                ChangeState(State.MOVE);
            }
        }
    }

    private void ChangeState(State state)
    {
#if FSM_DEBUG
        Debug.Log($"Change State : {_currentState} -> {state}");
#endif
        switch (state)
        {
            case State.IDLE:
                _fsm.ChangeState(_idleState);
                break;
            case State.CHASE:
                _fsm.ChangeState(_chaseState);
                break;
            case State.MOVE:
                _fsm.ChangeState(_moveState);
                break;
            case State.HOLD:
                _fsm.ChangeState(_holdState);
                break;
            default:
                Debug.LogError($"{state} is not exist in {nameof(State)}");
                break;
        }
    }

    /******************************bool Method******************************************/

    #region bool Method

    private bool IsPlayerAtTouchPos()
    {
        // 캐릭터 발 위치를 고려하여 터치 위치와 거리를 계산
        var pos = TouchPos;
        pos.y += _characterFootOffset;

        return Vector3.Distance(transform.position, TouchPos) < _distanceThreshold;
    }

    private bool IsTargetNullOrInactive()
    {
        return Target == null || !Target.activeSelf;
    }

    private bool IsTargetInRange()
    {
        return Vector3.Distance(transform.position, Target.transform.position) < Data.CurrentWeapon.Data.attackRange;
    }

    #endregion
}