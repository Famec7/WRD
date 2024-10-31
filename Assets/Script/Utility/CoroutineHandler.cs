using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : Singleton<CoroutineHandler>
{
    protected override void Init()
    {
        ;
    }

    public new Coroutine StartCoroutine(IEnumerator routine)
    {
        if (routine == null)
        {
            return null;
        }

        Coroutine coroutine = base.StartCoroutine(routine);

        return coroutine;
    }

    public new void StopCoroutine(Coroutine routine)
    {
        if (routine == null)
        {
            return;
        }

        base.StopCoroutine(routine);
    }

    public new void StopAllCoroutines()
    {
        base.StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}