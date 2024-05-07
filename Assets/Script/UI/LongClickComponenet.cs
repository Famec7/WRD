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
        if (!isLongClick)
        {
            UIManager.instance.CreateCombineUI(weaponID);
        }

        isClicked = false;
        isLongClick = false;
        elapsedTime = 0.0f;
    }

    void Update()
    {
        if (isClicked)
        {
            elapsedTime += Time.deltaTime;
            if (longClickTime < elapsedTime)
            {
                isLongClick = true;
                isNormalClick = false;
            }
        }
        
        if (isLongClick)
        {
            LongClickPopUpUI = UIManager.instance.longClickPopUpUI;
            
            LongClickPopUpUI.SetActive(true);
            LongClickPopUpUI.GetComponent<LongClickPopUpUi>().weaponID = weaponID;
            UIManager.instance.SetActiveBlockImage(true);
            LongClickPopUpUI.GetComponent<RectTransform>().position = transform.position + new Vector3(5,40);
            LongClickPopUpUI.GetComponent<LongClickPopUpUi>().SetFavoriteButtonText(isBookmarked, isInventory,isWeaponSlot);
        }
    }
}
