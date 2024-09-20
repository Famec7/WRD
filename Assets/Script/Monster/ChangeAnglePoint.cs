  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnglePoint : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 monsterMoveDir { get; set; }
    public int pointNum;
    
    void Start()
    {
        if (pointNum == 1)
            monsterMoveDir = new Vector2(0, -1);

        if (pointNum == 2)
            monsterMoveDir = new Vector2(1, 0);

        if (pointNum == 3)
            monsterMoveDir = new Vector2(0, 1);

        if (pointNum == 4)
            monsterMoveDir = new Vector2(-1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MonsterCenter"))
        {
            collision.transform.parent.gameObject.GetComponent<MonsterMoveComponent>().moveDir = monsterMoveDir;
            collision.transform.parent.gameObject.GetComponent<MonsterMoveComponent>().roadNum = pointNum;
            collision.transform.parent.gameObject.GetComponent<MonsterMoveComponent>().returnPos = transform.position;
            Vector3 collisionScale = collision.transform.parent.localScale;
            if (monsterMoveDir.x < 0 || monsterMoveDir.y < 0)
                collision.transform.parent.localScale = new Vector3(-1* collisionScale.x, 1* collisionScale.y, 1* collisionScale.z);
            else
                collision.transform.parent.localScale = new Vector3(1* collisionScale.x, 1* collisionScale.y, 1* collisionScale.z);

            
        }
    }
}
