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
    Vector3 pos;
    public State state;
    public float moveSpeed = 2f;
    public bool isAttackable;
    public GameObject weapon;
    Weapon weaponScript;
    public GameObject target = null;
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
            state = State.HOLD;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            state = State.STOP;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

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
                state = State.CHASE;
            }
            else
            {
                state = State.MOVE;
            }
        }
        switch (state)
        {
            case State.CHASE:
                Chase(target);
                break;
            case State.MOVE:
                Move(touchpos);
                break;
            case State.HOLD:
                Hold();
                break;
            case State.STAY:
                Stay();
                break;
            case State.STOP:
                Stop();
                break;
        }
    }

    void Hold()
    {
        StopAllCoroutines();
        StartCoroutine(CoRoutineHold());
    }
    IEnumerator CoRoutineHold()
    {
        while (true)
        {
            if (target == null)
            {
                isAttackable = false;
                FindTarget();
            }
            if (target && Vector3.Distance(transform.position, target.transform.position) > range)
            {
                target = null;
                weaponScript.target = null;
            }
            else if (target)
            {
                isAttackable = true;
            }
            yield return null;
        }
    }
    void Stop()
    {
        StopAllCoroutines();
        StartCoroutine(CoRoutineStop());
    }
    IEnumerator CoRoutineStop()
    {
        while (true)
        {
            target = null;
            weaponScript.target = null;
            isAttackable = false;
            yield return null;
        }
    }

    //추적: 타겟 존재 & 액티브 상태일 때 사거리 밖이면 이동 후 공격
    //타겟이 사라지면 Stay상태로 전환
    void Chase(GameObject target)
    {
        StopAllCoroutines();
        StartCoroutine(CoRoutineChase(target));
    }

    IEnumerator CoRoutineChase(GameObject target)
    {
        Vector3 targetpos = target.transform.position;
        while (true)
        {
            if (target && target.activeSelf)
            {
                if (Vector3.Distance(transform.position, targetpos) > range)
                {
                    isAttackable = false;
                    Vector3 dir = (targetpos - transform.position).normalized;
                    transform.position += dir * (moveSpeed * Time.deltaTime * 2);
                }
                else
                {
                    isAttackable = true;
                }
            }
            else
            {
                isAttackable = false;
                state = State.STAY;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    //이동, 매개변수 두번째는 targetpos와 거리가 distance가 될 때까지 이동한다는 의미
    void Move(Vector3 touchpos)
    {
        StopAllCoroutines();
        StartCoroutine(CoRoutineMove(touchpos, 0.05f));
    }
    IEnumerator CoRoutineMove(Vector3 targetpos, float distance)
    {
        Vector3 des = targetpos;
        while (true)
        {
            if (Vector3.Distance(transform.position, des) > distance)
            {
                Vector3 dir = (des - transform.position).normalized;
                transform.position += dir * (moveSpeed * Time.deltaTime);
            }
            else
            {
                state = State.STAY;
                break;
            }
            yield return null;
        }
    }

    //Stop: 그냥 가만히서 타겟 찾다 찾으면 체이스 상태로 전환
    void Stay()
    {
        StopAllCoroutines();
        StartCoroutine(CoRoutineStay());
    }
    IEnumerator CoRoutineStay()
    {
        while (true)
        {
            if (target == null || target.activeSelf == false)
            {
                isAttackable = false;
                FindTarget();
            }
            else
            {
                state = State.CHASE;
                Chase(target);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    //타겟 찾기: 원 범위 안에서 적 발견하면 걔가 타겟됨, 발견 못할시 target은 null인 상태 그대로
    void FindTarget()
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
}
