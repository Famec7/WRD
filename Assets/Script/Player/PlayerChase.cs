using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChase : MonoBehaviour
{
    public bool isChase = false;
    public void Chase(GameObject target)
    {
        if(!isChase)
        {
            StartCoroutine(CoRoutineChase(target));
            isChase = true;
        }
    }
    public void StopDoing()
    {
        StopAllCoroutines();
        isChase = false;
    }

    IEnumerator CoRoutineChase(GameObject target)
    {
        while (true)
        {
            Debug.Log(System.DateTime.Now);
            Vector3 targetpos = target.transform.position;
            if (target && target.activeSelf)
            {
                if (Vector3.Distance(transform.position, targetpos) > PlayerManager.instance.range)
                {
                    PlayerManager.instance.isAttackable = false;
                    Vector3 dir = (targetpos - transform.position).normalized;
                    transform.position += dir * (PlayerManager.instance.moveSpeed * Time.deltaTime * 2);
                }
                else
                {
                    PlayerManager.instance.isAttackable = true;
                }
            }
            else
            {
                PlayerManager.instance.isAttackable = false;
                PlayerManager.instance.state = State.STAY;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
