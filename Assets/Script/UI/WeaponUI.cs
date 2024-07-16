using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : Singleton<WeaponUI>
{
    public WeaponSlotUI[] weaponSlots;
    public int weaponID;
    
    [SerializeField]
    private Transform slotParent;
    
    // Start is called before the first frame update
    void Awake()
    {
        weaponSlots = slotParent.GetComponentsInChildren<WeaponSlotUI>();
    }

    // Update is called once per frame
    protected override void Init()
    {
        return;
    }

    public void AddItem(int order, InventoryItem item)
    {
        // 만약 가득차면 리턴
        
        WeaponSlotUI targetSlot = null;
        
        if (!weaponSlots[order].hasWeapon)
        {
            targetSlot = weaponSlots[order];
        }
        
        else
            return;
        
        targetSlot.gameObject.transform.GetChild(0).GetComponent<InventorySlot>().weapon = item;

        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        //롱클릭 팝업 ui 가져와서 글자 최신화를 위해 setFavoritevbutton 함수 실행
        longClickPopUpUi.SetBookmarkedButtonText(longClickPopUpUi.isBookmarked, false, true);

        //쓰고있는 무기 배열에 추가하고 사용할 수 있는 무기 개수 배열 업데이트
        GameManager.instance.useWeapon.Add(weaponID);
        GameManager.instance.UpdateUseableWeaponCnt();

        //즐겨찾기에서 시작하고 사용가능한 무기에 해당 하는 무기 없을 시 장착 버튼 비활성화
        if (longClickPopUpUi.isBookmarked && GameManager.instance.useAbleWeaponCnt[weaponID - 1] <= 0)
            longClickPopUpUi._equipButton.SetActive(false);

        //해당 슬롯에 이미지 설정 및 id 설정
        targetSlot.hasWeapon = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
        targetSlot.weaponID = weaponID;
        targetSlot.transform.GetChild(0).GetComponent<LongClickComponenet>().weaponID = weaponID;

        

        // 즐겨찾기에서 시작했으면 해당 슬롯이 있는 인벤토리 슬롯, 인벤토리에서 시작했으면 그냥 자기 인벤토리 슬롯
        if (!longClickPopUpUi.isBookmarked)
            targetSlot.inventorySlot = longClickPopUpUi.inventorySlot;
        else
            targetSlot.inventorySlot = InventoryManager.instance.FindInventorySlot(weaponID);

        //"E" 활성화 및 해당 슬롯이 있는 슬롯 isEquiped true (다시 못끼게)
        targetSlot.inventorySlot.equipText.gameObject.SetActive(true);
        targetSlot.inventorySlot.isEquiped = true;
        
    }

    public void ChangeItem(int order, InventoryItem item)
    { 
        WeaponSlotUI targetSlot = weaponSlots[order];
       
        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        //롱클릭 팝업 ui 가져와서 글자 최신화를 위해 setFavoritevbutton 함수 실행
        longClickPopUpUi.SetBookmarkedButtonText(longClickPopUpUi.isBookmarked, false, true);

        //쓰고있는 무기 배열에 추가하고 사용할 수 있는 무기 개수 배열 업데이트
        GameManager.instance.useWeapon.Add(weaponID);
        GameManager.instance.UpdateUseableWeaponCnt();

        //즐겨찾기에서 시작하고 사용가능한 무기에 해당 하는 무기 없을 시 장착 버튼 비활성화
        if (longClickPopUpUi.isBookmarked && GameManager.instance.useAbleWeaponCnt[weaponID - 1] <= 0)
            longClickPopUpUi._equipButton.SetActive(false);

        //해당 슬롯에 이미지 설정 및 id 설정
        targetSlot.hasWeapon = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
        targetSlot.weaponID = weaponID;
        targetSlot.transform.GetChild(0).GetComponent<LongClickComponenet>().weaponID = weaponID;

        targetSlot.transform.GetChild(0).GetComponent<InventorySlot>().weapon = item;

    }

    public void RemoveItem(int[] weaponIDs)
    {
        foreach (var weaponID in weaponIDs)
        {
            foreach (var slot in weaponSlots)
            {
                if (slot.weaponID == weaponID)
                {
                    Init();
                    break;
                }
            }
        }
    }
}
