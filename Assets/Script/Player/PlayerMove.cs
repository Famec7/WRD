using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float characterFootOffset = 0.3f;
    public bool isMove = false;
    public void Move(Vector3 touchpos)
    {
        if(!isMove)
        {
            StartCoroutine(CoRoutineMove(touchpos, 0.05f));
            isMove = true;
        }

    }
    public void StopDoing()
    {
        isMove = false;
        StopAllCoroutines();
    }
    IEnumerator CoRoutineMove(Vector3 targetpos, float distance)
    {
        Vector3 des = targetpos;
        des.y += characterFootOffset;
        while (true)
        {
            if (Vector3.Distance(transform.position, des) > distance)
            {
                Vector3 dir = (des - transform.position).normalized;
                transform.position += dir * (PlayerManager.instance.moveSpeed * Time.deltaTime);
            }
            else
            {
                PlayerManager.instance.state = State.STAY;
                isMove = false;
                break;
            }
            yield return null;
        }
    }

}
