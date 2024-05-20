using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum State
{
    STAY,
    CHASE,
    MOVE,
    HOLD,
    STOP
}
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    Vector3 pos;
    public State state;
    public float moveSpeed = 2f;
    public bool isAttackable;
    public GameObject weapon;
    public Weapon weaponScript;
    public PlayerChase chaseScript;
    public PlayerHold holdScript;
    public PlayerMove moveScript;
    public PlayerStay stayScript;
    public PlayerStop stopScript;
    public GameObject target = null;
    public Vector3 DirToTarget;
    public GameObject highlight = null;
    private Vector3 touchpos;
    public float range;
    public bool isWeaponStateChanged;
    Collider2D[] colsInRange;
    Vector3 moveDir;
    void Start()
    {
        pos = transform.position;
        state = State.STAY;
        weapon = GameObject.Find("weapon");
        weaponScript = weapon.GetComponent<Weapon>();
        chaseScript = GetComponent<PlayerChase>();
        holdScript = GetComponent<PlayerHold>();
        moveScript = GetComponent<PlayerMove>();
        stayScript = GetComponent<PlayerStay>();
        stopScript = GetComponent<PlayerStop>();
        highlight = GameObject.Find("highlight");
    }
    void Update()
    {
        if(weaponScript.hasAttacked == true)
        {
            isWeaponStateChanged = true;
        }
        if (isAttackable)
            weaponScript.isAttackable = true;
        else
            weaponScript.isAttackable = false;
        if (target)
        {
            DirToTarget = (target.transform.position - transform.position).normalized;
            if ((target.transform.position.x > transform.position.x) && isWeaponStateChanged)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                isWeaponStateChanged = false;
            }
            else if((target.transform.position.x <= transform.position.x) && isWeaponStateChanged)
            {
                transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                isWeaponStateChanged = false;
            }
            highlight.SetActive(true);
            highlight.transform.position = target.transform.position;
        }
        else
        {
            highlight.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            weaponScript.InitSkill(0);
            weaponScript.AddToSkillList();
            foreach(Skill s in weaponScript.skillList)
            {
                if(s.data.skillType == SkillType.ACTIVE)
                {
                    range = s.data.skillRange;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            StopCurrentDo();
            state = State.HOLD;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopCurrentDo();
            state = State.STOP;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchpos.z = -1;
            target = null;
            isAttackable = false;
            moveDir = (touchpos - transform.position).normalized;
            if (moveDir.x > 0)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            target = null;
            Collider2D col = Physics2D.OverlapPoint(touchpos);
            if (col && (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission")))
            {
                target = col.gameObject;
                StopCurrentDo();
                state = State.CHASE;
            }
            else
            {
                StopCurrentDo();
                state = State.MOVE;
            }
        }
        switch (state)
        {
            case State.CHASE:
                chaseScript.Chase(target);
                break;
            case State.MOVE:
                moveScript.Move(touchpos);
                break;
            case State.HOLD:
                holdScript.Hold();
                break;
            case State.STAY:
                stayScript.Stay();
                break;
            case State.STOP:
                stopScript.Stop();
                break;
        }
    }
    //타겟 찾기: 원 범위 안에서 적 발견하면 걔가 타겟됨, 발견 못할시 target은 null인 상태 그대로
    public void FindTarget()
    {
        float shortestDistant = 10000;
        GameObject shortestTarget = null;
        colsInRange = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D col in colsInRange)
        {
            if (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission"))
            {
                if (Vector3.Distance(col.transform.position, gameObject.transform.position) < shortestDistant)
                {
                    shortestDistant = Vector3.Distance(col.transform.position, gameObject.transform.position);
                    shortestTarget = col.gameObject;
                }
            }
        }
        weaponScript.target = shortestTarget;
        target = shortestTarget;
    }
    void StopCurrentDo()
    {
        switch(state)
        {
            case State.STAY:
                stayScript.StopDoing();
                break;
            case State.STOP:
                stopScript.StopDoing();
                break;
            case State.CHASE:
                chaseScript.StopDoing();
                break;
            case State.MOVE:
                moveScript.StopDoing();
                break;
            case State.HOLD:
                holdScript.StopDoing();
                break;
        }
    }
}
