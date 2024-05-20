using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStop : MonoBehaviour
{
    public bool isStop = false;
    public void Stop()
    {
        if(!isStop)
        {
            StartCoroutine(CoRoutineStop());
            isStop = true;
        }
    }
    public void StopDoing()
    {
        isStop = false;
        StopAllCoroutines();
    }
    IEnumerator CoRoutineStop()
    {
        while (true)
        {
            PlayerManager.instance.target = null;
            PlayerManager.instance.weaponScript.target = null;
            PlayerManager.instance.isAttackable = false;
            yield return null;
        }
    }
}
