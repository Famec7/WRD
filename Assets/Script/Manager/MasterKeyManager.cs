using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasterKeyManager : Singleton<MasterKeyManager>
{
    public Transform ParentTransform;
    public GameObject WeaponPickerUIPrefab;
    public List<GameObject> WeaponPickerList = new List<GameObject>();
    public List<Button> MasterKeyButton = new List<Button>();
    public List<TextMeshProUGUI> MasterKeyCountText = new List<TextMeshProUGUI>();
    public int[] masterKeyCnt = new int[6];

    void Start()
    {
        foreach (WeaponTier tier in System.Enum.GetValues(typeof(WeaponTier)))
        {
            if (tier == WeaponTier.Empty || tier == WeaponTier.COUNT) continue;

            var WeaponPicker = Instantiate(WeaponPickerUIPrefab, ParentTransform);
            WeaponPicker.GetComponent<WeaponPickerUI>().Init(tier);
            WeaponPicker.SetActive(false);
            WeaponPickerList.Add(WeaponPicker);
            UpdateMasterKeyCount(tier, 100);
        }
    }

    public void UpdateMasterKeyCount(WeaponTier tier, int changeAmount)
    {
        // 유효성 검사
        if (tier == WeaponTier.Empty || tier == WeaponTier.COUNT)
        {
            Debug.LogError("Invalid WeaponTier provided.");
            return;
        }

        int index = (int)tier - 1; // 배열에서 WeaponTier의 인덱스 계산
        masterKeyCnt[index] += changeAmount; // 배열 값 변경

        // 값이 0보다 작아지지 않도록 보정
        if (masterKeyCnt[index] < 0)
        {
            masterKeyCnt[index] = 0;
            Debug.LogWarning($"{tier} 마스터 키의 개수는 음수가 될 수 없습니다. 0으로 설정합니다.");
        }

        // 텍스트 UI 업데이트
        if (index >= 0 && index < MasterKeyCountText.Count)
        {
            MasterKeyCountText[index].text = masterKeyCnt[index].ToString(); // 텍스트 갱신
        }
        else
        {
            Debug.LogWarning($"UI 텍스트가 설정되지 않은 인덱스: {index}");
        }

        Debug.Log($"{tier} 마스터 키의 개수가 업데이트되었습니다. 현재 개수: {masterKeyCnt[index]}");
    }

    public void ClickMasterKeyButton(int idx)
    {
        // 유효성 검사: idx가 WeaponPickerList 범위 내에 있는지 확인
        if (idx < 0 || idx >= WeaponPickerList.Count || masterKeyCnt[idx] <= 0)
        {
            Debug.LogError($"Invalid index: {idx}. WeaponPickerList에 해당 인덱스가 없습니다.");
            return;
        }

        // 모든 WeaponPicker UI 비활성화
        foreach (var weaponPicker in WeaponPickerList)
        {
            weaponPicker.SetActive(false);
        }

        // 선택한 WeaponPicker UI 활성화
        WeaponPickerList[idx].SetActive(true);
        Debug.Log($"WeaponPicker UI {idx}가 활성화되었습니다.");
    }

    protected override void Init()
    {
        ;
    }
}