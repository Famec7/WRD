using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SkillState
{
    Enable, // 사용 불가능한 상태
    Ready, // 사용 가능한 상태
    Casting, // 스킬 사용 시작한 상태
    Active, // 스킬 사용 중인 상태
    Cooldown // 스킬 쿨타임 중인 상태
}

[System.Serializable]
public class Skill : MonoBehaviour, ISkill
{
    public ActiveSkillData data;
    public GameObject user;
    public GameObject target;
    public GameObject projectile;

    public GameObject hitEffect;

    private Dictionary<SkillState, IState<Skill>> _stateDic = new Dictionary<SkillState, IState<Skill>>();
    private FSM<Skill> _fsm;

    void Start()
    {
        IState<Skill> ready = new ReadyState();
        IState<Skill> casting = new CastingState();
        IState<Skill> active = new ActiveState();
        IState<Skill> cooldown = new CooldownState();

        _stateDic.Add(SkillState.Ready, ready);
        _stateDic.Add(SkillState.Casting, casting);
        _stateDic.Add(SkillState.Active, active);
        _stateDic.Add(SkillState.Cooldown, cooldown);

        _fsm = new FSM<Skill>(this);
    }

    private void Update()
    {
        /*_fsm.Update();*/
    }

    public void SetSkill(int skillNumber)
    {
        // 스킬 데이터를 불러오는 코드 추가 필요
    }

    public virtual void UseSkill()
    {
    }

    #region FSM

    #region Ready

    public virtual void ReadyEnter()
    {
        target = null;
        projectile = null;
        hitEffect = null;
        data.checkCooltime = data.skillCooltime;
        user = gameObject;
    }

    public virtual void ReadyUpdate()
    {
        _fsm.ChangeState(_stateDic[SkillState.Casting]);
    }

    public virtual void ReadyExit()
    {
        ;
    }

    #endregion

    #region Casting

    public virtual void CastingEnter()
    {
        ;
    }

    public virtual void CastingUpdate()
    {
        _fsm.ChangeState(_stateDic[SkillState.Active]);
    }

    public virtual void CastingExit()
    {
        ;
    }

    #endregion

    #region Active

    public virtual void ActiveEnter()
    {
        ;
    }

    public virtual void ActiveUpdate()
    {
        _fsm.ChangeState(_stateDic[SkillState.Cooldown]);
    }

    public virtual void ActiveExit()
    {
        ;
    }

    #endregion

    #region Cooldown

    public virtual void CooldownEnter()
    {
        ;
    }

    public virtual void CooldownUpdate()
    {
        StartCoroutine(WaitSkillCooltime());
    }

    public virtual void CooldownExit()
    {
        ;
    }

    #endregion

    #endregion

    
    private ContactFilter2D contactFilter = new ContactFilter2D();
    /// <summary>
    /// 탐지된 대상을 저장합니다.
    /// 탐지된 대상이 없을 경우 null을 반환합니다.
    /// <param name="center">탐지되는 중심</param>
    /// <param name="size">탐지 길이의 절반 사이즈</param>
    /// <param name="angle">탐지 각도</param>
    /// </summary>
    protected virtual void DetectTarget(Vector2 center, Vector2 size, float angle = 0f)
    {
        List<Collider2D> cols = new List<Collider2D>();
        int count = Physics2D.OverlapBox(center, size / 2, 0, contactFilter, cols);

        #region Debug
#if DEBUG
        // 상자의 모서리를 계산합니다.
        Vector2 topLeft = center + size / 2;
        Vector2 topRight = center + new Vector2(-size.x, size.y) / 2;
        Vector2 bottomLeft = center + new Vector2(size.x, -size.y) / 2;
        Vector2 bottomRight = center - size / 2;

        // 상자의 모서리를 그립니다.
        Debug.DrawLine(topLeft, topRight, Color.red);
        Debug.DrawLine(topRight, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        Debug.DrawLine(bottomLeft, topLeft, Color.red);
#endif
        #endregion

        foreach (Collider2D col in cols)
        {
            Vector2 dir = col.transform.position - this.transform.position;
            if (dir.x < 0)
            {
                target = col.gameObject;
                break;
            }
        }
    }

    private IEnumerator ShowHitEffect(float time)
    {
        GameObject hitEffectObject = Instantiate(hitEffect, target.transform.position, Quaternion.identity);
        hitEffectObject.transform.position = target.transform.position;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(hitEffectObject, 0);
    }

    protected virtual void FireProjectile(Vector3 userPos, Vector3 targetPos, bool isMulti)
    {
        //발사체 오브젝트 생성 코드 추가 필요

        if (Vector3.Distance(projectile.transform.position, targetPos) < 0.1f)
        {
            //타겟 이펙트 출력 코드 필요
            if (!isMulti && data.canUse)
                DealingOneTargetDamage();
            else
                DealingAOEDamage();
        }
    }

    protected virtual void DealingOneTargetDamage()
    {
        if (data.canUse)
        {
            target.GetComponent<Monster>().HasAttacked(data.skillDamage);
            Debug.Log("공격함" + DateTime.Now);
            StartCoroutine(ShowHitEffect(0.5f));
            data.canUse = false;
        }

        StartCoroutine(WaitSkillCooltime());
    }

    protected virtual void DealingAOEDamage()
    {
        if (data.canUse)
        {
            Vector3 target_pos = target.transform.position;
            Collider2D[] cols_in_range = Physics2D.OverlapCircleAll(target_pos, data.scopeRange);
            foreach (Collider2D col in cols_in_range)
            {
                if (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission"))
                {
                    col.GetComponent<Monster>().HasAttacked(data.skillDamage);
                }
            }

            data.canUse = false;
        }

        StartCoroutine(WaitSkillCooltime());
    }

    protected virtual IEnumerator WaitSkillCooltime()
    {
        while (data.checkCooltime > 0)
        {
            data.checkCooltime -= Time.deltaTime;
            yield return null;
        }

        data.checkCooltime = data.skillCooltime;
        data.canUse = true;
        _fsm.ChangeState(_stateDic[SkillState.Ready]);
    }

    protected virtual IEnumerator WaitSkillUptime()
    {
        while (data.checkUptime > 0)
        {
            data.checkUptime -= Time.deltaTime;
            yield return null;
        }
    }

    protected virtual IEnumerator WaitTargetSelected()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) yield break;

                Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchpos.z = -1;

                Collider2D col = Physics2D.OverlapPoint(touchpos);
                if (col && (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission")))
                {
                    target = col.gameObject;
                    yield break;
                }
                else
                {
                    // 대상 선택 잘못되었습니다 설명 UI 코드 필요
                    target = null;
                    yield break;
                }
            }

            yield return null;
        }
    }
}