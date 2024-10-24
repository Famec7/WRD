﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerController : CharacterController, ISubject
{
    /***********************Variables*****************************/
    private FSM<PlayerController> _fsm;

    public Vector3 TouchPos { get; private set; }

    [SerializeField] private Transform _arm;

    #region State

    public enum State
    {
        IDLE,
        CHASE,
        MOVE,
        HOLD,
    }

    public State CurrentState { get; private set; }

    private readonly IdleState _idleState = new();
    private readonly ChaseState _chaseState = new();
    private readonly MoveState _moveState = new();
    private readonly HoldState _holdState = new();

    #endregion

    #region Offset

    private const float _characterFootOffset = 0.3f;
    private const float _distanceThreshold = 0.01f;

    #endregion

    #region Target

    public override GameObject Target
    {
        get => base.Target;
        set
        {
            base.Target = value;

            if (value == null || !value.activeSelf)
            {
                ChangeState(State.IDLE);
                return;
            }

            // 상태를 추적으로 변경후 옵저버에게 알림
            ChangeState(State.CHASE);
            
            Vector3 targetDir = value.transform.position - transform.position;
            SetFlip(targetDir.x > 0);
            
            Notify();
        }
    }

    #endregion

    /***********************Method*****************************/
    private void Start()
    {
        _fsm = new FSM<PlayerController>(this);
        ChangeState(State.IDLE);
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case State.IDLE:
                break;
            case State.CHASE:
                if (!IsWeaponEquipped() || IsTargetNullOrInactive())
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
        if (!Input.GetMouseButtonDown(0) || UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("UI")))
        {
            return;
        }

        TouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var pos = TouchPos;
        pos.z = 0f;
        TouchPos = pos;

        MoveDir = (TouchPos - transform.position).normalized;

        LayerMask layerMask = LayerMaskManager.Instance.MonsterLayerMask;
        Collider2D col = Physics2D.OverlapPoint( TouchPos, layerMask);
        // 몬스터 클릭 시 추적
        if (col != null)
        {
            Target = col.gameObject;
        }
        else
        {
            Target = null;
            ChangeState(State.MOVE);
        }
    }

    public void ChangeState(State state)
    {
#if FSM_DEBUG
        Debug.Log($"Change State : {CurrentState} -> {state}");
#endif
        if(CurrentState == state)
            return;
        
        CurrentState = state;
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
        
        Notify();
    }

    /******************************bool Method******************************************/

    #region bool Method

    private bool IsPlayerAtTouchPos()
    {
        // 캐릭터 발 위치를 고려하여 터치 위치와 거리를 계산
        var pos = TouchPos;
        pos.y -= _characterFootOffset;

        return Vector3.Distance(transform.position, TouchPos) < _distanceThreshold;
    }

    public bool IsTargetInRange()
    {
        if(IsTargetNullOrInactive())
            return false;
        
        return Vector3.Distance(transform.position, Target.transform.position) <= Data.CurrentWeapon.Data.AttackRange;
    }
    
    public bool IsWeaponEquipped()
    {
        return Data.CurrentWeapon != null;
    }

    #endregion

    /******************************Observer******************************************/
    #region Observer

    private readonly List<IObserver> _observers = new();

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
        else
            Debug.LogError($"{observer} is not exist in {_observers}");
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.OnNotify();
        }
    }

    #endregion
    
    /******************************Weapon******************************************/
    public override void AttachWeapon(WeaponBase weapon)
    {
        Data.SetCurrentWeapon(weapon);
        
        weapon.transform.position = transform.position + weapon.Pivot.GetPivot();
        
        weapon.transform.SetParent(_arm);

        Vector3 playerRotation = transform.rotation.eulerAngles;
        
        _arm.rotation = Quaternion.Euler(0, playerRotation.y == -180 ? 180 : 0, 0);
    }
    
    public override void DetachWeapon()
    {
        Target = null;
        Data.CurrentWeapon.transform.SetParent(null);
        Data.SetCurrentWeapon(null);
        
        _arm.localRotation = Quaternion.Euler(0, 0, 0);
    }
}