using System;
using System.Collections;
using UnityEngine;

public class CoroutineHandler : Singleton<CoroutineHandler>
{
    protected override void Init()
    {
    }

    public new Coroutine StartCoroutine(IEnumerator routine)
    {
        return base.StartCoroutine(routine);
    }

    public new void StopCoroutine(Coroutine routine)
    {
        base.StopCoroutine(routine);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}