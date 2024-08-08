using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float hp;
    public float speed;
    public float armor;
    public bool isMovable;
    public bool isDead = false;
    public Status status;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HasAttacked(float damage)
    {
        hp -= damage;
        status.HP -= damage;
        if (status.HP <= 0 && !isDead)
            IsDead();
    }

    public void IsDead()
    {
        isDead = true;
        MonsterPool.instance.ReturnObject(gameObject);
    }
}
