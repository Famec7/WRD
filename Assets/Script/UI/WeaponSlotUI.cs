using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSlotUI : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject weaponImage;
    public bool hasWeapon = false;
    public int weaponID;
    public InventorySlot inventorySlot;

    [SerializeField]
    private bool isWeaponUseable;
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
        inventorySlot.isEquiped = false;
        inventorySlot = null;
        transform.GetChild(0).GetComponent<Image>().sprite = null;
        transform.GetChild(0).GetComponent<Image>().enabled = false;
    }


    public void ChangeWeaponUseable()
    {
        isWeaponUseable = GameManager.instance.weaponCnt[weaponID-1] > 0;

        if (isWeaponUseable)
            transform.GetChild(0).GetComponent<Image>().color = Color.white;
        else
            transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.5f;
    }
}
