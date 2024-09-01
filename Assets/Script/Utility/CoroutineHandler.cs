using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : Singleton<CoroutineHandler>
{
    private readonly HashSet<Coroutine> _activeCoroutines = new HashSet<Coroutine>();
    
    protected override void Init()
    {
        ;
    }

    public new Coroutine StartCoroutine(IEnumerator routine)
    {
        if(routine == null)
        {
            return null;
        }
        
        Coroutine coroutine = base.StartCoroutine(routine);
        
        if(coroutine != null)
            _activeCoroutines.Add(coroutine);
        
        return coroutine;
    }

    public new void StopCoroutine(Coroutine routine)
    {
        if (routine == null)
        {
            return;
        }
        
        if(_activeCoroutines.Contains(routine))
        {
            base.StopCoroutine(routine);
            _activeCoroutines.Remove(routine);
        }
    }
    
    public new void StopAllCoroutines()
    {
        foreach (var coroutine in _activeCoroutines)
        {
            base.StopCoroutine(coroutine);
        }
        
        _activeCoroutines.Clear();
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