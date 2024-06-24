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
        for (int weaponId = 1; weaponId <= WeaponDataManager.instance.Data.Length; weaponId++)
        {
            var inventoryDescriptionPopUpUIGameObject = Instantiate(inventoryDescriptionUIPrefab, parentTransform) as GameObject;
            InventoryDescriptionPopUpUI inventoryDescriptionPopUpUI = inventoryDescriptionPopUpUIGameObject.GetComponent<InventoryDescriptionPopUpUI>();
            inventoryDescriptionPopUpUI.weaponId = weaponId;
            inventoryDescriptionPopUpUI.weaponNameText.text = WeaponDataManager.instance.Data[weaponId - 1].weaponName;
            string weaponIconPath = "WeaponIcon/" + weaponId.ToString();

            inventoryDescriptionPopUpUI.weaponImage.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);

            inventoryDescriptionPopUpUI.weaponClassText.text = WeaponDataManager.instance.GetKorWeaponClassText(weaponId);
            inventoryDescriptionPopUpUI.weaponStatText[0].text = WeaponDataManager.instance.Data[weaponId - 1].attackDamage.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[1].text = WeaponDataManager.instance.Data[weaponId - 1].attackSpeed.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[2].text = WeaponDataManager.instance.Data[weaponId - 1].attackRange.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[3].text = WeaponDataManager.instance.GetKorWeaponTypeText(weaponId);
            inventoryDescriptionPopUpUI.weaponTypeText.text = WeaponDataManager.instance.GetKorWeaponRTypeText(weaponId);
            
            inventoryDescriptionUIList.Add(inventoryDescriptionPopUpUIGameObject);
            inventoryDescriptionPopUpUIGameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
