using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponNumber;
    public List<Skill> skillList;
    public float range;
    public float damage;
    public float attackSpeed;
    public bool isAttackable;
    public bool hasAttacked;
    public GameObject target;
    public string type;
    public AutoAttack autoAttack;

    public void SetWeapon(int range, int damage, int attack_speed)
    {
        this.range = range;
        this.damage = damage;
        this.attackSpeed = attack_speed;
    }

    void Awake()
    {
        autoAttack = GetComponent<AutoAttack>();
        hasAttacked = false;
    }

    public void InitSkill(int weaponCode) // 실제 코드에서는 csv파일에서 다 가져옴
    {
        if (weaponCode == 0)
        {
            autoAttack.data.skillNumber = 101;
            autoAttack.data.skillType = SkillType.BASIC;
            autoAttack.data.skillDamage = 4;
            autoAttack.data.skillRange = 2;
            autoAttack.data.skillCooltime = 1;
            autoAttack.data.canUse = true;
        }
        else if (weaponCode == 1)
        {
            autoAttack.data.skillNumber = 103;
            autoAttack.data.skillType = SkillType.BASIC;
            autoAttack.data.skillDamage = 2;
            autoAttack.data.skillRange = 2;
            autoAttack.data.skillCooltime = 1;
            autoAttack.data.canUse = true;
        }
    }

    public void AddToSkillList()
    {
        if (skillList.Count != 0)
            return;
        skillList.Add(autoAttack); // 임시
    }

    void Update()
    {

        Attack();
    }

    /// <summary>
    /// 기본 공격 & 패시브 스킬 사용
    /// 기본 공격시 발동하는 스킬
    /// </summary>
    protected virtual void Attack()
    {
        if (isAttackable && !hasAttacked)
            StartCoroutine(CoroutineAttack());
    }

    private ContactFilter2D contactFilter = new ContactFilter2D();
    protected virtual void DetectTarget(Vector2 center, Vector2 size, float angle = 0f)
    {
        List<Collider2D> cols = new List<Collider2D>();
        int count = Physics2D.OverlapBox(center, size / 2, 0, contactFilter, cols);
        
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

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    protected virtual IEnumerator CoroutineAttack()
    {
        if (!hasAttacked && target)
        {
            foreach (Skill s in skillList)
            {
                if (s.data.skillType == SkillType.BASIC || s.data.skillType == SkillType.PASSIVE)
                {
                    s.target = this.target;
                    s.UseSkill();
                }

                if (s.data.skillType == SkillType.BASIC)
                    attackSpeed = s.data.skillCooltime;
            }

            hasAttacked = true;
            yield return new WaitForSeconds(1f / attackSpeed);
            hasAttacked = false;
        }
    }
}