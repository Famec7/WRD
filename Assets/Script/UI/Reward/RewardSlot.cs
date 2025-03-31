using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlot : MonoBehaviour
{
    #region RewardSlotElements
    [SerializeField]
    private TextMeshProUGUI countTMP_;
    [SerializeField]
    private Image rewardItemImage_;
    [SerializeField]
    private Image rewardSlotBackground_;
    #endregion

    private void Start()
    {
        StartCoroutine(MoveShrinkAndDestroy());
    }

    public IEnumerator MoveShrinkAndDestroy()
    {
        yield return new WaitForSeconds(1f);

        float duration = 0.9f;
        float elapsed = 0f;
        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector3 startLocalPos = rectTransform.localPosition;
        Vector3 startScale = rectTransform.localScale;

        RectTransform targetRect = UIManager.instance.InventoryOpenButton.GetComponent<RectTransform>();
        Vector3 targetLocalPos = targetRect.localPosition;
        Vector3 targetScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rectTransform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, t);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        rectTransform.localPosition = targetLocalPos;
        rectTransform.localScale = targetScale;
        Destroy(gameObject);
    }

    public void SettingRandomWeaponRewardSlot(int weaponNum)
    {
        var path = "WeaponIcon/" + weaponNum;
        rewardItemImage_.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
        countTMP_.gameObject.SetActive(false);
        rewardSlotBackground_.color = WeaponTierTranslator.GetClassColor(WeaponDataManager.Instance.Database.GetWeaponDataByNum(weaponNum).WeaponClass);
    }

    public void SettingMasterKeyRewardSlot(Tuple<WeaponTier, int> rewardTuple)
    {
        countTMP_.text = "x" + rewardTuple.Item2.ToString();
        rewardSlotBackground_.color = WeaponTierTranslator.GetClassColor(rewardTuple.Item1);
    }
}
