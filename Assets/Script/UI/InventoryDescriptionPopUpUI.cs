using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescriptionPopUpUI : UIPopUp
{
    // Start is called before the first frame update
    public Image weaponImage;
    public GameObject skillDescriptionPrefab;
    
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponClassText;
    public TextMeshProUGUI weaponTypeText;
    public TextMeshProUGUI[] weaponStatText;

    public int weaponId;
    void Start()
    {
        SetClosePopUp();
        gameObject.tag = "InventoryDescriptionUI";
    }

    // Update is called once per frame
    public void ClickDescriptionButton()
    {
        UIManager.instance.CreateDetailedDescriptionUI(weaponId);
    }

    
    protected override void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.CloseInventoryDescriptionPopUpUI();
        });
    }
}
