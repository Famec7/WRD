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
    void Start()
    {
        weaponSlots = slotParent.GetComponentsInChildren<WeaponSlotUI>();
    }

    // Update is called once per frame
    protected override void Init()
    {
        return;
    }

    public void AddItem(int order)
    {
        bool isSlotFull = true;
        WeaponSlotUI targetSlot = null;
        
        if (!weaponSlots[order].hasWeapon)
        {
            targetSlot = weaponSlots[order];
            isSlotFull = false;
        }
        
        else
            return;
        
        string weaponIconPath = "WeaponIcon/" + weaponID.ToString();
        
        InventoryItem item = new InventoryItem
        {
            image =  ResourceManager.Instance.Load<Sprite>(weaponIconPath)
        };
        item.AssignWeapon(weaponID);

        GameManager.instance.useWeapon.Add(weaponID);
        GameManager.instance.UpdateUseableWeaponCnt();

        if (isSlotFull)
            targetSlot = weaponSlots[0];

        targetSlot.hasWeapon = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
        targetSlot.weaponID = weaponID;
        
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
