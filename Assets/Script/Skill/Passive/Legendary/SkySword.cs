using System.Collections;
using UnityEngine;

public class SkySword : PassiveSkillBase
{
    private WaitForSeconds _delay;
    
    [SerializeField] private AudioClip sfx;

    protected override void Init()
    {
        base.Init();
        _delay = new WaitForSeconds(Data.GetValue(1));
    }

    public override bool Activate(GameObject target = null)
    {
        if (CheckTrigger())
        {
            SoundManager.Instance.PlaySFX(sfx);
            
            var manager = ProjectileManager.Instance;
            var projectile = manager.CreateProjectile<SkyProjectile>("SkySword");

            if (projectile is null)
            {
#if UNITY_EDITOR
                Debug.LogError("FallingSwordProjectile is not enough");
#endif
                return false;
            }
            
            projectile.SetPosition(weapon.owner.transform.position, weapon.owner.Target.transform.position);
            projectile.SetData(Data);

            return true;
        }

        return false;
    }
}