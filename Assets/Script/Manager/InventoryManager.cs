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
    
    public ScrollRect scrollRect;

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
        slots[0].isEquiped = true;
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
            slots[j].isEquiped = false;
            slots[j].equipText.gameObject.SetActive(false);

            if (slots[j].weapon == null) continue;
            
            for (int weapnUIIDX = 0; weapnUIIDX < UIManager.instance.weaponSlotUI.Length; weapnUIIDX++)
            {
                WeaponSlotUI weaponSlotUI = UIManager.instance.weaponSlotUI[weapnUIIDX];

                if (weaponSlotUI.hasWeapon)
                {
                    if (weaponSlotUI.transform.GetChild(0).GetComponent<InventorySlot>().weapon == slots[j].weapon)
                    {
                        weaponSlotUI.inventorySlot = slots[j];
                        weaponSlotUI.inventorySlot.equipText.gameObject.SetActive(true);
                        weaponSlotUI.inventorySlot.isEquiped = true;
                        break;
                    }
                }

            }
        }
    }
    public void FreshSlot(bool isSync = true)
    {
        int i = 0;
        int j = 0;
        for (; i < items.Count && j < slots.Length; i++)
        {
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

        if(isSync)
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

            if(refresh)
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

        normalInventoryContent.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(20,-200);
        allShowInventoryContent.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(20, -200);
    }

    public bool RemoveItem(List<int> itemIDs, int mainWeaponID, InventoryItem item)
    {
        bool returnValue = false;
        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        InventorySlot pressSlot = null;

        if (longClickPopUpUi.inventorySlot != null)
            pressSlot = longClickPopUpUi.inventorySlot;
        if (longClickPopUpUi.weaponSlot != null && longClickPopUpUi.weaponSlot.inventorySlot != null)
            pressSlot = longClickPopUpUi.weaponSlot.inventorySlot;

        if (pressSlot == null)
        {
            InventorySlot equippedSlot = null;
            InventorySlot firstMatchingSlot = null;

            foreach (var slot in slots)
            {
                if (!slot.hasItem) continue;

                if (slot.weapon.data.ID != itemIDs[0]) continue;

                if (slot.isEquiped)
                {
                    equippedSlot = slot;
                    break;
                }

                if (firstMatchingSlot == null)
                {
                    firstMatchingSlot = slot;
                }

            }
            pressSlot = equippedSlot ?? firstMatchingSlot;
        }

        if (pressSlot != null)
        {
            // ���࿡ �������� �����
            if (pressSlot.isEquiped)
            {
                for (int j = 0; j < UIManager.instance.weaponSlotUI.Length; j++)
                {
                    var weaponSlot = UIManager.instance.weaponSlotUI[j];

                    if (pressSlot == weaponSlot.inventorySlot)
                    {
                        // �κ��丮 �����ۿ��� ���� ������� ���� ����
                        items.Remove(pressSlot.weapon);
                        //����UI���� ������ �ٲٱ�
                        WeaponUI.Instance.ChangeItem(j, item);
                        // ��� ����Ʈ���� ����
                        GameManager.instance.weaponCnt[itemIDs[0] - 1]--;

                        itemIDs.Remove(itemIDs[0]);
                        break;
                    }
                }
            }
        }

        foreach (int i in itemIDs)
        {
            InventoryItem removeToItem = null;
            if (i>5)
            {
                
                // �� �������� ��ᰡ ���� ��
                if (GameManager.instance.useAbleWeaponCnt[i-1] > 0)
                {
                    foreach (var slot in slots)
                    {
                        if (!slot.hasItem) continue;

                        if (!slot.isEquiped && slot.weapon.data.ID == i)
                        {
                            removeToItem = slot.weapon;
                            break;
                        }
                    }
                    GameManager.instance.useAbleWeaponCnt[i-1]--;
                }
                // �������� ����� ��
                else
                {
                    for (int j = 0; j < UIManager.instance.weaponSlotUI.Length; j++)
                    {
                        var weaponSlot = UIManager.instance.weaponSlotUI[j];

                        if (!weaponSlot.hasWeapon) continue;
                        removeToItem = weaponSlot.transform.GetChild(0).GetComponent<InventorySlot>().weapon;

                        if (removeToItem.data.ID == i)
                        {
                            weaponSlot.Init();
                            break;
                        }
                    }
                }
            }

            items.Remove(removeToItem);
            GameManager.instance.weaponCnt[i - 1]--;
        }
        
        FreshSlot(pressSlot != null);
        return returnValue;
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
                
                bool isEquiped = GameManager.instance.IsUsing(j+1);
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

    public void CreateAllItem()
    {
        for (int i = 5; i < 31; i++)
        {

            string weaponIconPath = "WeaponIcon/" + i.ToString();

            InventoryItem item = new InventoryItem
            {
                image = ResourceManager.Instance.Load<Sprite>(weaponIconPath)
            };
            item.AssignWeapon(i);


            GameManager.instance.weaponCnt[i - 1]++;
            GameManager.instance.UpdateUseableWeaponCnt();
            InventoryManager.instance.AddItem(item, false);
        }

        BookMakredSlotUI.Instance.UpdateAllSlot();

    }
}

