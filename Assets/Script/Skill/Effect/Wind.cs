﻿using System;
using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float _duration = 2.0f;
    
    private Action<Monster> _onWindEnd;

    private float _radius;
    private Vector3 _targetPosition;
    private Transform _parent;
    
    public void Init(float radius, Vector3 targetPosition, Action<Monster> onWindEnd, Transform parent)
    {
        _radius = radius;
        _targetPosition = targetPosition;
        _onWindEnd = onWindEnd;
    }
    
    public void Play()
    {
        this.transform.SetParent(null);
        this.transform.position = _targetPosition;
        this.gameObject.SetActive(true);
        
        // radius 범위 안의 적들을 중앙으로 끌어당김
        LayerMask layer = LayerMaskProvider.MonsterLayerMask;
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, _radius, default, layer);
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                StartCoroutine(IE_MoveMonster(monster));
            }
        }
        
        StartCoroutine(IE_WindDuration());
    }

    public void Stop()
    {
        this.gameObject.SetActive(false);
        this.transform.SetParent(_parent);
    }
    
    // 몬스터의 위치를 특정 위치로 이동하는 코루틴
    private IEnumerator IE_MoveMonster(Monster monster)
    {
        while ((monster.transform.position - _targetPosition).sqrMagnitude > Mathf.Epsilon)
        {
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, _targetPosition, 0.1f);
            yield return null;
        }
        
        _onWindEnd?.Invoke(monster);
    }
    
    private IEnumerator IE_WindDuration()
    {
        yield return new WaitForSeconds(_duration);
        Stop();
    }
}