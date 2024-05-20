using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStay : MonoBehaviour
{
    public bool isStay = false;
    public void Stay()
    {
        if(!isStay)
        {
            StartCoroutine(CoRoutineStay());
            isStay = true;
        }
    }
    public void StopDoing()
    {
        isStay = false;
        StopAllCoroutines();
    }
    IEnumerator CoRoutineStay()
    {
        while (true)
        {
            if (PlayerManager.instance.target == null || PlayerManager.instance.target.activeSelf == false)
            {
                PlayerManager.instance.isAttackable = false;
                PlayerManager.instance.FindTarget();
            }
            else
            {
                PlayerManager.instance.state = State.CHASE;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
