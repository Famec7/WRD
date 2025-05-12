using System.Collections.Generic;
using UnityEngine;

public class DividedDevilBullet : PassiveSkillBase
{
    [SerializeField]
    private AudioClip _bulletSound;
    
    private readonly List<Monster> _targets = new List<Monster>();

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger())
            return false;

        if (FindTargetsBy(target))
        {
            SpawnBullet();
            SoundManager.Instance.PlaySFX(_bulletSound);
            return true;
        }

        return false;
    }

    private bool FindTargetsBy(GameObject target)
    {
        _targets.Clear();

        var targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, 3f, default, targetLayer);
        if (targets.Count == 0)
            return false;

        int targetMaxCount = (int)Data.GetValue(0);
        foreach (var t in targets)
        {
            if (t.gameObject.CompareTag("Boss"))
            {
                _targets.Add(t.GetComponent<Monster>());
                targetMaxCount--;
            }
        }

        foreach (var t in targets)
        {
            if (t.TryGetComponent(out Monster monster))
            {
                _targets.Add(monster);

                targetMaxCount--;
                if (targetMaxCount == 0)
                {
                    break;
                }
            }
        }

        return true;
    }

    private void SpawnBullet()
    {
        int bulletCount = (int)Data.GetValue(0);
        int targetIndex = 0;
        float damage = Data.GetValue(1);

        while(bulletCount-- > 0)
        {
            Monster monster = _targets[targetIndex];
            
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            Vector3 spawnPosition = this.transform.position + new Vector3(randomX, randomY, 0);

            GuidedProjectile bullet = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, spawnPosition);
            bullet.SetType(RangedWeapon.Type.Gun);
            bullet.Target = monster.gameObject;
            bullet.OnHit = () => OnHit(monster, damage);
            
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
        StatusEffect statusEffect = new Mark(monster.gameObject, Data.GetValue(2));
        StatusEffectManager.Instance.AddStatusEffect(monster.status, statusEffect);

        monster.HasAttacked(damage);
    }
}