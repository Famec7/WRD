using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class DetailedDescriptionUI : UIPopUp
{
    // Start is called before the first frame update
    public int weaponId;
    
    public Image weaponImage;
    
    public GameObject skillDescriptionPrefab;
    public GameObject highLevelCombinationPrefab;
    
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponClassText;
    public TextMeshProUGUI weaponDescriptionText;
    public List<GameObject> iconList;



    private List<int> canCombinWeaponsList;
    private int canCombineCnt;

    void Start()
    {
      SetClosePopUp();
    }
 

    protected override void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.CloseDetailedDescriptionPopUpUI();
            UIManager.instance.longClickPopUpUI.SetActive(false);
        });
    }

    // Update is called once per frame
}

