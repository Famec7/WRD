using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSlotUI : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject weaponImage;
    public bool hasWeapon = false;
    public int weaponID;
    public InventorySlot inventorySlot;
    
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
        weaponImage = null;
        hasWeapon = false;
        weaponID = 0;
        transform.GetChild(0).GetComponent<Image>().sprite = null;
        transform.GetChild(0).GetComponent<Image>().enabled = false;
    }
}
