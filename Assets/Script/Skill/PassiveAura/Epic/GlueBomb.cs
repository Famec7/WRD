using System.Collections.Generic;
using UnityEngine;

public class GlueBomb : PassiveAuraSkillBase
{
    public List<BombProjectile> BombProjectiles = new List<BombProjectile>();

    // 몬스터별 폭탄 갯수와 BombNum 오브젝트 관리
    private Dictionary<Monster, BombNumController> monsterBombControllers = new Dictionary<Monster, BombNumController>();
    private Dictionary<Monster, List<BombProjectile>> monsterBombs = new Dictionary<Monster, List<BombProjectile>>();

    [Space]
    [Header("무기 종류에 맞는 사운드")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private GameObject bombNumPrefab; // BombNum 프리팹

    private void Start()
    {
        weapon.OnAttack += OnAttack;
    }

    private void OnAttack()
    {
        // 폭탄 터뜨리기
        if (BombProjectiles.Count >= Data.GetValue(0))
        {
            BombProjectiles[0].Explosion();
            RemoveBombFromMonster(BombProjectiles[0].Target.GetComponent<Monster>(), BombProjectiles[0]);
            BombProjectiles.RemoveAt(0);
        }
        else if (weapon.owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<BombProjectile>("Bomb", this.transform.position);
            projectile.damage = Data.GetValue(1);

            projectile.Target = weapon.owner.Target.gameObject;
            BombProjectiles.Add(projectile);

            AddBombToMonster(monster, projectile);
            SoundManager.Instance.PlaySFX(_attackSound);
        }
    }

    private void AddBombToMonster(Monster monster, BombProjectile bomb)
    {
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

    private void RemoveBombFromMonster(Monster monster, BombProjectile bomb)
    {
        if (monsterBombs.ContainsKey(monster))
        {
            monsterBombs[monster].Remove(bomb);
            Destroy(bomb.gameObject);

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

    private void RemoveAllBombsFromMonster(Monster monster)
    {
        // 몬스터와 관련된 모든 폭탄 제거
        if (monsterBombs.ContainsKey(monster))
        {
            foreach (var bomb in monsterBombs[monster])
            {
                Destroy(bomb.gameObject);
            }
            monsterBombs.Remove(monster);
        }

        // BombNum 오브젝트 제거
        if (monsterBombControllers.ContainsKey(monster))
        {
            Destroy(monsterBombControllers[monster].gameObject);
            monsterBombControllers.Remove(monster);
        }
    }
}
