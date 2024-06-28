using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private FSM<PlayerController> _fsm;

    private Vector3 _moveDir;
    public Vector3 TouchPos { get; private set; }

    private Camera _mainCam;

    /***********************Variables*****************************/

    #region Status

    // 이동 속도
    [Header("Move Speed")] [SerializeField]
    private float _moveSpeed = 2f;

    public float MoveSpeed
    {
        get => _moveSpeed;
        private set => _moveSpeed = value;
    }

    // 현재 무기 데이터
    [SerializeField] private WeaponBase _currentWeapon;
    public WeaponBase CurrentWeapon => _currentWeapon;

    #endregion

    #region Components

    private SpriteRenderer _spriteRenderer;

    #endregion

    #region State

    private enum State
    {
        IDLE,
        CHASE,
        MOVE,
        HOLD,
    }

    private State _currentState;

    #endregion

    #region Offset

    private const float _characterFootOffset = 0.3f;
    private const float _distanceThreshold = 0.05f;

    #endregion

    #region Target

    private GameObject _target;

    public GameObject Target
    {
        get => _target;
        set
        {
            _target = value;
            _targetUI.Target = value != null ? value.transform : null;
            ChangeState(State.CHASE);
        }
    }

    /****Target UI****/
    [Header("Target UI")] [SerializeField] private TargetUI _targetUI;

    #endregion

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _fsm = new FSM<PlayerController>(this);
        ChangeState(State.IDLE);

        _mainCam = Camera.main;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.IDLE:
                break;
            case State.CHASE:
                /*if (IsTargetNullOrInactive() || !IsTargetInRange())
                    ChangeState(State.IDLE);*/
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

    private void ChangeState(State state)
    {
#if FSM_DEBUG
        Debug.Log($"Change State : {_currentState} -> {state}");
#endif
        _currentState = state;

        switch (state)
        {
            case State.IDLE:
                _fsm.ChangeState(new IdleState());
                break;
            case State.CHASE:
                _fsm.ChangeState(new ChaseState());
                break;
            case State.MOVE:
                _fsm.ChangeState(new MoveState());
                break;
            case State.HOLD:
                _fsm.ChangeState(new HoldState());
                break;
            default:
                break;
        }
    }

    private void OnClickMove()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TouchPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            var pos = TouchPos;
            pos.z = 0f;
            TouchPos = pos;

            _moveDir = (TouchPos - transform.position).normalized;
            _spriteRenderer.flipX = !(_moveDir.x > 0);

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
        return Vector3.Distance(transform.position, Target.transform.position) < CurrentWeapon.Data.attackRange;
    }

    #endregion
}