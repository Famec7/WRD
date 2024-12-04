using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GlueBomb : PassiveAuraSkillBase
{
    public List<BombProjectile> BombProjectiles = new List<BombProjectile>();

    // 몬스터별 폭탄 갯수와 BombNum 오브젝트 관리
    private Dictionary<Monster, BombNumController> monsterBombControllers = new Dictionary<Monster, BombNumController>();
    public Dictionary<Monster, List<BombProjectile>> monsterBombs = new Dictionary<Monster, List<BombProjectile>>();

    [Space]
    [Header("무기 종류에 맞는 사운드")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private GameObject bombNumPrefab; // BombNum 프리팹

    private void Start()
    {
        weapon.OnAttack = OnAttack;
    }

    private void OnAttack()
    {
        if (BombProjectiles.Count >= Data.GetValue(0))
        {
            Monster targetMonster = BombProjectiles[0].Target.GetComponent<Monster>();
           BombProjectiles[0].Explosion();

            if (!targetMonster.isDead)
            {
                RemoveBombFromMonster(targetMonster, BombProjectiles[0]);
                BombProjectiles.RemoveAt(0);
            }
        }
        else if (weapon.owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<BombProjectile>("Bomb", this.transform.position);
            projectile.damage = Data.GetValue(1);

            projectile.Target = weapon.owner.Target.gameObject;
            BombProjectiles.Add(projectile);


            StartCoroutine(AddBombToMonster(monster, projectile));
            SoundManager.Instance.PlaySFX(_attackSound);
        }
    }


    IEnumerator AddBombToMonster(Monster monster, BombProjectile bomb)
    {
        yield return new WaitForSeconds(0.2f);
        // BombNumController 생성 또는 갱신
        if (!monsterBombControllers.ContainsKey(monster))
        {
            var bombNumInstance = Instantiate(bombNumPrefab);
            bombNumInstance.transform.position = monster.transform.position + new Vector3(0, 0.2f, 0); 
            bombNumInstance.GetComponent<BombNumController>().target = monster.transform;

            var controller = bombNumInstance.GetComponent<BombNumController>();
            monsterBombControllers[monster] = controller;

            // 몬스터가 죽으면 BombNum과 폭탄 제거 
            monster.OnMonsterDeath += () => RemoveAllBombsFromMonster(monster);
        }

        // 폭탄 추가
        if (!monsterBombs.ContainsKey(monster))
        {
            monsterBombs[monster] = new List<BombProjectile>();
        }
        monsterBombs[monster].Add(bomb);

        // 폭탄 갯수 업데이트
        monsterBombControllers[monster].UpdateBombCount(1);
    }

    public void RemoveBombFromMonster(Monster monster, BombProjectile bomb)
    {
        if (monsterBombs.ContainsKey(monster))
        {
            monsterBombs[monster].Remove(bomb);
            ProjectileManager.Instance.ReturnProjectileToPool<BombProjectile>(bomb, "Bomb");

            if (monsterBombControllers.ContainsKey(monster))
            {
                monsterBombControllers[monster].UpdateBombCount(-1);

                // 폭탄이 0개라면 BombNum 오브젝트 제거
                if (monsterBombControllers[monster].BombCount <= 0)
                {
                    Destroy(monsterBombControllers[monster].gameObject);
                    monsterBombControllers.Remove(monster);
                }
            }
        }
    }

    public void RemoveAllBombsFromMonster(Monster monster)
    {
        // 몬스터와 관련된 모든 폭탄 제거 (복사본 사용)
        if (monsterBombs.TryGetValue(monster, out var bombs))
        {
            foreach (var bomb in bombs.ToList()) // 안전한 반복 (복사본 사용)
            {
                bomb.IsAttached = false;
                bomb.Target = null;
                // BombProjectiles에서 제거
                if (BombProjectiles.Contains(bomb))
                {
                    BombProjectiles.Remove(bomb);
                }

                // 폭탄 객체를 풀로 반환
                ProjectileManager.Instance.ReturnProjectileToPool<BombProjectile>(bomb, "Bomb");
                bomb.gameObject.transform.position= Vector3.zero;
            }

            monsterBombs.Remove(monster);
        }

        // BombNum 오브젝트 제거
        if (monsterBombControllers.TryGetValue(monster, out var controller))
        {
            Destroy(controller.gameObject);
            monsterBombControllers.Remove(monster);
        }
    }

}
