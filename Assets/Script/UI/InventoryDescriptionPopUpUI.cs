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
    void Start()
    {
        SetClosePopUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected override void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.CloseInventoryDescriptionPopUpUI();
        });
    }
}
