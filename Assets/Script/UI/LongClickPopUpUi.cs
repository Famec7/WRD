using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickPopUpUi : MonoBehaviour
{
    public GameObject _bookmarkButton;
    public GameObject _equipButton;
    public InventorySlot inventorySlot;

    public WeaponSlotUI weaponSlot;

    public int weaponID;

    public bool isBookmarked;
    public bool isInventory;
    public bool isWeaponSlot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
     
            var results = UIManager.instance.GetRayCastResult(true);
            // 없으면 return
            if (results.Count <= 0) return;

            bool isAnotherTouch = true;
            foreach (var result in results)
            {
                if ((result.gameObject.CompareTag("LongClickPopUpUI")))
                    isAnotherTouch = false;
            }

        }

        transform.SetAsLastSibling();
    }

    public void SetBookmarkedButtonText(bool isBookmarked, bool isInventory, bool isWeaponSlot)
    {

        var bookmarkButtonTMP = _bookmarkButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        var equipButtonTMP = _equipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
 

        if (!isWeaponSlot && GameManager.instance.useAbleWeaponCnt[weaponID - 1] > 0)
            equipButtonTMP.text = "장착";

        if (isBookmarked || (isInventory && BookMakredSlotUI.Instance.GetSlotWithWeaponID(weaponID)))
        {
            bookmarkButtonTMP.text = "즐겨찾기 해제";
        }
        else if (isInventory || isWeaponSlot)
        {
            bookmarkButtonTMP.text = "즐겨찾기 등록";
        }
        
        if (isWeaponSlot || isInventory && inventorySlot.isEquiped)
        {
            equipButtonTMP.text = "장착 해제";
        }
    }

    public void ClickDetailedDescriptionButton()
    {
        UIManager.instance.CreateDetailedDescriptionUI(weaponID);
        gameObject.SetActive(false);
        UIManager.instance.SetActiveBlockImage(false);
    }

    public void ClickBookMarkButton()
    {
        if (!isBookmarked && !BookMakredSlotUI.Instance.GetSlotWithWeaponID(weaponID))
        {
            BookMakredSlotUI.Instance.weaponID = weaponID;
            UIManager.instance.BookmarkSlotSelectUI.SetActive(true);
            UIManager.instance.BookmarkSlotUI.SetActive(true);
        }
        else if (BookMakredSlotUI.Instance.GetSlotWithWeaponID(weaponID))
        {
            BookMakredSlotUI.Instance.RemoveItem(BookMakredSlotUI.Instance.GetSlotWithWeaponID(weaponID).transform.GetChild(0).GetComponent<InventorySlot>());
        }

        UIManager.instance.CloseInventoryDescriptionPopUpUI();
        gameObject.SetActive(false);
    }
    
    public void ClickEquipButton()
    {
        if (isWeaponSlot || (isInventory && inventorySlot.isEquiped))
            UnEuqip(isInventory);
        else
        {
            WeaponUI.Instance.weaponID = weaponID;
            UIManager.instance.WeaponSlotSelectUI.SetActive(true);

            if(!isBookmarked)
                UIManager.instance.WeaponSlotSelectUI.GetComponent<SlotSelectUI>().SetItem(inventorySlot._weapon);
            else
            {
                InventoryItem item = InventoryManager.instance.FindUnEquipedItem(weaponID);
                if (item != null)
                    UIManager.instance.WeaponSlotSelectUI.GetComponent<SlotSelectUI>().SetItem(item);
            }
        }

        UIManager.instance.CloseInventoryDescriptionPopUpUI();
        gameObject.SetActive(false);
    }
    public void UnEuqip(bool isInventory = false)
    {
        //인벤토리 아닌데서 키면
        if (!isInventory) 
            weaponSlot.hasWeapon = false;
        else
            inventorySlot.isEquiped = false;


        //인벤토리 다시 그리기
        InventoryManager.instance.FreshSlot();
        // 장착하고 있는 무기 줄이기
        GameManager.instance.RemoveUseWeaponList(weaponID);
        //만약 웨폰 슬롯이 널이면
        if (isInventory || weaponSlot == null)
        {
            for (int weapnUIIDX = 0; weapnUIIDX < UIManager.instance.weaponSlotUI.Length; weapnUIIDX++)
            {
                //하단 웨폰슬롯ui중에 무기 같은 거 찾아서 웨폰슬롯으로 넣어줌
                WeaponSlotUI targetWeaponSlot = UIManager.instance.weaponSlotUI[weapnUIIDX];
                if (targetWeaponSlot.hasWeapon)
                {
                    if (targetWeaponSlot.inventorySlot.weapon == inventorySlot.weapon)
                    {
                        weaponSlot = targetWeaponSlot;
                        break;
                    }
                }
            }
        }
        inventorySlot.GetComponent<InventorySlot>().equipText.gameObject.SetActive(false);

        //장착무기 ui 초기화
        weaponSlot.Init();
        weaponSlot = null;
        inventorySlot = null;

        BookMakredSlotUI.Instance.UpdateAllSlot();
        gameObject.SetActive(false);
        
        // 장착중인 무기 해제
        WeaponManager.Instance.RemoveWeapon(weaponID);
    }

    public void SetLongClickPopUpUI(int _weaponID, bool _isBookmarked, bool _isInventory,bool _isWeaponSlot, InventorySlot _inventorySlot, WeaponSlotUI _weaponSlot)
    {
        weaponID = _weaponID;
        isBookmarked = _isBookmarked;
        isInventory = _isInventory;
        isWeaponSlot = _isWeaponSlot;
        inventorySlot = _inventorySlot;
        weaponSlot = _weaponSlot;
    }
}
    