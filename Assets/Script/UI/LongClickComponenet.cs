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

    public GameObject LongClickPopUpUI;
    
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

    public void MouseUp()
    {

        if (!isLongClick && weaponID > 0 )
        {
            hasItem = GameManager.instance.useAbleWeaponCnt[weaponID - 1] > 0;

            UIManager.instance.CreateCombineUI(weaponID,true,isInventory,GetComponent<InventorySlot>().isEquiped);
            
            LongClickPopUpUI = UIManager.instance.longClickPopUpUI;
            LongClickPopUpUI.SetActive(true);
            LongClickPopUpUI.GetComponent<LongClickPopUpUi>().weaponID = weaponID;
            
            if (isInventory)
            {
                UIManager.instance.CreateInventoryDescriptionUI(weaponID);
                InventoryManager.instance.inventorySelectUI.SetActive(true);
                
                LongClickPopUpUI.GetComponent<LongClickPopUpUi>()._bookmarkButton.SetActive(true);
                LongClickPopUpUI.GetComponent<LongClickPopUpUi>()._equipButton.SetActive(hasItem);
                
                InventoryManager.instance.inventorySelectUI.GetComponent<RectTransform>().position = transform.position;
                UIManager.instance.SetActiveBlockImage(true);
                LongClickPopUpUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(-420,162);
            }

            if (isWeaponSlot)
            {
                LongClickPopUpUI.GetComponent<LongClickPopUpUi>()._bookmarkButton.SetActive(false);
                LongClickPopUpUI.GetComponent<LongClickPopUpUi>()._equipButton.SetActive(true);
                LongClickPopUpUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(88,352);
            }
            
            if(!isInventory) 
                LongClickPopUpUI.GetComponent<LongClickPopUpUi>().weaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;
        
                
            LongClickPopUpUI.GetComponent<LongClickPopUpUi>().inventorySlot = GetComponent<InventorySlot>();      
            LongClickPopUpUI.GetComponent<LongClickPopUpUi>().SetFavoriteButtonText(isBookmarked, isInventory,isWeaponSlot);
        }
    
        isClicked = false;
        isLongClick = false;
        elapsedTime = 0.0f;
    }

   
}
