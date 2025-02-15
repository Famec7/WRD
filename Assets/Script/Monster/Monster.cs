using System;
using UnityEngine;
using static UnityEngine.Rendering.BoolParameter;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour, IPoolObject
{
    public float hp;
    public float speed;
    public float armor;
    public bool isMovable;
    public bool isDead = false;
    public Status status;

    [HideInInspector] public GameObject hpUI;

    public event Action OnMonsterStart;
    public event Action OnMonsterAttacked;
    public event Action OnMonsterDeath;
    public void HasAttacked(float damage)
    {
        damage += status.DamageAmplification * damage;
        damage += status.DevilBulletDamageAmplification * damage;
        
        status.HP -= damage;
        if (status.HP <= 0 && !isDead)
            IsDead();
        else
        {
            OnMonsterAttacked?.Invoke();//죽지 않고 데미지 받으면 데미지 Action 실행
        }
    }
    
    public void Die()
    {
        if (!isDead)
            IsDead();
    }

    private void IsDead()
    {
        isDead = true;

        OnMonsterDeath?.Invoke();

        gameObject.GetComponent<MonsterMoveComponent>().ResetCurrentSegment();

        if (status.unitCode >= UnitCode.MISSIONBOSS1 && status.unitCode <= UnitCode.MISSIONBOSS6)
        {
            MissionManager.Instance.TargetMonsterList.Remove(this);

            // 타이머 리셋 및 제거
            if (MissionManager.Instance.MonsterTimerDict.TryGetValue(this, out MissionTimer timer))
            {
                timer.ResetTimer(); // 타이머 리셋
                Destroy(timer.gameObject); // 타이머 오브젝트 제거 (필요한 경우)
                MissionManager.Instance.MonsterTimerDict.Remove(this); // 딕셔너리에서 제거
            }
            RewardManager.Instance.GetReward(status.unitCode);
            MessageManager.Instance.ShowMessage(status.unitCode.ToString() + " Clear!", new Vector2(0,200), 2f, 0.5f);
            MissionManager.Instance.missionInfo._missionSlots[status.unitCode - UnitCode.MISSIONBOSS1].Clear(true);

        }

        if (status.unitCode >= UnitCode.BOSS1 && status.unitCode <= UnitCode.BOSS6)
            RewardManager.Instance.GetReward(status.unitCode);

        MonsterHPBarPool.ReturnObject(hpUI.GetComponent<MonsterHPBar>());
        MonsterPoolManager.Instance.ReturnObject(status.unitCode.ToString(), gameObject);
    }

    public void GetFromPool()
    {
        // 구현 내용 생략
        OnMonsterStart?.Invoke();
    }

    public void ReturnToPool()
    {
        // 구현 내용 생략
    }
}
