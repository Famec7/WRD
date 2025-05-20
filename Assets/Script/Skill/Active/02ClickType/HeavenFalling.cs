using UnityEngine;

public class HeavenFalling : ClickTypeSkill
{
    [Header("Offset From Target")] [SerializeField]
    private Vector2 _offset;
    
    [SerializeField] private AudioClip sfx;

    public override void OnActiveEnter()
    {
        SoundManager.Instance.PlaySFX(sfx);
    }

    public override bool OnActiveExecute()
    {
        // 성검 소환
        var holySword = ProjectileManager.Instance.CreateProjectile<HolyProjectile>();
        
        holySword.SetData(Data);
        holySword.SetPosition((Vector3)ClickPosition + (Vector3)_offset);
        holySword.Target = ClickPosition;
        
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
}