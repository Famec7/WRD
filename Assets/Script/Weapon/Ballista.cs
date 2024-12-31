using UnityEngine;

public class Ballista : RangedWeapon
{
    [SerializeField]
    private float _range = 2.0f;
    
    [SerializeField]
    private AudioClip _twoTargetSound;
    
    protected override void Attack()
    {
        base.Attack();

        Monster monster = FindNearestMonster(owner.Target.transform.position);
        
        if (monster is null)
            return;
        
        if(GetPassiveSkill().Activate(monster.gameObject))
            return;
        
        var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);

        projectile.Target = monster.gameObject;
        projectile.SetType(type);
        
        projectile.OnHit += () => OnHit(monster, Data.AttackDamage);
        
        SoundManager.Instance.PlaySFX(_twoTargetSound);
    }

    private Monster FindNearestMonster(Vector3 targetPosition)
    {
        var targetLayer = LayerMaskManager.Instance.MonsterLayerMask;
        
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, _range, default, targetLayer);

        GameObject nearestTarget = null;
        float minDistance = _range;

        foreach (var tar in targets)
        {
            if(tar.gameObject.activeSelf is false)
                continue;
            
            if(tar.gameObject == owner.Target)
                continue;
            
            float distance = Vector3.Distance(targetPosition, tar.transform.position);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = tar.gameObject;
            }
        }

        return nearestTarget?.GetComponent<Monster>();
    }
}