using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponImage : MonoBehaviour
{
    // Start is called before the first frame update

    public float minClickTime = 0.3f;

    public GraphicRaycaster gr;
    private float clickTime;
    private bool isClick;

    private Vector3 originalPos;
    public bool isDrag = false;
    public bool isInventory;
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
            if ((transform.parent.gameObject.CompareTag("WeaponSlot")))
                rectTransform1.sizeDelta = new Vector2(250, 250);

            if ((transform.parent.gameObject.CompareTag("RecentEarnSlot")))
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
        transform.position = Input.mousePosition;
        var parent = transform.parent;
        parent.GetComponent<RectTransform>().SetAsLastSibling();
        parent.parent.GetComponent<RectTransform>().SetAsLastSibling();

        UIManager.instance.touchPos = Input.mousePosition;

        isDrag = true;

        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        
        var results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        
        if (results.Count <= 0) return;
      
    }

    public void ButtonDrop()
    {
        //UGUI RayCast
        var ped = new PointerEventData(null)
        {
            position = Input.mousePosition
        };
        var results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        // ������ return
        if (results.Count <= 0) return;

        GameObject targetSlotObject = null;
        
        foreach (var result in results)
        {
            if (result.gameObject.CompareTag(transform.parent.gameObject.tag)  && result.gameObject != transform.parent.gameObject)
                targetSlotObject = result.gameObject;

            if (isInventory && result.gameObject.CompareTag("InventorySlot"))
                targetSlotObject = result.gameObject;
        }
        
        if (!isInventory)
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
                    gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
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
        
        transform.position = originalPos;
        isDrag = false;
        UIManager.instance.CloseCombinePopUpUI();

    }
}
