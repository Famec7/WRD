using UnityEngine;

public class HeavenFalling : ClickTypeSkill
{
    [Header("Offset From Target")]
    [SerializeField]
    private Vector2 _offset;
    
    protected override void OnActiveEnter()
    {
        // 성검 소환
        var holySword = ProjectileManager.Instance.CreateProjectile<HolyProjectile>();

        if (pivotPosition == Vector2.zero)
        {
            // 스킬 범위 안에 적이 있으면 타겟을 적으로 설정
            var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Data.Range, default, targetLayer);
            
            if (targets.Count > 0)
            {
                pivotPosition = targets[0].transform.position;
            }
            else
            {
                IsActive = false;
                return;
            }
        }

        holySword.SetData(Data);
        holySword.transform.position = (Vector3)pivotPosition + (Vector3)_offset;
        Debug.Log(holySword.transform.position);
        holySword.Target = pivotPosition;
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {
        ;
    }
}