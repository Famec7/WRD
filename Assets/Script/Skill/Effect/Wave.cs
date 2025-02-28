using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private float _pushForce;
    private Vector2 _moveDirection;
    private float _moveSpeed;
    private float _damage;
    
    private Coroutine _moveCoroutine;
    public Action OnWaveEnd;

    private HashSet<Monster> _damagedMonsters = new();

    public void Init(float pushForce, Vector2 moveDirection, float moveSpeed, float damage)
    {
        _pushForce = pushForce;
        _moveDirection = moveDirection;
        _moveSpeed = moveSpeed;
        _damage = damage;
    }

    public void PlayEffect()
    {
        this.gameObject.SetActive(true);
        _moveCoroutine = StartCoroutine(IE_MoveWave());
    }
    
    public void StopEffect()
    {
        OnWaveEnd?.Invoke();
        this.gameObject.SetActive(false);
        StopCoroutine(_moveCoroutine);
    }

    private IEnumerator IE_MoveWave()
    {
        while (true)
        {
            transform.position += (Vector3)(_moveDirection * (_moveSpeed * Time.deltaTime));
            yield return null;
        }
        
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndPoint"))
        {
            StopEffect();
        }
        
        if (other.TryGetComponent(out Monster monster))
        {
            monster.transform.position += (Vector3)(_moveDirection * _pushForce * Time.deltaTime);
            
            if (!_damagedMonsters.Contains(monster))
            {
                monster.HasAttacked(_damage);
                _damagedMonsters.Add(monster);
            }
        }
    }
}