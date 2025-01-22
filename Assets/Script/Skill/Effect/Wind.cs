using System;
using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private Action<Monster> _onWindEnd;

    private float _radius;
    private Vector3 _targetPosition;
    
    public void Init(float radius, Vector3 targetPosition, Action<Monster> onWindEnd)
    {
        _radius = radius;
        _targetPosition = targetPosition;
        _onWindEnd = onWindEnd;
    }
    
    public void Play()
    {
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
    }

    public void Stop(Monster monster)
    {
        this.gameObject.SetActive(false);
        _onWindEnd?.Invoke(monster);
    }
    
    // 몬스터의 위치를 특정 위치로 이동하는 코루틴
    private IEnumerator IE_MoveMonster(Monster monster)
    {
        while ((monster.transform.position - _targetPosition).sqrMagnitude > Mathf.Epsilon)
        {
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, _targetPosition, 0.1f);
            yield return null;
        }
        
        Stop(monster);
    }
}