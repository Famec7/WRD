using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHold : MonoBehaviour
{
    public bool isHold = false;
    public void Hold()
    {
        if(!isHold)
        {
            StartCoroutine(CoRoutineHold());
            isHold = true;
        }
    }
    public void StopDoing()
    {
        isHold = false;
        StopAllCoroutines();
    }
    IEnumerator CoRoutineHold()
    {
        while (true)
        {
            if (PlayerManager.instance.target == null)
            {
                PlayerManager.instance.isAttackable = false;
                PlayerManager.instance.FindTarget();
            }
            if (PlayerManager.instance.target && Vector3.Distance(transform.position, PlayerManager.instance.target.transform.position) > PlayerManager.instance.range)
            {
                PlayerManager.instance.target = null;
                PlayerManager.instance.weaponScript.target = null;
            }
            else if (PlayerManager.instance.target)
            {
                PlayerManager.instance.isAttackable = true;
            }
            yield return null;
        }
    }
}
