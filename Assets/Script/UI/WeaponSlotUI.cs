using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSlotUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int slotIndex;
    public int SlotIndex => slotIndex;

    public GameObject weaponImage;
    public bool hasWeapon = false;
    public int weaponID;
    public InventorySlot inventorySlot;
    public bool isWeaponUseable = true;
    void Start()
    {
        if (hasWeapon)
        {
            int tmpCode = weaponID;
            UIManager.instance.touchPos = Input.mousePosition;
          //  transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => UIManager.instance.CreateCombineUI(tmpCode));
        }
    }

    public void Init()
    {
        hasWeapon = false;
        weaponID = 0;
        transform.GetChild(0).GetComponent<InventorySlot>().weapon = null;

        if(inventorySlot)
            inventorySlot.isEquiped = false;

        inventorySlot = null;
        transform.GetChild(0).GetComponent<Image>().sprite = null;
        transform.GetChild(0).GetComponent<Image>().enabled = false;
    }


    public void ChangeWeaponUseable()
    {
        isWeaponUseable = GameManager.instance.useAbleWeaponCnt[weaponID-1] > 0;

        if (isWeaponUseable)
            transform.GetChild(0).GetComponent<Image>().color = Color.white;
        else
            transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.5f;
    }

    public void OnPointerEnter()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = rectTransform.sizeDelta * 1.1f;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;
    }

    public void OnPointerExit()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = rectTransform.sizeDelta / 1.1f;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;
    }
}
