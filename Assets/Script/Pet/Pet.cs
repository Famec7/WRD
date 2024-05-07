using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public GameObject target;
    public GameObject weapon;
    public Weapon weaponScript;
    public PetManager pm;
    public bool hasWeaponAttacked;
    public bool isWeaponStateChanged;
    public float range;
    // Start is called before the first frame update
    void Awake()
    {
        weapon = transform.GetChild(1).gameObject;
        weaponScript = weapon.GetComponent<Weapon>();
        pm = GameObject.Find("PetManager").GetComponent<PetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponScript.hasAttacked == true)
        {
            isWeaponStateChanged = true;
        }
        if (target == null || target.activeSelf == false)
            FindTarget();
        if (target)
        {
            weaponScript.SetTarget(target);
            if (target.transform.position.x > transform.position.x && isWeaponStateChanged)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                isWeaponStateChanged = false;
            }
            else if (target.transform.position.x <= transform.position.x && isWeaponStateChanged)
            {
                transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                isWeaponStateChanged = false;
            }
        }
        else
        {
            weaponScript.SetTarget(null);
        }
        if (pm.GetComponent<PetManager>().isAttackable && target && Vector3.Distance(target.transform.position, transform.position) < range)
            weaponScript.isAttackable = true;
        else    
            weaponScript.isAttackable = false;


        if(Input.GetKeyDown(KeyCode.W))
        {
            weaponScript.InitSkill(1);
            weaponScript.AddToSkillList();
        }
    }

    void FindTarget()
    {
        if(pm.GetComponent<PetManager>().playerState == State.STAY && target)
            return;
        float shortestDistant = 10000;
        GameObject shortestTarget = null;
        Collider2D[] colsInRange = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D col in colsInRange)
        {
            if (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission"))
            {
                if (Vector3.Distance(col.transform.position, transform.position) < shortestDistant)
                {
                    shortestDistant = Vector3.Distance(col.transform.position, transform.position);
                    shortestTarget = col.gameObject;
                }
            }
        }
        weaponScript.SetTarget(shortestTarget);
        target = shortestTarget;
    }

}
