using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecentEarnUI : MonoBehaviour
{
    public WeaponSlotUI[] weaponSlots;
    public WeaponSlotUI[] weaponUIWeaponSlots;

    [SerializeField]
    private Transform slotParent;

    public GameObject WeaponUI;
    // Start is called before the first frame update
    void Start()
    {
        
        weaponSlots = slotParent.GetComponentsInChildren<WeaponSlotUI>();
        weaponUIWeaponSlots = WeaponUI.GetComponentsInChildren<WeaponSlotUI>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void AddItem(InventoryItem _item)
    {
        bool isSlotFull = true;
        WeaponSlotUI targetSlot = null;
        foreach(var slot in weaponSlots) { 
            if (!slot.hasWeapon)
            {
                targetSlot = slot;
                isSlotFull = false;
                break;
            }    
        }

        if (isSlotFull)
            targetSlot = weaponSlots[0];

        targetSlot.hasWeapon = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = _item.image;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
        targetSlot.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => UIManager.instance.CreateCombineUI(_item.data.id));
        targetSlot.weaponID = _item.data.id;
        
    }

    public void RemoveItem(int[] itemCode)
    {
        List<int> notRemoveItem = new List<int>();

        foreach (int i in itemCode)
        {
            bool notRemove = true;

           foreach(var slot in weaponSlots)
           {
                if (slot.weaponID == i)
                {
                   slot.Init();
                   notRemove = false;
                   break;
                }
           }

           if (notRemove)
           {
                notRemoveItem.Add(i);
           }
        }

        foreach(int i in notRemoveItem)
        {
            foreach (var slot in weaponUIWeaponSlots)
            {
                if (slot.weaponID == i)
                {
                    slot.Init();
                    break;
                }
            }
        }

    }
}
