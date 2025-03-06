using System;
using UnityEngine;

public class ShrinkingTriangleIndicator : SkillIndicator
{
    /*******************Degree*******************/
    [Header("초기 각도")]
    [SerializeField] private float _initialAngle = 60.0f;
    
    [Header("최소 각도")]
    [SerializeField] private float _minAngle = 15.0f;
    
    [Header("축소 속도(1초당 shrinkRate만큼 축소)")]
    [SerializeField] private float shrinkRate = 15.0f;

    private float _currentAngle;

    private float CurrentAngle
    {
        get => _currentAngle;
        set
        {
            _currentAngle = value;
            
            if (_currentAngle < _minAngle)
            {
                _currentAngle = _minAngle;
                OnShrinkEnd?.Invoke();
            }
            
            DrawIndicator(_currentAngle);
        }
    }

    /*******************Time*******************/
    public float ElapsedTime { get; private set; }
    
    /*******************Event*******************/
    public Action OnShrinkEnd;
    
    private void Start()
    {
        CurrentAngle = _initialAngle;
    }

    private void Update()
    {
        if (!IsSpriteRendered)
            return;
        
        if (CurrentAngle > _minAngle)
        {
            CurrentAngle -= shrinkRate * Time.deltaTime;
            ElapsedTime += Time.deltaTime;
        }
    }
    
    private void DrawIndicator(float angle)
    {
        float scale = transform.localScale.x;
        
        float scaleY = (angle / _initialAngle) * scale;
        transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
    }

    protected override void OnSpriteRendered()
    {
        base.OnSpriteRendered();
        CurrentAngle = _initialAngle;
    }
}