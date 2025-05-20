using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardPopUpUI : MonoBehaviour
{
    #region RewardPopupUIElements
    [SerializeField]
    private TextMeshProUGUI titleTMP_;
    [SerializeField]
    private GameObject rewardSpace_;
    [SerializeField]
    private GameObject rewardSlotPrefab_;
    #endregion

    private void OnEnable()
    {
        StartCoroutine(CloseRewardPopupAfterDelay());
    }

    IEnumerator CloseRewardPopupAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    public void SettingRewardPopUpUI(bool isBoss, int idx)
    {
        string titleText = "";

        if (isBoss)
        {
            titleText = GameManager.Instance.wave.ToString() + "라운드 보스 클리어!\n";
        }
        else
        {
            titleText = "미션" + (idx+1).ToString() + "클리어!\n";
        }

        titleText += "다음 보상 획득";
        titleTMP_.text = titleText;
    }

    // rewardSlot의 위치를 수동으로 지정하는 헬퍼 메서드
    private void PositionRewardSlot(GameObject rewardSlot)
    {
        // Instantiate 후 rewardSpace_의 자식 개수를 기준으로 인덱스 결정
        int index = rewardSpace_.transform.childCount - 1;  // 새로 추가된 슬롯이 마지막 자식

        int col = index % 3;       // 0, 1, 2 (한 줄에 3개)
        int row = index / 3;       // 0번 줄, 1번 줄, 2번 줄...

        // 시작 좌표 (75, -75)에서 x 간격 225, y 간격 -225로 계산
        float xPos = 75 + col * 225;
        float yPos = -75 - row * 225;

        RectTransform slotRect = rewardSlot.GetComponent<RectTransform>();
        slotRect.anchoredPosition = new Vector2(xPos, yPos);
    }

    public void CreateRandomWeaponRewardSlot(int weaponNum)
    {
        GameObject rewardSlot = Instantiate(rewardSlotPrefab_, rewardSpace_.transform);
        PositionRewardSlot(rewardSlot);
        rewardSlot.GetComponent<RewardSlot>().SettingRandomWeaponRewardSlot(weaponNum);
    }

    public void CreateModifyRewardSlot(int cnt)
    {
        GameObject rewardSlot = Instantiate(rewardSlotPrefab_, rewardSpace_.transform);
        PositionRewardSlot(rewardSlot);
        rewardSlot.GetComponent<RewardSlot>().SettingModifyRewardSlot(cnt);
    }

    public void CreateMasterKeyRewardSlot(Tuple<WeaponTier, int> rewardTuple)
    {
        GameObject rewardSlot = Instantiate(rewardSlotPrefab_, rewardSpace_.transform);
        PositionRewardSlot(rewardSlot);
        rewardSlot.GetComponent<RewardSlot>().SettingMasterKeyRewardSlot(rewardTuple);
    }
}
