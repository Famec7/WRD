using UnityEngine;

public class HeavenFalling : ClickTypeSkill
{
    [Header("Offset From Target")] [SerializeField]
    private Vector2 _offset;

    public override void OnActiveEnter()
    {
        ;
    }

    public override bool OnActiveExecute()
    {
        // 성검 소환
        var holySword = ProjectileManager.Instance.CreateProjectile<HolyProjectile>();
        
        holySword.SetData(Data);
        holySword.transform.position = (Vector3)ClickPosition + (Vector3)_offset;
        holySword.Target = ClickPosition;
        
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
}