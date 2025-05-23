﻿using UnityEngine;

public class TideBlow : ClickTypeSkill
{
    [Header("스킬 관련 오브젝트")]
    [SerializeField] private Wave _wave;
    [SerializeField] private WaterPool _waterPool;

    [Header("스킬 관련 변수")]
    [SerializeField] private float _pushForce = 10.0f;
    [SerializeField] private float _moveSpeed = 3.0f;
    
    [Header("스킬 관련 사운드")]
    [SerializeField] private AudioClip sfx;
    
    public override void OnActiveEnter()
    {
        _wave.transform.SetParent(null);
        _waterPool.transform.SetParent(null);
        
        // Wave
        Vector2 direction = (ClickPosition - (Vector2)transform.position).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;
            _wave.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            direction.x = 0;
            _wave.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        _wave.Init(_pushForce, direction, _moveSpeed, Data.GetValue(0));
        _wave.transform.position = transform.position;
        _wave.PlayEffect();
        
        SoundManager.Instance.PlaySFX(sfx);
        
        _wave.OnWaveEnd = CreateWaterPool;
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }

    private void CreateWaterPool()
    {
        _waterPool.transform.position = _wave.transform.position;
        _waterPool.Init(Data.GetValue(2), Data.GetValue(3), Data.GetValue(1));
        _waterPool.PlayEffect();
    }
}