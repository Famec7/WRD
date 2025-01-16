using UnityEngine;

public class HeavenFalling : ClickTypeSkill
{
    [Header("Offset From Target")] [SerializeField]
    private Vector2 _offset;

    public override void OnActiveEnter()
    {
        // 성검 소환
        var holySword = ProjectileManager.Instance.CreateProjectile<HolyProjectile>();

        if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
        {
            if (target is null)
                return;
            
            PivotPosition = target.transform.position;
        }


        holySword.SetData(Data);
        holySword.transform.position = (Vector3)PivotPosition + (Vector3)_offset;
        holySword.Target = PivotPosition;
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
}