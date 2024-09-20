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
        if (item == null) return;

        WeaponSlotUI targetSlot = null;
        
        if (weaponSlots[order].hasWeapon)
        {
            WeaponManager.Instance.RemoveWeapon(order);
            GameManager.instance.RemoveUseWeaponList(weaponSlots[order].weaponID);
            weaponSlots[order].Init();
        }

        targetSlot = weaponSlots[order];
        targetSlot.gameObject.transform.GetChild(0).GetComponent<InventorySlot>().weapon = item;

        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        longClickPopUpUi.SetBookmarkedButtonText(longClickPopUpUi.isBookmarked, false, true);

        GameManager.instance.useWeapon.Add(weaponID);
        GameManager.instance.UpdateUseableWeaponCnt();

        if (longClickPopUpUi.isBookmarked && GameManager.instance.useAbleWeaponCnt[weaponID - 1] <= 0)
            longClickPopUpUi._equipButton.SetActive(false);

        targetSlot.hasWeapon = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
        targetSlot.weaponID = weaponID;
        targetSlot.transform.GetChild(0).GetComponent<LongClickComponenet>().weaponID = weaponID;

        

        if (!longClickPopUpUi.isBookmarked)
            targetSlot.inventorySlot = longClickPopUpUi.inventorySlot;
        else
            targetSlot.inventorySlot = InventoryManager.instance.FindInventorySlot(weaponID);

        targetSlot.inventorySlot.equipText.gameObject.SetActive(true);
        targetSlot.inventorySlot.isEquiped = true;
        InventoryManager.instance.SyncWeaponSlotInventorySlot();
        
        WeaponManager.Instance.AddWeapon(order, weaponID);
    }

    public void ChangeItem(int order, InventoryItem item)
    { 
        WeaponSlotUI targetSlot = weaponSlots[order];
       
        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        //��Ŭ�� �˾� ui �����ͼ� ���� �ֽ�ȭ�� ���� setFavoritevbutton �Լ� ����
        longClickPopUpUi.SetBookmarkedButtonText(longClickPopUpUi.isBookmarked, false, true);

        //�����ִ� ���� �迭�� �߰��ϰ� ����� �� �ִ� ���� ���� �迭 ������Ʈ
        GameManager.instance.useWeapon.Add(weaponID);

        //���ã�⿡�� �����ϰ� ��밡���� ���⿡ �ش� �ϴ� ���� ���� �� ���� ��ư ��Ȱ��ȭ
        if (longClickPopUpUi.isBookmarked && GameManager.instance.useAbleWeaponCnt[weaponID - 1] <= 0)
            longClickPopUpUi._equipButton.SetActive(false);

        //�ش� ���Կ� �̹��� ���� �� id ����
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
