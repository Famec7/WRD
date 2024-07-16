using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public static InventoryManager instance;

    public List<InventoryItem> items;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Transform notHeldParent;
    [SerializeField]
    private InventorySlot[] slots;

    [SerializeField]
    AllShowInventorySlot[] notHeldslots;

    public GameObject inventorySlotsPrefab;
    public GameObject allShowInventoryContent;
    public GameObject normalInventoryContent;
    public GameObject allShowButton;
    public GameObject classShowButton;
    public GameObject inventorySelectUI;
    public GameObject allShowClassSelectButtons;
    public GameObject classShowClassSelectButtons;
    public GameObject[] sortButtons;
    

    public Toggle allShowToggle;

    bool isAllShow = false;
    public bool isLastAllShow = false;
    public bool isClassSorted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        slots = slotParent.GetComponentsInChildren<InventorySlot>();
        notHeldslots = notHeldParent.GetComponentsInChildren<AllShowInventorySlot>();
        items = new List<InventoryItem>();
        
        string path = "WeaponIcon/" + 7;
        InventoryItem item = new InventoryItem
        {
            image =  ResourceManager.Instance.Load<Sprite>(path)
        };
        item.AssignWeapon(7);
        AddItem(item);
        WeaponUI.Instance.weaponSlots[4].transform.GetChild(0).GetComponent<InventorySlot>().weapon = item;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetAsLastSibling();
        isAllShow = allShowInventoryContent.activeSelf;
    }

    public void SyncWeaponSlotInventorySlot()
    {
        for (int j = 0; j < slots.Length; j++)
        {
            if (slots[j].weapon == null)
            {
                slots[j].isEquiped = false;
                slots[j].equipText.gameObject.SetActive(false);
                continue;
            };

            for (int weapnUIIDX = 0; weapnUIIDX < UIManager.instance.weaponSlotUI.Length; weapnUIIDX++)
            {
                WeaponSlotUI weaponSlotUI = UIManager.instance.weaponSlotUI[weapnUIIDX];

                if (weaponSlotUI.hasWeapon)
                {
                    if (weaponSlotUI.transform.GetChild(0).GetComponent<InventorySlot>().weapon == slots[j].weapon)
                    {
                        weaponSlotUI.inventorySlot.equipText.gameObject.SetActive(true);
                        weaponSlotUI.inventorySlot.isEquiped = true;
                        weaponSlotUI.inventorySlot = slots[j];
                        break;
                    }
                }

            }
        }
    }
    public void FreshSlot()
    {
        int i = 0;
        int j = 0;
        for (; i < items.Count && j < slots.Length; i++)
        {
            slots[j].equipText.gameObject.SetActive(false);

            //for (int weapnUIIDX = 0; weapnUIIDX < UIManager.instance.weaponSlotUI.Length; weapnUIIDX++)
            //{
            //    WeaponSlotUI weaponSlotUI = UIManager.instance.weaponSlotUI[weapnUIIDX];
            //    if (weaponSlotUI.hasWeapon)
            //    {

            //        if (weaponSlotUI.inventorySlot.weapon == items[i] && slots[j].isEquiped)
            //        {
            //            weaponSlotUI.inventorySlot.equipText.gameObject.SetActive(true);
            //            weaponSlotUI.inventorySlot.isEquiped = true;
            //            weaponSlotUI.inventorySlot = slots[j];
            //            break;
            //        }
            //    }
          
            //}

            slots[j].weapon = items[i];
            slots[j].hasItem = true;
            slots[j].gameObject.GetComponent<LongClickComponenet>().weaponID = items[i].data.ID;
            
            j++;
        }
        for (; j < slots.Length; j++)
        {
            slots[j].weapon = null;
            slots[j].hasItem = false;
            slots[j].gameObject.GetComponent<LongClickComponenet>().weaponID = -1;
        }

        SyncWeaponSlotInventorySlot();

    }

    public void InventorySort(int grade)
    {
        int i = 0;
        int j = 0;
        string cmp="";
        List<int> tmpList = new List<int>(GameManager.instance.useWeapon);
        switch(grade)
        {
            case 0:
                cmp = "unnormal";
                break;
            case 1:
                cmp = "rare";
                break;
            case 2:
                cmp = "epic";
                break;
            case 3:
                cmp = "legendary";
                break;
            case 4:
                cmp = "myth";
                break;
        }
        
        for (; i < items.Count && j < slots.Length; i++)
        {
            if (items[i].data.WeaponClass != cmp)
                continue;
                   
            slots[j].weapon = items[i];
            slots[j].gameObject.GetComponent<LongClickComponenet>().weaponID = items[i].data.ID;
            j++;
        }

        for (; j < slots.Length; j++)
            slots[j].Init();

        SyncWeaponSlotInventorySlot();
    }

    public void AddItem(InventoryItem _item, bool refresh = true)
    {
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot();
        }
        else
        {
            GameObject newSlots = Instantiate(inventorySlotsPrefab,slotParent);

        }
    }

    public void CloseButton()  
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ChangeAllShowToggle() 
    {
        allShowInventoryContent.SetActive(allShowToggle.isOn);
        normalInventoryContent.SetActive(!allShowToggle.isOn);
        bool isSelect = false;
        
        foreach (ClassSelectButton button in allShowClassSelectButtons.GetComponentsInChildren<ClassSelectButton>())
        {
            if (button.select)
            {
                NotHeldSort(button.grade);
                InventorySort(button.grade);
                isSelect = true;
            }   
        }

        foreach (ClassSelectButton button in classShowClassSelectButtons.GetComponentsInChildren<ClassSelectButton>())
        {
            if (button.select)
            {
                NotHeldSort(button.grade);
                InventorySort(button.grade);
                isSelect = true;
            }
        }

        if (!isSelect)
        {
            NotHeldSort(-1);
            FreshSlot();
        }

        if (allShowToggle.isOn)
        {
            for (int i = 0; i < sortButtons.Length; i++)
            {
                sortButtons[i].gameObject.SetActive(false);
            }
        }

        isLastAllShow = allShowToggle.isOn;
    }

    public void ClickAllShowButton()
    {
        if (!isLastAllShow)
        {
            classShowClassSelectButtons.SetActive(false);
        }

        allShowClassSelectButtons.SetActive(false);
        
        allShowButton.GetComponent<Image>().color = new Color32(255, 223, 0, 255);
        classShowButton.GetComponent<Image>().color = new Color32(173, 173, 173, 255);
        InventoryManager.instance.FreshSlot();

        isClassSorted = false;
        
        if (!allShowToggle.isOn)
        {
            for (int i = 0; i < sortButtons.Length; i++)
            {
                sortButtons[i].gameObject.SetActive(true);
            }
        }

        NotHeldSort(-1);
    }

    public void ClickClassShowButton()
    {
        allShowClassSelectButtons.SetActive(true);
        classShowClassSelectButtons.SetActive(true);
        isClassSorted = true;

        allShowButton.GetComponent<Image>().color = new Color32(173, 173, 173, 255);
        classShowButton.GetComponent<Image>().color = new Color32(255, 223, 0, 255);
       
        foreach(ClassSelectButton button in classShowClassSelectButtons.GetComponentsInChildren<ClassSelectButton>())
        {
            if (button.select)
            {
                InventorySort(button.grade);
            }
        }

        foreach (ClassSelectButton button in allShowClassSelectButtons.GetComponentsInChildren<ClassSelectButton>())
        {
            if (button.select)
            {
                NotHeldSort(button.grade);
            }
        }

        for (int i = 0; i < sortButtons.Length; i++)
        {
            sortButtons[i].gameObject.SetActive(false);
        }

    }

    public bool RemoveItem(int[] itemIDs, int mainWeaponID, InventoryItem item)
    {
        bool returnValue = false;
        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        InventorySlot pressSlot = null;

        if (longClickPopUpUi.inventorySlot != null)
            pressSlot = longClickPopUpUi.inventorySlot;
        if (longClickPopUpUi.weaponSlot != null && longClickPopUpUi.weaponSlot.inventorySlot != null)
            pressSlot = longClickPopUpUi.weaponSlot.inventorySlot;

        Dictionary<int, int> materialCounts = new Dictionary<int, int>();
        foreach (int itemID in itemIDs) 
        {
            if (materialCounts.ContainsKey(itemID))
                materialCounts[itemID]++;
            else
                materialCounts[itemID] = 1;
        }
        //WeaponSlotUI targetSlot = UIManager.instance.Wea;
        foreach (int i in itemIDs)
        {
            var itemToRemove = items.Find(item => item.data.ID == i);
            
            foreach (var slot in slots)
            {
                if (!slot.hasItem) continue;

                if (pressSlot == null)
                {
                    if (itemToRemove == slot.weapon && slot.isEquiped && GameManager.instance.weaponCnt[i - 1] + materialCounts[i] == materialCounts[i])
                    {
                        UnEquipMaterialWeapon(slot, itemToRemove, pressSlot, materialCounts);
                        materialCounts[i]--;
                    }
                }

                else
                {
                    if (pressSlot.isEquiped && pressSlot.weapon == itemToRemove && !returnValue && slot == pressSlot)
                    {
                        for (int j = 0; j < UIManager.instance.weaponSlotUI.Length; j++)
                        {
                            var weaponSlot = UIManager.instance.weaponSlotUI[j];

                            if (slot == weaponSlot.inventorySlot)
                            {
                                WeaponUI.Instance.ChangeItem(j,item);
                                materialCounts[i]--;
                                //targetSlot = weaponSlot;
                                pressSlot = weaponSlot.inventorySlot;
                                returnValue = true;
                            }
                        }
                    }

                    if (itemToRemove != pressSlot.weapon && itemToRemove == slot.weapon && slot.isEquiped && GameManager.instance.useAbleWeaponCnt[i] < materialCounts[i])
                    {
                        UnEquipMaterialWeapon(slot, itemToRemove, pressSlot, materialCounts);
                        materialCounts[i]--;
                    }

                }
            }

            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                GameManager.instance.useAbleWeaponCnt[item.data.ID - 1]++;
            }

           
        }

        //if (pressSlot != null)
        //{
        //    foreach (var slot in slots)
        //    {
        //        if (slot.hasItem) continue;
        //        targetSlot.inventorySlot = slot;
        //        targetSlot.inventorySlot.isEquiped = true;
        //        break;
        //    }
        //}
        FreshSlot();
        return returnValue;
    }

    public void UnEquipMaterialWeapon(InventorySlot slot , InventoryItem itemToRemove, InventorySlot pressSlot, Dictionary<int,int>materialCounts)
    {
        slot.isEquiped = false;

        for (int j = 0; j < UIManager.instance.weaponSlotUI.Length; j++)
        {
            var weaponSlot = UIManager.instance.weaponSlotUI[j];
            if (slot == weaponSlot.inventorySlot)
            {
                weaponSlot.Init();
                slot.hasItem = false;
            }
        }
    }

    public void NotHeldSort(int grade)
    {
        string cmp = "";

        switch (grade)
        {
            case 0:
                cmp = "unnormal";
                break;
            case 1:
                cmp = "rare";
                break;
            case 2:
                cmp = "epic";
                break;
            case 3:
                cmp = "legendary";
                break;
            case 4:
                cmp = "myth";
                break;
        }

        int i = 0 , j= 5;

        for (; i < notHeldslots.Length && j <WeaponDataManager.Instance.Database.GetWeaponDataCount(); j++)
        {
            var data = WeaponDataManager.Instance.GetWeaponData(j + 1);

            if (data.WeaponClass == cmp || grade == -1)
            {
                GameObject notHeldSlotItem = notHeldslots[i].transform.GetChild(0).gameObject;

                string path = "WeaponIcon/" + data.ID.ToString();

                notHeldslots[i].gameObject.SetActive(true);
                notHeldslots[i].GetComponent<AllShowInventorySlot>().weaponID = data.ID;

                notHeldSlotItem.GetComponent<InventorySlot>().weapon = new InventoryItem();
                notHeldSlotItem.GetComponent<InventorySlot>().weapon.AssignWeapon(data.ID);
                notHeldSlotItem.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                
                bool isEquiped = GameManager.instance.isUsing(j+1);
                notHeldSlotItem.GetComponent<InventorySlot>().isEquiped = isEquiped;
                notHeldSlotItem.GetComponent<InventorySlot>().equipText.gameObject.SetActive(isEquiped);
                notHeldSlotItem.GetComponent<LongClickComponenet>().weaponID = data.ID;
                i++;
            }
         
        }

        for (;  i < notHeldslots.Length; i++)
        {
            notHeldslots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            notHeldslots[i].gameObject.SetActive(false);
            notHeldslots[i].transform.GetChild(0).gameObject.GetComponent<LongClickComponenet>().weaponID = 0;
        }

    }

    public void AllClassSort()
    {
        items.Sort((item1,item2) => (item2.data.ID).CompareTo(item1.data.ID));
        FreshSlot();
    }

    public void AllRecentSort()
    {
        items.Sort((item1, item2) => (item2.earnTime).CompareTo(item1.earnTime));
        FreshSlot();
    }
    
    public WeaponTuple<WeaponTier, int> GetHighestTier()
    {
        WeaponTier highest = WeaponTier.UNNORMAL;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].data.tier > highest)
                highest = items[i].data.tier;
        }
        
        return new WeaponTuple<WeaponTier, int>(highest, items.FindAll(item => item.data.tier == highest).Count);
    }

    public InventorySlot FindInventorySlot(int id)
    {
        InventorySlot targetSlot = null;

        foreach (InventorySlot slot in slots)
        {
            if (slot.hasItem)
            {
                if (!slot.isEquiped && slot.weapon.data.ID == id)
                {
                    targetSlot = slot;
                    break;
                }
            }
        }

        return targetSlot;

    }

    public InventoryItem FindUnEquipedItem(int id)
    {
        InventoryItem findItem = null;

        foreach (InventorySlot slot in slots)
        {
            if (!slot.isEquiped  && slot.hasItem)
            {
                if (slot.weapon.data.ID == id)
                findItem = slot.weapon;
            }
        }

        return findItem;
    }
}

