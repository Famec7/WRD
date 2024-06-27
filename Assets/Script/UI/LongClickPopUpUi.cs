using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickPopUpUi : MonoBehaviour
{
    public GraphicRaycaster gr;

    public GameObject _bookmarkButton;
    public GameObject _equipButton;
    public InventorySlot inventorySlot;
    public WeaponSlotUI weaponSlot;
    public int weaponID;
    
    private bool isBookmarked;
    private bool isInventory;
    private bool isWeaponSlot;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            gr.Raycast(ped, results);
            // 없으면 return
            if (results.Count <= 0) return;
            bool isAnotherTouch = true;
            foreach (var result in results)
            {   
                if ((result.gameObject.CompareTag("LongClickPopUpUI")))
                    isAnotherTouch = false;
            }
      
        }
        
        transform.SetAsLastSibling();
    }

    public void SetFavoriteButtonText(bool isBookmarked, bool isInventory, bool isWeaponSlot)
    {
        this.isBookmarked = isBookmarked;
        this.isInventory = isInventory;
        this.isWeaponSlot = isWeaponSlot;
        
        var bookmarkButtonTMP = _bookmarkButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        var equipButtonTMP = _equipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        
        if (!isWeaponSlot && GameManager.instance.weaponCnt[weaponID-1] > 0)
            equipButtonTMP.text = "장착";
        
        if (isBookmarked)
        {
            bookmarkButtonTMP.text = "즐겨찾기 해제";
        }
        else if (isInventory || isWeaponSlot)
        {
            bookmarkButtonTMP.text = "즐겨찾기 등록";
        }
        
        if (isWeaponSlot)
        {
            equipButtonTMP.text = "장착 해제";
        }
    }

    public void ClickDetailedDescriptionButton()
    {
        UIManager.instance.CreateDetailedDescriptionUI(weaponID);
        gameObject.SetActive(false);
        UIManager.instance.SetActiveBlockImage(false);
    }

    public void ClickBookMarkButton()
    {
        if (!isBookmarked)
        {
            BookMakredSlotUI.Instance.AddItem(weaponID);
            gameObject.SetActive(false);
            UIManager.instance.SetActiveBlockImage(false);
        }
      
    }
    
    public void ClickEquipButton()
    {
        if (isWeaponSlot || (isInventory && inventorySlot.isEquiped))
            UnEuqip();
        
        WeaponUI.Instance.weaponID = weaponID;
        UIManager.instance.WeaponSlotSelectUI.SetActive(true);
        if (isInventory)
        {
            inventorySlot.isEquiped = true;
            inventorySlot.equipText.gameObject.SetActive(true);
        }
    }
    public void UnEuqip()
    {
        inventorySlot.isEquiped = false;
    }
}
    