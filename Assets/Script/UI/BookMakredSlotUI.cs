using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMakredSlotUI : Singleton<BookMakredSlotUI>
{
    public WeaponSlotUI[] weaponSlots;

    [SerializeField]
    private Transform slotParent;

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

    public void AddItem(int weaponID)
    {
        bool isSlotFull = true;
        WeaponSlotUI targetSlot = null;
        
        foreach(var slot in weaponSlots) 
        { 
            if (!slot.hasWeapon)
            {
                targetSlot = slot;
                isSlotFull = false;
                break;
            }    
        }
        string weaponIconPath = "WeaponIcon/" + weaponID.ToString();
        
        InventoryItem item = new InventoryItem
        {
            image =  ResourceManager.Instance.Load<Sprite>(weaponIconPath)
        };
        item.AssignWeapon(weaponID);
        
        targetSlot.hasWeapon = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
        targetSlot.weaponID = weaponID;
        targetSlot.transform.GetChild(0).GetComponent<LongClickComponenet>().weaponID = weaponID;
    }

    public void RemoveItem(int[] itemCode)
    {
        

    }
}
