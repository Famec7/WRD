using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LesserOrb : RangedWeapon
{
    private BoxCollider2D _rangeCollider;
    private float _slowDownRate = 0.5f;

    private void Awake()
    {
        _rangeCollider = GetComponent<BoxCollider2D>();
        //Todo : 스킬 데이터 받아오기
        _rangeCollider.size = new Vector2(Data.attackRange, Data.attackRange);
    }
    
    /// <summary>
    /// OnTriggerEnter2D 와 OnTriggerExit2D 를 이용하여 범위 내에 있는 몬스터에게 SlowDown 상태이상을 부여한다.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.AddStatusEffect(status,new SlowDown(other.gameObject, _slowDownRate));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(SlowDown));
        }
    }
}