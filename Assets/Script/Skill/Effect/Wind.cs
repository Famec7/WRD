using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Wind : MonoBehaviour
{
    [SerializeField] private float _duration = 2.0f;
    
    private Action<Monster> _onWindEnd;
    
    private Vector3 _targetPosition;
    private Transform _parent;
    
    public void Init(float radius, Vector3 targetPosition, Action<Monster> onWindEnd, Transform parent)
    {
        _targetPosition = targetPosition;
        _onWindEnd = onWindEnd;
        
        GetComponent<CircleCollider2D>().radius = radius * 0.5f;
    }
    
    public void Play()
    {
        this.transform.SetParent(null);
        this.transform.position = _targetPosition;
        this.gameObject.SetActive(true);
        
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
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, _targetPosition, 0.01f);
            yield return null;
        }
        
        _onWindEnd?.Invoke(monster);
    }
    
    private IEnumerator IE_WindDuration()
    {
        yield return new WaitForSeconds(_duration);
        Stop();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            StartCoroutine(IE_MoveMonster(monster));
        }
    }
}