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

    void Start()
    {
        rectTransform1 = GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        originalPos = transform.position;
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

        isDrag = true;

        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        // 없으면 return
        if (results.Count <= 0) return;

        foreach (var result in results)
        {
            if ((result.gameObject.CompareTag("WeaponSlot")))
                GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);

            if ((result.gameObject.CompareTag("RecentEarnSlot")))
                GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        }
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
        // 없으면 return
        if (results.Count <= 0) return;

        GameObject targetSlotObject = null;
        //slot찾아서 GameObject 할당시키기
        foreach (var result in results)
        {
            if ((result.gameObject.CompareTag("WeaponSlot") || result.gameObject.CompareTag("RecentEarnSlot")) && result.gameObject != transform.parent.gameObject)
                targetSlotObject = result.gameObject;

            if ((result.gameObject.CompareTag("WeaponSlot")))
                GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);

            if (isInventory && result.gameObject.CompareTag("InventorySlot"))
                targetSlotObject = result.gameObject;
        }
        //내 이미지랑 해당이미지 변경하고 이것저것 멤버변수 바꿔줌
        if (!isInventory)
        {
            if (targetSlotObject != null)
            {
                WeaponSlotUI targetSlot = targetSlotObject.GetComponent<WeaponSlotUI>();
                Image targetSlotImage = targetSlot.transform.GetChild(0).gameObject.GetComponent<Image>();

                int weaponID = transform.parent.gameObject.GetComponent<WeaponSlotUI>().weaponID;
                int targetweaponID = targetSlot.weaponID;
                bool alreadyHasWeapon = targetSlot.GetComponent<WeaponSlotUI>().hasWeapon;

                Sprite targetSprite = targetSlotImage.sprite;
                Sprite mySprite = gameObject.GetComponent<Image>().sprite;

                //타겟 슬롯의 변수 변경
                targetSlot.hasWeapon = true;
                targetSlot.weaponID = weaponID;
                targetSlotImage.enabled = true;

                if (gameObject.GetComponent<InventorySlot>().weapon == null)
                {
                    InventoryItem item = new InventoryItem
                    {
                        image = gameObject.GetComponent<Image>().sprite
                    };
                    item.AssignWeapon(weaponID);

                    gameObject.GetComponent<InventorySlot>().weapon = item;
                }

                targetSlot.transform.GetChild(0).gameObject.GetComponent<InventorySlot>().weapon =
                    gameObject.GetComponent<InventorySlot>().weapon;
                targetSlotImage.sprite = mySprite;
                targetSlotImage.color = new Color32(255, 255, 255, 255);

              //  gameObject.GetComponent<Button>().onClick.RemoveAllListeners(); // 현재 오브젝트 리스터 삭제

                if (alreadyHasWeapon)
                {
                    gameObject.GetComponent<Image>().sprite = targetSprite;
                    gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    transform.parent.gameObject.GetComponent<WeaponSlotUI>().weaponID = targetweaponID;
                    gameObject.GetComponent<Image>().enabled = true;
                    transform.parent.gameObject.GetComponent<WeaponSlotUI>().hasWeapon = true;

                    // targetSlot.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    // targetSlot.transform.GetChild(0).gameObject.GetComponent<Button>().onClick
                    //     .AddListener(() => UIManager.instance.CreateCombineUI(targetweaponID));
                }

                else
                    transform.parent.GetComponent<WeaponSlotUI>().Init();

                UIManager.instance.touchPos = Input.mousePosition;
                // targetSlot.transform.GetChild(0).gameObject.GetComponent<Button>().onClick
                //     .AddListener(() => UIManager.instance.CreateCombineUI(weaponID));
            }
        }
        else
        {
            if (targetSlotObject != null)
            {
                
            }
        }
        transform.position = originalPos;
        isDrag = false;

    }
}
