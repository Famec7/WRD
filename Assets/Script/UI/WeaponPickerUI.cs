using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickerUI : UIPopUp
{
    // Start is called before the first frame update
    public WeaponTier Tier;
    public GameObject WeaponPickButtonPrefab;
    private Scrollbar _scrollbar;
    public Transform _scrollbarTransform;

    private void Awake()
    {
        _scrollbar = GetComponentInChildren<Scrollbar>();
        SetClosePopUp();
    }

    public void Init(WeaponTier tier)
    {
        Tier = tier;
        List<WeaponData> dataList = WeaponDataManager.Instance.Database.GetAllSameTierWeaponData(tier);
        foreach (var data in dataList)
        {
            Debug.Log("Init" + data);
            GameObject WeaponPickButton = Instantiate(WeaponPickButtonPrefab, _scrollbarTransform);
            string weaponIconPath = "WeaponIcon/" + data.num;
            WeaponPickButton.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
            InventoryItem item = new InventoryItem
            {
                image = ResourceManager.Instance.Load<Sprite>(weaponIconPath)
            };
            WeaponPickButton.GetComponent<Button>().onClick.AddListener(() => InventoryManager.instance.OpenWeaponPickerConfirmPopUp(data.num));
        }
    }
    protected override void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
          gameObject.SetActive(false);
        });
    }
}
