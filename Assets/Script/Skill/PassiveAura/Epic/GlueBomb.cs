using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GlueBomb : PassiveAuraSkillBase
{
    public List<BombProjectile> BombProjectiles = new List<BombProjectile>();

    // 몬스터별 폭탄 개수와 BombNum 오브젝트 관리
    private Dictionary<Monster, BombNumController> monsterBombControllers = new Dictionary<Monster, BombNumController>();
    public Dictionary<Monster, List<BombProjectile>> monsterBombs = new Dictionary<Monster, List<BombProjectile>>();

    [Space]
    [Header("무기 종류에 맞는 사운드")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private GameObject bombNumPrefab; // BombNum 프리팹

    // 무기 장착 상태를 추적 (true면 장착, false면 해제)
    private bool weaponEquipped = false;

    private void Start()
    {
        weapon.OnAttack = OnAttack;
        // 초기 무기 장착 상태 확인
        weaponEquipped = WeaponManager.Instance.IsEuqiped(410);
        if (!weaponEquipped)
        {
            RemoveAllBombs();
        }
    }

    private void Update()
    {
        bool currentlyEquipped = WeaponManager.Instance.IsEuqiped(410);

        // 무기 재장착 시: 이전에 미장착 상태였다가 장착 상태가 되면 모든 리스트 및 딕셔너리 초기화
        if (currentlyEquipped && !weaponEquipped)
        {
            RemoveAllBombs();
            weaponEquipped = true;
        }
        // 무기 해제 시: 장착 상태였다가 미장착 상태가 되면 삭제
        else if (!currentlyEquipped && weaponEquipped)
        {
            RemoveAllBombs();
            weaponEquipped = false;
        }
    }

    private void OnAttack()
    {
        // 무기가 미장착이면 공격하지 않음
        if (!WeaponManager.Instance.IsEuqiped(410))
            return;

        // weapon, weapon.owner, weapon.owner.Target가 null이면 경고 후 종료
        if (weapon == null || weapon.owner == null || weapon.owner.Target == null)
        {
            Debug.LogWarning("Weapon, owner, or target is null in GlueBomb.OnAttack");
            return;
        }

        // BombProjectile 개수가 일정 값 이상이면 첫번째 폭탄을 터트림
        if (BombProjectiles.Count >= Data.GetValue(0))
        {
            // BombProjectile[0]과 해당 Target이 null이 아닌지 확인
            if (BombProjectiles[0] != null && BombProjectiles[0].Target != null)
            {
                Monster targetMonster = BombProjectiles[0].Target.GetComponent<Monster>();
                BombProjectiles[0].Explosion();

                if (targetMonster != null && !targetMonster.isDead)
                {
                    RemoveBombFromMonster(targetMonster, BombProjectiles[0]);
                    BombProjectiles.RemoveAt(0);
                }
            }
            else
            {
                // 만약 첫번째 폭탄이나 Target이 null이라면 리스트에서 제거
                BombProjectiles.RemoveAt(0);
            }
        }
        // 그렇지 않으면 새로운 BombProjectile 생성 후 몬스터에 추가
        else if (weapon.owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<BombProjectile>("Bomb", this.transform.position);
            projectile.damage = Data.GetValue(1);
            projectile.Target = weapon.owner.Target.gameObject;
            BombProjectiles.Add(projectile);

            StartCoroutine(AddBombToMonster(monster, projectile));
            SoundManager.Instance.PlaySFX(_attackSound, 0.5f);
        }
    }

    IEnumerator AddBombToMonster(Monster monster, BombProjectile bomb)
    {
        yield return new WaitForSeconds(0.2f);

        // 몬스터가 이미 죽었다면 중단
        if (monster.isDead)
            yield break;

        // BombNumController 생성 또는 갱신
        if (!monsterBombControllers.ContainsKey(monster))
        {
            var bombNumInstance = Instantiate(bombNumPrefab);
            bombNumInstance.transform.position = monster.transform.position + new Vector3(0, 0.2f, 0);
            var controller = bombNumInstance.GetComponent<BombNumController>();
            if (controller != null)
            {
                controller.target = monster.transform;
                monsterBombControllers[monster] = controller;
            }
            // 몬스터 사망 시 해당 몬스터와 관련된 모든 폭탄 삭제
            monster.OnMonsterDeath += () => RemoveAllBombsFromMonster(monster);
        }

        // 해당 몬스터에 BombProjectile 추가
        if (!monsterBombs.ContainsKey(monster))
        {
            monsterBombs[monster] = new List<BombProjectile>();
        }
        monsterBombs[monster].Add(bomb);

        // BombNumController가 유효하면 폭탄 수 업데이트
        if (monsterBombControllers.TryGetValue(monster, out var validController) && validController != null)
        {
            validController.UpdateBombCount(1);
        }
    }

    public void RemoveBombFromMonster(Monster monster, BombProjectile bomb)
    {
        if (monsterBombs.ContainsKey(monster))
        {
            monsterBombs[monster].Remove(bomb);
            ProjectileManager.Instance.ReturnProjectileToPool<BombProjectile>(bomb, "Bomb");

            if (monsterBombControllers.ContainsKey(monster))
            {
                var controller = monsterBombControllers[monster];
                if (controller == null)
                {
                    monsterBombControllers.Remove(monster);
                }
                else
                {
                    controller.UpdateBombCount(-1);
                    if (controller.BombCount <= 0)
                    {
                        Destroy(controller.gameObject);
                        monsterBombControllers.Remove(monster);
                    }
                }
            }
        }
    }

    public void RemoveAllBombsFromMonster(Monster monster)
    {
        // 해당 몬스터 관련 모든 BombProjectile 삭제
        if (monsterBombs.TryGetValue(monster, out var bombs))
        {
            foreach (var bomb in bombs.ToList())
            {
                bomb.IsAttached = false;
                bomb.Target = null;
                if (BombProjectiles.Contains(bomb))
                {
                    BombProjectiles.Remove(bomb);
                }
                ProjectileManager.Instance.ReturnProjectileToPool<BombProjectile>(bomb, "Bomb");
                bomb.gameObject.transform.position = Vector3.zero;
            }
            monsterBombs.Remove(monster);
        }

        // 해당 몬스터의 BombNumController 삭제
        if (monsterBombControllers.TryGetValue(monster, out var controller))
        {
            if (controller != null)
            {
                Destroy(controller.gameObject);
            }
            monsterBombControllers.Remove(monster);
        }
    }

    // 무기 교체 시 전체 BombProjectile 관련 데이터를 초기화하는 메서드
    public void RemoveAllBombs()
    {
        // 모든 BombProjectile 삭제
        foreach (var bomb in BombProjectiles.ToList())
        {
            bomb.IsAttached = false;
            bomb.Target = null;
            ProjectileManager.Instance.ReturnProjectileToPool<BombProjectile>(bomb, "Bomb");
            bomb.gameObject.transform.position = Vector3.zero;
        }
        BombProjectiles.Clear();

        // 모든 몬스터 관련 폭탄 리스트 초기화
        monsterBombs.Clear();

        // 모든 BombNumController 삭제
        foreach (var controller in monsterBombControllers.Values.ToList())
        {
            if (controller != null)
                Destroy(controller.gameObject);
        }
        monsterBombControllers.Clear();
    }
}
