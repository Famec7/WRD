using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMakredSlotUI : Singleton<BookMakredSlotUI>
{
    public WeaponSlotUI[] weaponSlots;

    [SerializeField]
    private Transform slotParent;

    public int weaponID;

    // Start is called before the first frame update
    void Start()
    {
        weaponSlots = slotParent.GetComponentsInChildren<WeaponSlotUI>();
    }

    // Update is called once per frame
    protected override void Init()
    {
        return;
    }

    public void AddItem(int order = -1)
    {
        bool isSlotFull = true;
        WeaponSlotUI targetSlot = null;

        // Filled from the front if order is -1
        if (order != -1)
        {
            if (!weaponSlots[order].hasWeapon)
            {
                targetSlot = weaponSlots[order];
                isSlotFull = false;
            }
            else
                return;
        }

        //  If the order is not -1, it enters the selected slot
        else
        {
            foreach (var slot in weaponSlots)
            {
                if (!slot.hasWeapon)
                {
                    targetSlot = slot;
                    isSlotFull = false;
                    break;
                }
            }
        }

        string weaponIconPath = "WeaponIcon/" + WeaponDataManager.Instance.GetWeaponData(weaponID).num.ToString();

        InventoryItem item = new InventoryItem
        {
            image = ResourceManager.Instance.Load<Sprite>(weaponIconPath)
        };
        item.AssignWeapon(weaponID);

        targetSlot.hasWeapon = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;

        targetSlot.weaponID = weaponID;
        targetSlot.transform.GetChild(0).GetComponent<LongClickComponenet>().weaponID = weaponID;
        targetSlot.transform.GetChild(0).GetComponent<InventorySlot>().weapon =item;
        targetSlot.ChangeWeaponUseable();
    }

    public WeaponSlotUI GetSlotWithWeaponID(int id)
    {
        WeaponSlotUI result = null;

        foreach(var slot in weaponSlots)
        {
            if(slot.weaponID == id)
            {
                result = slot;
                break;
            }
        }

        return result;

    }

    public void RemoveItem(InventorySlot slot)
    {

        GameObject targetSlot = slot.gameObject;
        slot.transform.parent.GetComponent<WeaponSlotUI>().Init();
        targetSlot.GetComponent<LongClickComponenet>().weaponID = 0;
        targetSlot.GetComponent<Image>().enabled = false; ;
        targetSlot.GetComponent<Image>().sprite = null;

        UIManager.instance.CloseCombinePopUpUI();
    }


    public void UpdateAllSlot()
    {
        foreach (WeaponSlotUI slot in weaponSlots)
        {
            if (slot.weaponID > 0)
                slot.ChangeWeaponUseable(); ;
        }
         
    }

    public bool IsDuplicatedID(int weaponID)
    {
        foreach(WeaponSlotUI slot in weaponSlots)
        {
            if (slot.weaponID == weaponID)
                return true;
        }

        return false;
    }
}
