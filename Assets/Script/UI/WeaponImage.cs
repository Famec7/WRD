using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponImage : MonoBehaviour
{
    // Start is called before the first frame update

    public float minClickTime = 0.3f;

    private float clickTime;
    private bool isClick;

    public Vector3 originalPos;
    public bool isDrag = false;
    public bool isSlide = false;
    public bool isInventory;
    public bool isBookmarked;
    private RectTransform rectTransform;
    private LongClickComponenet longClickComponenet;

    private Sprite mySprite;

    private Canvas canvas;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = transform.position;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        longClickComponenet = GetComponent<LongClickComponenet>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDrag && !isSlide)
        {
            rectTransform.anchoredPosition = originalPos;
        }
        else
        {
            float sizeOffset = transform.parent.gameObject.name == "MainWeaponSlot" ? 1.5f : 1f;
            if ((transform.parent.gameObject.CompareTag("WeaponSlot")))
                rectTransform.sizeDelta = new Vector2(120, 120) * sizeOffset;

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

        FollowMouse();

        var parent = transform.parent;
        parent.GetComponent<RectTransform>().SetAsLastSibling();
        parent.parent.GetComponent<RectTransform>().SetAsLastSibling();
        
        if (isBookmarked)
            parent.parent.parent.GetComponent<RectTransform>().SetAsLastSibling();


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
                
                WeaponManager.Instance.RemoveWeapon(transform.parent.GetComponent<WeaponSlotUI>().SlotIndex);
                WeaponManager.Instance.AddWeapon(targetSlot.SlotIndex, weaponID);
            }
        }

        if (gameObject.transform.parent.CompareTag("BookMarkedSlot"))
        {
            if (targetSlotObject.gameObject.CompareTag("WeaponSlot"))
            {
                int targetWeaponID = targetSlotObject.GetComponent<WeaponSlotUI>().weaponID;
                int myWeaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;
                LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
                InventorySlot targetInventorySlot = targetSlotObject.GetComponent<WeaponSlotUI>().inventorySlot;
                WeaponSlotUI targetWeaponSlot = targetSlotObject.GetComponent<WeaponSlotUI>();

                if (targetSlotObject.GetComponent<WeaponSlotUI>().hasWeapon)
                {
                    longClickPopUpUi.SetLongClickPopUpUI(targetWeaponID, false, false, false, targetInventorySlot, targetWeaponSlot);
                    longClickPopUpUi.UnEuqip();

                    BookMakredSlotUI.Instance.RemoveItem(BookMakredSlotUI.Instance.GetSlotWithWeaponID(transform.parent.gameObject.GetComponent<WeaponSlotUI>().weaponID).transform.GetChild(0).GetComponent<InventorySlot>());
                    BookMakredSlotUI.Instance.weaponID = targetWeaponID;
                    BookMakredSlotUI.Instance.AddItem(int.Parse(transform.parent.name));
                }

                string numberString = Regex.Replace(targetSlotObject.gameObject.name, @"\D", "");
                int order = numberString == "" ? 4: int.Parse(numberString);

                longClickPopUpUi.inventorySlot = InventoryManager.instance.FindInventorySlot(myWeaponID);
                WeaponUI.Instance.weaponID = myWeaponID;
                WeaponUI.Instance.AddItem(order, InventoryManager.instance.FindUnEquipedItem(myWeaponID));
                transform.parent.GetComponent<WeaponSlotUI>().ChangeWeaponUseable();
            }
                      
        }
        if (gameObject.transform.parent.CompareTag("WeaponSlot") && targetSlotObject.gameObject.CompareTag("BookMarkedSlot"))
        {
            int targetWeaponID = targetSlotObject.GetComponent<WeaponSlotUI>().weaponID;
            int myWeaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;
            LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();

            if (targetSlotObject.GetComponent<WeaponSlotUI>().hasWeapon && GameManager.Instance.useAbleWeaponCnt[targetWeaponID - 1] > 0)
            {
                longClickPopUpUi.SetLongClickPopUpUI(myWeaponID, false, false, false, transform.parent.GetComponent<WeaponSlotUI>().inventorySlot, transform.parent.GetComponent<WeaponSlotUI>());
                longClickPopUpUi.UnEuqip();
               
                string numberString = Regex.Replace(transform.parent.gameObject.name, @"\D", "");
                int order = numberString == "" ? 4 : int.Parse(numberString);

                longClickPopUpUi.inventorySlot = InventoryManager.instance.FindInventorySlot(targetWeaponID);
                WeaponUI.Instance.weaponID = targetWeaponID;
                WeaponUI.Instance.AddItem(order, InventoryManager.instance.FindUnEquipedItem(targetWeaponID));
                targetSlotObject.GetComponent<WeaponSlotUI>().ChangeWeaponUseable();
            }

            if (!BookMakredSlotUI.Instance.IsDuplicatedID(myWeaponID))
            {
                BookMakredSlotUI.Instance.weaponID = myWeaponID;
                BookMakredSlotUI.Instance.AddItem(int.Parse(targetSlotObject.name));
            }

        }

        transform.position = originalPos;
        isDrag = false;

        UIManager.instance.CloseCombinePopUpUI();
        UIManager.instance.longClickPopUpUI.SetActive(false);
    }

    public void FollowMouse()
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }

        else
        {
            Debug.LogError("화면 좌표를 월드 좌표로 변환하는 데 실패했습니다.");
        }
    }


}
