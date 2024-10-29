using System.Collections.Generic;
using UnityEngine;

public class DividedDevilBullet : PassiveSkillBase
{
    private List<Monster> _targets;
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger())
            return false;

        if (FindTargetsBy(target))
        {
            SpawnBullet();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private bool FindTargetsBy(GameObject target)
    {
        _targets.Clear();
        
        var targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, 3f, default, targetLayer);
        if(targets.Count == 0)
            return false;

        foreach (var t in targets)
        {
            if (t.gameObject.CompareTag("Boss"))
            {
                _targets.Add(t.GetComponent<Monster>());
            }
        }
        
        int targetMaxCount = (int)Data.GetValue(0);
        if (_targets.Count != 0)
            targetMaxCount--;

        for (int i = 0; i < targetMaxCount; i++)
        {
            if (targets[i].TryGetComponent(out Monster monster))
            {
                _targets.Add(monster);
            }
        }

        return true;
    }

    private void SpawnBullet()
    {
        int bulletCount = (int)Data.GetValue(0);
        int targetIndex = 0;
        float damage = Data.GetValue(1);
        
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 spawnPosition = this.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            
            GuidedProjectile bullet = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, spawnPosition);

            bullet.Target = _targets[targetIndex].gameObject;
            bullet.OnHit += () => OnHit(_targets[targetIndex], damage);
            
            targetIndex++;
            
            if (targetIndex >= _targets.Count)
            {
                targetIndex = 0;
                damage = Data.GetValue(3);
            }
        }
    }

    private void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);

        StatusEffect statusEffect = new Mark(monster.gameObject, Data.GetValue(2));
        StatusEffectManager.Instance.AddStatusEffect(monster.status, statusEffect);
    }
}