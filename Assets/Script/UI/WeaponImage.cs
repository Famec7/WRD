using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class WeaponImage : MonoBehaviour
{
    // Start is called before the first frame update

    public float minClickTime = 0.3f;

    private float clickTime;
    private bool isClick;

    private Vector3 originalPos;
    public bool isDrag = false;
    public bool isInventory;
    public bool isBookmarked;
    private RectTransform rectTransform;
    private RectTransform rectTransform1;
    private LongClickComponenet longClickComponenet;

    void Start()
    {
        rectTransform1 = GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        originalPos = transform.position;

        longClickComponenet = GetComponent<LongClickComponenet>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isClick)
            clickTime += Time.deltaTime;
        else
            clickTime = 0;

        if (clickTime > minClickTime)
        {
            transform.position = Input.mousePosition;
        }

        if (!isDrag)
        {
            transform.position = originalPos;
        }
        else
        {
            float sizeOffset = transform.parent.gameObject.name == "MainWeaponSlot" ? 1.5f : 1f;
            if ((transform.parent.gameObject.CompareTag("WeaponSlot")))
                rectTransform1.sizeDelta = new Vector2(120, 120) * sizeOffset;

            if ((transform.parent.gameObject.CompareTag("BookMarkedSlot")))
                rectTransform.sizeDelta = new Vector2(100, 100);
        }
    }


    public void ButtonDown()
    {
        isClick = true;
    }

    public void ButtonUp()
    {
        isClick = false;
    }

    public void ButtonDrag()
    {
        if (!transform.parent.GetComponent<WeaponSlotUI>().isWeaponUseable && transform.parent.CompareTag("BookMarkedSlot")) return;

        transform.position = Input.mousePosition;
        var parent = transform.parent;
        parent.GetComponent<RectTransform>().SetAsLastSibling();
        parent.parent.GetComponent<RectTransform>().SetAsLastSibling();
        
        if (isBookmarked)
            parent.parent.parent.GetComponent<RectTransform>().SetAsLastSibling();

        UIManager.instance.touchPos = Input.mousePosition;

        isDrag = true;
        var results = UIManager.instance.GetRayCastResult();

        if (results.Count <= 0) return;
      
    }

    public void ButtonDrop()
    {
        var results = UIManager.instance.GetRayCastResult();
        if (results.Count <= 0) return;

        GameObject targetSlotObject = null;
        
        foreach (var result in results)
        {
            if (result.gameObject.CompareTag(transform.parent.gameObject.tag)  && result.gameObject != transform.parent.gameObject)
                targetSlotObject = result.gameObject;

            if (isInventory && result.gameObject.CompareTag("InventorySlot"))
                targetSlotObject = result.gameObject;

            if(result.gameObject.CompareTag("WeaponSlot"))
                targetSlotObject = result.gameObject;

            if (result.gameObject.CompareTag("BookMarkedSlot"))
                targetSlotObject = result.gameObject;
        }
        
        if (!isInventory && !gameObject.transform.parent.CompareTag("BookMarkedSlot") && !targetSlotObject.gameObject.CompareTag("BookMarkedSlot"))
        {
            if (targetSlotObject != null)
            {
                WeaponSlotUI targetSlot = targetSlotObject.GetComponent<WeaponSlotUI>();
                Image targetSlotImage = targetSlot.transform.GetChild(0).gameObject.GetComponent<Image>();

                int weaponID = transform.parent.gameObject.GetComponent<WeaponSlotUI>().weaponID;
                int targetweaponID = targetSlot.weaponID;
                bool alreadyHasWeapon = targetSlot.GetComponent<WeaponSlotUI>().hasWeapon;
                InventoryItem targetItem = targetSlot.transform.GetChild(0).gameObject.GetComponent<InventorySlot>().weapon;
                InventorySlot targetInventorySlot = targetSlot.inventorySlot;

                Sprite targetSprite = targetSlotImage.sprite;
                Sprite mySprite = gameObject.GetComponent<Image>().sprite;
                Color myColor = gameObject.GetComponent<Image>().color;
                InventoryItem myItem = gameObject.GetComponent<InventorySlot>().weapon;
                InventorySlot myInventorySlot = gameObject.transform.parent.GetComponent<WeaponSlotUI>().inventorySlot;

                targetSlot.hasWeapon = true;
                targetSlot.transform.GetChild(0).GetComponent<LongClickComponenet>().weaponID = weaponID;
                targetSlot.weaponID = weaponID;
                targetSlotImage.enabled = true;
                targetSlot.transform.GetChild(0).gameObject.GetComponent<InventorySlot>().weapon = gameObject.GetComponent<InventorySlot>().weapon;
                targetSlotImage.sprite = mySprite;
                targetSlotImage.color = myColor;
                targetSlot.inventorySlot = myInventorySlot;

                if (alreadyHasWeapon)
                {
                    gameObject.GetComponent<Image>().sprite = targetSprite;
                    gameObject.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
                    gameObject.GetComponent<Image>().enabled = true;
                    gameObject.GetComponent<InventorySlot>().weapon = targetItem;
                    transform.parent.gameObject.GetComponent<WeaponSlotUI>().hasWeapon = true;
                    transform.parent.gameObject.GetComponent<WeaponSlotUI>().inventorySlot = targetInventorySlot; 
                }

                else
                    transform.parent.GetComponent<WeaponSlotUI>().Init();

                transform.parent.gameObject.GetComponent<WeaponSlotUI>().weaponID = targetweaponID;
                transform.gameObject.GetComponent<LongClickComponenet>().weaponID = targetweaponID;
                UIManager.instance.touchPos = Input.mousePosition;
            }
        }

        if (gameObject.transform.parent.CompareTag("BookMarkedSlot"))
        {
            if (targetSlotObject.gameObject.CompareTag("WeaponSlot"))
            {
                int targetWeaponID = targetSlotObject.GetComponent<WeaponSlotUI>().weaponID;
                int myWeaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;

                if (targetSlotObject.GetComponent<WeaponSlotUI>().hasWeapon)
                {
                  
                    LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
                    longClickPopUpUi.weaponID = targetWeaponID;
                    longClickPopUpUi.weaponSlot = targetSlotObject.GetComponent<WeaponSlotUI>();
                    longClickPopUpUi.inventorySlot = targetSlotObject.GetComponent<WeaponSlotUI>().inventorySlot;
                    longClickPopUpUi.UnEuqip();

                    BookMakredSlotUI.Instance.RemoveItem(BookMakredSlotUI.Instance.GetSlotWithWeaponID(transform.parent.gameObject.GetComponent<WeaponSlotUI>().weaponID).transform.GetChild(0).GetComponent<InventorySlot>());
                    BookMakredSlotUI.Instance.weaponID = targetWeaponID;
                    BookMakredSlotUI.Instance.AddItem(int.Parse(transform.parent.name));
                }

                string numberString = Regex.Replace(targetSlotObject.gameObject.name, @"\D", "");
                int order = numberString == "" ? 4: int.Parse(numberString);

                WeaponUI.Instance.AddItem(order, InventoryManager.instance.FindUnEquipedItem(myWeaponID));
                transform.parent.GetComponent<WeaponSlotUI>().ChangeWeaponUseable();
                
            }
                      
        }

        //if (gameObject.transform.parent.CompareTag("WeaponSlot") && targetSlotObject.gameObject.CompareTag("BookMarkedSlot"))
        //{
        //    int targetWeaponID = targetSlotObject.GetComponent<WeaponSlotUI>().weaponID;
        //    int myWeaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;

        //    if (!BookMakredSlotUI.Instance.isDuplicatedID(myWeaponID))
        //    {
        //        if (targetSlotObject.GetComponent<WeaponSlotUI>().hasWeapon)
        //            BookMakredSlotUI.Instance.RemoveItem(targetSlotObject.transform.GetChild(0).GetComponent<InventorySlot>());


        //        BookMakredSlotUI.Instance.weaponID = myWeaponID;
        //        BookMakredSlotUI.Instance.AddItem(int.Parse(targetSlotObject.name));
        //    }

        //    if (targetSlotObject.GetComponent<WeaponSlotUI>().hasWeapon && GameManager.instance.useAbleWeaponCnt[targetWeaponID - 1] > 0)
        //    {
        //        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        //        longClickPopUpUi.weaponID = myWeaponID;
        //        longClickPopUpUi.weaponSlot = transform.parent.GetComponent<WeaponSlotUI>();
        //        longClickPopUpUi.inventorySlot = transform.parent.GetComponent<WeaponSlotUI>().inventorySlot;
        //        longClickPopUpUi.UnEuqip();

        //        longClickPopUpUi.isBookmarked = true;
        //        string numberString = Regex.Replace(transform.parent.gameObject.name, @"\D", "");
        //        int order = numberString == "" ? 4 : int.Parse(numberString);
        //        WeaponUI.Instance.AddItem(order, InventoryManager.instance.FindUnEquipedItem(targetWeaponID));
        //    }

        //}

        transform.position = originalPos;
        isDrag = false;
        UIManager.instance.CloseCombinePopUpUI();

    }
}
