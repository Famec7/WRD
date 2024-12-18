using UnityEngine;

public class HeavenFalling : ClickTypeSkill
{
    [Header("Offset From Target")] [SerializeField]
    private Vector2 _offset;

    protected override void OnActiveEnter()
    {
        // 성검 소환
        var holySword = ProjectileManager.Instance.CreateProjectile<HolyProjectile>();

        if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
        {
            if (target is null)
                return;
            
            pivotPosition = target.transform.position;
        }


        holySword.SetData(Data);
        holySword.transform.position = (Vector3)pivotPosition + (Vector3)_offset;
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