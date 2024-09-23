using UnityEngine;

public class Ballista : RangedWeapon
{
    [SerializeField]
    private float _range = 2.0f;
    
    protected override void Attack()
    {
        base.Attack();

        Monster monster = FindNearestMonster(owner.Target.transform.position);

        var projectile =
            ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);

        projectile.Target = owner.Target.gameObject;
        projectile.Damage = Data.AttackDamage;

        projectile.SetType(type);

        notifyAction?.Invoke();
        
        passiveSkill.Activate(monster.gameObject);
    }

    private Monster FindNearestMonster(Vector3 targetPosition)
    {
        var targetLayer = LayerMaskManager.Instance.MonsterLayerMask;
        
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, _range, default, targetLayer);

        GameObject nearestTarget = null;
        float minDistance = Data.AttackRange;

        foreach (var tar in targets)
        {
            if(tar.gameObject.activeSelf is false)
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