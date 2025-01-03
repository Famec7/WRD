using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEditor.VersionControl;
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
    public GameObject WeaponPickerConfirmPopUp;

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
        
        string path = "WeaponIcon/" + 201;
        InventoryItem item = new InventoryItem
        {
            image =  ResourceManager.Instance.Load<Sprite>(path)
        };
        item.AssignWeapon(6);
        AddItem(item);
        WeaponUI.Instance.weaponSlots[4].transform.GetChild(0).GetComponent<InventorySlot>().weapon = item;
        slots[0].isEquiped = true;

        WeaponManager.Instance.AddWeapon(4, 6);
        
#if ADD_ALL_ITEM
        CreateAllItem();
#endif
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
        List<int> tmpList = new List<int>(GameManager.Instance.useWeapon);
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
        {
            slots[j].Init();
            slots[j].gameObject.GetComponent<LongClickComponenet>().weaponID = 0;
        }

        SyncWeaponSlotInventorySlot();
    }

    public void AddItem(InventoryItem _item, bool refresh = true)
    {
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            if (refresh)
                FreshSlot();
        }
        else
        {
            // Instantiate new GameObject which contains multiple slots (5 in this case)
            GameObject newSlots = Instantiate(inventorySlotsPrefab, slotParent);

            // Get all InventorySlot components from the newly instantiated GameObject's children
            InventorySlot[] newInventorySlots = newSlots.GetComponentsInChildren<InventorySlot>();

            if (newInventorySlots.Length > 0)
            {
                // Resize the slots array to accommodate the new slots
                InventorySlot[] newSlotsArray = new InventorySlot[slots.Length + newInventorySlots.Length];

                // Copy existing slots to the new array
                for (int i = 0; i < slots.Length; i++)
                {
                    newSlotsArray[i] = slots[i];
                }

                // Add the new slots to the array
                for (int i = 0; i < newInventorySlots.Length; i++)
                {
                    newSlotsArray[slots.Length + i] = newInventorySlots[i];
                }

                // Assign the new array to the slots field
                slots = newSlotsArray;

                items.Add(_item);

            }
            else
            {
                Debug.LogError("No InventorySlot components found in the new slots!");
            }
        
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

    public bool RemoveItem(List<int> itemIDs, int mainWeaponID, InventoryItem item, bool isMainWeapon)
    {
        bool returnValue = false;
        LongClickPopUpUi longClickPopUpUi = UIManager.instance.longClickPopUpUI.GetComponent<LongClickPopUpUi>();
        InventorySlot pressSlot = null;


        if (longClickPopUpUi.inventorySlot != null)
            pressSlot = longClickPopUpUi.inventorySlot;
        if (longClickPopUpUi.weaponSlot != null && longClickPopUpUi.weaponSlot.inventorySlot != null)
            pressSlot = longClickPopUpUi.weaponSlot.inventorySlot;

        if (pressSlot != null)
        {
            if (!itemIDs.Contains(pressSlot.gameObject.GetComponent<LongClickComponenet>().weaponID))
            {
                pressSlot = null;
            }
        }

        if (WeaponDataManager.Instance.GetKorWeaponClassText(mainWeaponID) != "안흔함" && isMainWeapon)
        {
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
                if (pressSlot.isEquiped)
                {
                    for (int j = 0; j < UIManager.instance.weaponSlotUI.Length; j++)
                    {
                        var weaponSlot = UIManager.instance.weaponSlotUI[j];

                        if (pressSlot == weaponSlot.inventorySlot)
                        {

                            items.Remove(pressSlot.weapon);
                            WeaponUI.Instance.weaponID = item.data.ID;
                            WeaponUI.Instance.ChangeItem(j, item);
                            GameManager.Instance.weaponCnt[itemIDs[0] - 1]--;
                            GameManager.Instance.RemoveUseWeaponList(pressSlot.GetComponent<LongClickComponenet>().weaponID);
                            WeaponManager.Instance.RemoveWeapon(j);
                            WeaponManager.Instance.AddWeapon(j, mainWeaponID);
                            weaponSlot.weaponID = mainWeaponID;
                            itemIDs.Remove(itemIDs[0]);
                            break;
                        }
                    }
                }
            }
        }

        foreach (int i in itemIDs)
        {
            InventoryItem removeToItem = null;
            if (i>5)
            {
                if (GameManager.Instance.useAbleWeaponCnt[i-1] > 0)
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
                    GameManager.Instance.useAbleWeaponCnt[i-1]--;
                }

                else
                {
                    for (int j = 0; j < UIManager.instance.weaponSlotUI.Length; j++)
                    {
                        var weaponSlot = UIManager.instance.weaponSlotUI[j];

                        if (!weaponSlot.hasWeapon) continue;
                        removeToItem = weaponSlot.transform.GetChild(0).GetComponent<InventorySlot>().weapon;

                        if (removeToItem.data.ID == i)
                        {
                            WeaponManager.Instance.RemoveWeapon(j);
                            GameManager.Instance.RemoveUseWeaponList(i);
                            weaponSlot.Init();
                            break;
                        }
                    }
                }
            }

            items.Remove(removeToItem);
            GameManager.Instance.weaponCnt[i - 1]--;
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

                string path = "WeaponIcon/" + data.num.ToString();

                notHeldslots[i].gameObject.SetActive(true);
                notHeldslots[i].GetComponent<AllShowInventorySlot>().weaponID = data.ID;

                notHeldSlotItem.GetComponent<InventorySlot>().weapon = new InventoryItem();
                notHeldSlotItem.GetComponent<InventorySlot>().weapon.AssignWeapon(data.ID);
                notHeldSlotItem.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                
                bool isEquiped = GameManager.Instance.IsUsing(j+1);
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
        foreach (var item in WeaponDataManager.Instance.Database.GetAllWeaponNums().Select((value, i) => (value, i)))
        {
            {
                if (WeaponDataManager.Instance.GetWeaponData(item.i+1).WeaponClass == "normal") continue;

                string weaponIconPath = "WeaponIcon/" + item.value.ToString();

                InventoryItem weapon = new InventoryItem
                {
                    image = ResourceManager.Instance.Load<Sprite>(weaponIconPath)
                };
                weapon.AssignWeapon(item.i+1);


                GameManager.Instance.weaponCnt[item.i - 1]++;
                GameManager.Instance.UpdateUseableWeaponCnt();
                AddItem(weapon, false);
            }

            BookMakredSlotUI.Instance.UpdateAllSlot();
        }
    }

    public void AddItemByNum(int num)
    {
        string path = "WeaponIcon/" + num;
        InventoryItem item = new InventoryItem
        {
            image = ResourceManager.Instance.Load<Sprite>(path)
        };
        item.AssignWeapon(WeaponDataManager.Instance.Database.GetWeaponIdByNum(num));
        AddItem(item);
    }

    public void OpenWeaponPickerConfirmPopUp(int num)
    {
        WeaponData data = WeaponDataManager.Instance.Database.GetWeaponDataByNum(num);
        TextMeshProUGUI confirmText = WeaponPickerConfirmPopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI warningText = WeaponPickerConfirmPopUp.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        confirmText.text = WeaponTierTranslator.TranslateToKorean(data.tier) + " 마스터키를 사용하겠습니까?";
        warningText.text = data.WeaponName + " 대신 " + WeaponTierTranslator.TranslateToKorean(data.tier) +
            " 등급의\n마스터키가 1개가 교환됩니다.";

        WeaponPickerConfirmPopUp.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            MasterKeyManager.Instance.UpdateMasterKeyCount(data.tier, -1);
            if (data.tier > WeaponTier.Normal)
                AddItemByNum(num);
            else
                GameManager.Instance.weaponCnt[num - 101]++;
            WeaponPickerConfirmPopUp.SetActive(false);
        });

        WeaponPickerConfirmPopUp.SetActive(true);
    }

    public void OpenWeaponPickerConfirmPopUp(List<int> num,int targetWeaponID,List<int> materialIDList , bool isMain)
    {
        List<WeaponData>datas = new List<WeaponData>();
        foreach (int i in num)
        {
            WeaponData data = WeaponDataManager.Instance.Database.GetWeaponDataByNum(i);
            datas.Add(data);
        }

        TextMeshProUGUI confirmText = WeaponPickerConfirmPopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI warningText = WeaponPickerConfirmPopUp.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        Dictionary<string, int> tierUsageCount = new Dictionary<string, int>();
        List<string> messages = new List<string>();
        foreach (int i in num)
        {
            WeaponData weaponData = WeaponDataManager.Instance.Database.GetWeaponDataByNum(i);
            string tierKorean = WeaponTierTranslator.TranslateToKorean(weaponData.tier);

            if (tierUsageCount.ContainsKey(tierKorean))
            {
                tierUsageCount[tierKorean]++;
            }
            else
            {
                tierUsageCount[tierKorean] = 1;
            }

            // 무기 이름 가져오기
            string weaponName = weaponData.WeaponName;

            // 메시지 작성: "무슨 무기 대신에 무슨 등급"
            string message = $"{weaponName} 대신에 {tierKorean} 등급";
            messages.Add(message);
        }

        string message2 = "";
        foreach (var kvp in tierUsageCount)
        {
            message2 += $"{kvp.Key} 등급의 마스터키가 {kvp.Value}개, ";
        }

        // 마지막 ", " 제거
        if (message2.EndsWith(", "))
        {
            message2 = message2.Substring(0, message2.Length - 2);
        }

        Debug.Log(targetWeaponID);
        confirmText.text = string.Join(", ", messages) + "의 마스터키를 사용하시겠습니까?";
        warningText.text = "재료 무기 대신 " + message2 + " 사용됩니다.";
        WeaponPickerConfirmPopUp.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach(var data in datas)
                MasterKeyManager.Instance.UpdateMasterKeyCount(data.tier, -1);

            string weaponIconPath = "WeaponIcon/" + WeaponDataManager.Instance.Database.GetWeaponNumByID(targetWeaponID);
            InventoryItem item = new InventoryItem
            {
                image = ResourceManager.Instance.Load<Sprite>(weaponIconPath)
            };
            item.AssignWeapon(targetWeaponID);

            WeaponUI.Instance.weaponID = targetWeaponID;

            AddItem(item, false);
            RemoveItem(materialIDList, targetWeaponID, item, true);

            if (isClassSorted)
                ClickClassShowButton();

            if(isMain)
                UIManager.instance.CreateCombineUI(targetWeaponID);

            GameManager.Instance.weaponCnt[targetWeaponID - 1]++;
            GameManager.Instance.UpdateUseableWeaponCnt();
            BookMakredSlotUI.Instance.UpdateAllSlot();
            WeaponPickerConfirmPopUp.SetActive(false);
        });

        WeaponPickerConfirmPopUp.SetActive(true);
        WeaponPickerConfirmPopUp.transform.SetAsLastSibling();
    }
}

