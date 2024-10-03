using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongClickComponenet : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float elapsedTime = 0.0f;
    private float longClickTime = 2.0f;
    private bool isClicked = false;
    private bool isLongClick = false;
    private bool isNormalClick = false;
    public int weaponID;
    public bool isWeaponSlot;
    public bool isInventory;
    public bool isBookmarked;
    private bool hasItem;
    private RectTransform rectTransform;

    public GameObject LongClickPopUpUIObject;

    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        if(isWeaponSlot)
            weaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;
    }

    public void MouseDown()
    {
        if (weaponID > 0) 
            isClicked = true;
    }
    // ���콺 ���� ��
    public void MouseUp()
    {
        if (!isLongClick && weaponID > 0 )
        {          
            hasItem = GameManager.Instance.useAbleWeaponCnt[weaponID-1] > 0;
            UIManager.instance.CreateCombineUI(weaponID,true,isInventory,GetComponent<InventorySlot>().isEquiped);
            GameObject combineUI =UIManager.instance.combinePopupUIStack.Peek().gameObject;
            LongClickPopUpUIObject = UIManager.instance.longClickPopUpUI;
            LongClickPopUpUIObject.transform.parent = UIManager.instance._popupCanvas.transform;
            LongClickPopUpUi longClickPopUpUI = LongClickPopUpUIObject.GetComponent<LongClickPopUpUi>();
            LongClickPopUpUIObject.SetActive(true);

            longClickPopUpUI.weaponID = weaponID;
            longClickPopUpUI.inventorySlot = GetComponent<InventorySlot>();
            longClickPopUpUI._equipButton.SetActive(hasItem);

            if (isInventory)
            {
                GameObject inventoryDescriptionUI = UIManager.instance.CreateInventoryDescriptionUI(weaponID);
                combineUI.transform.parent = inventoryDescriptionUI.transform;
                LongClickPopUpUIObject.transform.parent = inventoryDescriptionUI.transform;

                InventoryManager.instance.inventorySelectUI.SetActive(true);
                InventoryManager.instance.inventorySelectUI.GetComponent<RectTransform>().position = transform.position;
                InventoryManager.instance.inventorySelectUI.GetComponent<InventorySelectUI>().enabled = true;
                InventoryManager.instance.inventorySelectUI.GetComponent<InventorySelectUI>().targetInventory = gameObject;

                longClickPopUpUI._bookmarkButton.SetActive(true);
                longClickPopUpUI._equipButton.SetActive(GameManager.Instance.weaponCnt[weaponID - 1] > 0);

                UIManager.instance.SetActiveBlockImage(true);
                LongClickPopUpUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-585,-90.5f);

                longClickPopUpUI.inventorySlot = GetComponent<InventorySlot>();
                combineUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(-70, -360);
            }

            if (isWeaponSlot)
            {
                longClickPopUpUI._bookmarkButton.SetActive(false);
                longClickPopUpUI._equipButton.SetActive(true);

                LongClickPopUpUIObject.transform.parent = combineUI.transform;
                LongClickPopUpUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-125,56);

                longClickPopUpUI.weaponSlot = transform.parent.GetComponent<WeaponSlotUI>();
                longClickPopUpUI.inventorySlot = longClickPopUpUI.weaponSlot.inventorySlot;
            }

            if (isBookmarked)
                longClickPopUpUI._bookmarkButton.SetActive(true);

            if (!isInventory)
                longClickPopUpUI.weaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;

            longClickPopUpUI.isBookmarked = isBookmarked;
            longClickPopUpUI.isInventory = isInventory;
            longClickPopUpUI.isWeaponSlot = isWeaponSlot;
            longClickPopUpUI.SetBookmarkedButtonText(isBookmarked, isInventory,isWeaponSlot);
        }
    
        isClicked = false;
        isLongClick = false;
        elapsedTime = 0.0f;
    }

   
}
