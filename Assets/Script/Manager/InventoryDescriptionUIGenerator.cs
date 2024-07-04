using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDescriptionUIGenerator : Singleton<InventoryDescriptionUIGenerator>
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject inventoryDescriptionUIPrefab;
    public Transform parentTransform;
    public List<GameObject> inventoryDescriptionUIList;

    
    protected override void Init()
    {
        return;
    }
    
    void Start()
    {
        for (int weaponId = 1; weaponId <= WeaponDataManager.Instance.Database.GetWeaponDataCount(); weaponId++)
        {
            var data = WeaponDataManager.Instance.GetWeaponData(weaponId);
            var inventoryDescriptionPopUpUIGameObject = Instantiate(inventoryDescriptionUIPrefab, parentTransform) as GameObject;
            InventoryDescriptionPopUpUI inventoryDescriptionPopUpUI = inventoryDescriptionPopUpUIGameObject.GetComponent<InventoryDescriptionPopUpUI>();
            inventoryDescriptionPopUpUI.weaponId = weaponId;
            inventoryDescriptionPopUpUI.weaponNameText.text = data.WeaponName;
            string weaponIconPath = "WeaponIcon/" + weaponId.ToString();

            inventoryDescriptionPopUpUI.weaponImage.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);

            inventoryDescriptionPopUpUI.weaponClassText.text = WeaponDataManager.Instance.GetKorWeaponClassText(weaponId);
            inventoryDescriptionPopUpUI.weaponStatText[0].text = data.AttackDamage.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[1].text = data.AttackSpeed.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[2].text = data.AttackRange.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[3].text = WeaponDataManager.Instance.GetKorWeaponTypeText(weaponId);
            inventoryDescriptionPopUpUI.weaponTypeText.text = WeaponDataManager.Instance.GetKorWeaponRTypeText(weaponId);
            
            inventoryDescriptionUIList.Add(inventoryDescriptionPopUpUIGameObject);
            inventoryDescriptionPopUpUIGameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
