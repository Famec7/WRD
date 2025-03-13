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
    public GameObject ScrollView;
    public GameObject Content;

    [SerializeField]
    private GridLayoutGroup gridLayoutGroup_;
    private int optinCnt_ = 0;

    private void Awake()
    {
        SetClosePopUp();
    }

  

    public void Init(WeaponTier tier)
    {
        Tier = tier;
        List<WeaponData> dataList = WeaponDataManager.Instance.Database.GetAllSameTierWeaponData(tier);
        foreach (var data in dataList)
        {
            GameObject WeaponPickButton = Instantiate(WeaponPickButtonPrefab, Content.transform);
            string weaponIconPath = "WeaponIcon/" + data.num;
            WeaponPickButton.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
            InventoryItem item = new InventoryItem
            {
                image = ResourceManager.Instance.Load<Sprite>(weaponIconPath)
            };
            WeaponPickButton.GetComponent<Button>().onClick.AddListener(() => { 
                InventoryManager.instance.OnclickWeaponPicker(data.num); 
                gameObject.SetActive(false);
            });

            optinCnt_++;
        }

        SetSize();
    }
    protected override void SetClosePopUp()
    {
        ;
    }

    public void SetSize()
    {
        float width = 700f;
        if (optinCnt_ >= 10)
            width = 700f;
        else if (optinCnt_ >= 8)
            width = 600f;
        else
            width = 500f;

        if (optinCnt_ % 2 == 1)
            gridLayoutGroup_.constraintCount = optinCnt_ / 2 + 1;
        else
            gridLayoutGroup_.constraintCount = optinCnt_ / 2;

        GetComponent<RectTransform>().sizeDelta = new Vector2(width, 300);
        ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 300);
    }
}
