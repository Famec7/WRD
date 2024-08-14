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
    // 마우스 땠을 떄
    public void MouseUp()
    {
        // 만약 롱클릭이 아니고 웨폰 아이디가 0보다 크면
        if (!isLongClick && weaponID > 0 )
        {
            // 착용가능한 무기가 있으면
            hasItem = GameManager.instance.useAbleWeaponCnt[weaponID-1] > 0;
            // 조합창 만들고
            UIManager.instance.CreateCombineUI(weaponID,true,isInventory,GetComponent<InventorySlot>().isEquiped);
            // 롱클릭팝업 ui 세팅하고
            LongClickPopUpUIObject = UIManager.instance.longClickPopUpUI;
            LongClickPopUpUi longClickPopUpUI = LongClickPopUpUIObject.GetComponent<LongClickPopUpUi>();
            LongClickPopUpUIObject.SetActive(true);
            longClickPopUpUI.weaponID = weaponID;
            longClickPopUpUI.inventorySlot = GetComponent<InventorySlot>();
            longClickPopUpUI._equipButton.SetActive(hasItem);
            // 만약 인벤에서 눌렀으면
            if (isInventory)
            {
                //UI 설명창 만들고 선택됐는지 확인하는 하이라이트 UI 키고 위치 조절
                UIManager.instance.CreateInventoryDescriptionUI(weaponID);
                InventoryManager.instance.inventorySelectUI.SetActive(true);
                InventoryManager.instance.inventorySelectUI.GetComponent<RectTransform>().position = transform.position;
                //장착,즐겨찾기 버튼 활성화
                longClickPopUpUI._bookmarkButton.SetActive(true);
                longClickPopUpUI._equipButton.SetActive(GameManager.instance.weaponCnt[weaponID - 1] > 0);
                // 뒤에 안눌리게 블락이미지 키고 롱클릭 팝업 UI 고정 위치에 위치
                UIManager.instance.SetActiveBlockImage(true);
                LongClickPopUpUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-420,162);
                // 추후 세팅을 위해 해당 슬롯을 longclickpopupui에 넣어줌
                longClickPopUpUI.inventorySlot = GetComponent<InventorySlot>();
            }

            if (isWeaponSlot)
            {
                //무기 슬롯이면 즐겨찾기 비활성화 하고 장착해제 버튼만 킴 그리고 위치 바꾸고 longClickPopUpUI에 weaponSlot 전달
                longClickPopUpUI._bookmarkButton.SetActive(false);
                longClickPopUpUI._equipButton.SetActive(true);
                LongClickPopUpUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(88,352);
                longClickPopUpUI.weaponSlot = transform.parent.GetComponent<WeaponSlotUI>();
                longClickPopUpUI.inventorySlot = longClickPopUpUI.weaponSlot.inventorySlot;
            }

            if (isBookmarked)
                longClickPopUpUI._bookmarkButton.SetActive(true);

            //인벤토리 아니면 부모에서 weaponid 넣어주기
            if (!isInventory)
                longClickPopUpUI.weaponID = transform.parent.GetComponent<WeaponSlotUI>().weaponID;

            // longclickpopupui bool 변수 세팅
            longClickPopUpUI.isBookmarked = isBookmarked;
            longClickPopUpUI.isInventory = isInventory;
            longClickPopUpUI.isWeaponSlot = isWeaponSlot;
            // 버튼 텍스트 바꿔주는 함수 호출
            longClickPopUpUI.SetBookmarkedButtonText(isBookmarked, isInventory,isWeaponSlot);
        }
    
        isClicked = false;
        isLongClick = false;
        elapsedTime = 0.0f;
    }

   
}
