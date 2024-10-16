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

    public void HasAttacked(float damage)
    {
        hp -= damage;
        status.HP -= damage + damage * status.damageAmplification;
        if (status.HP <= 0 && !isDead)
            IsDead();
    }

    private void IsDead()
    {
        isDead = true;

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

        MonsterHPBarPool.ReturnObject(transform.GetChild(1).GetComponent<MonsterHPBar>());
        MonsterPoolManager.Instance.ReturnObject(status.unitCode.ToString(), gameObject);
    }

    public void GetFromPool()
    {
        // 구현 내용 생략
    }

    public void ReturnToPool()
    {
        // 구현 내용 생략
    }
}
