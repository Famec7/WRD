using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombinePopUpUI : UIPopUp
{
    public int weaponId;
    private List<int> canCombinWeaponsList;

    public GameObject[] subCombinationPrefab;
    public GameObject descriptionButton;
    public TextMeshProUGUI weaponName;
    public Image weaponIcon;
    
    private int canCombineCnt;
    // Start is called before the first frame update
    void Start()
    {
        SetClosePopUp();
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetAsLastSibling();
    }

    public void ClickDescriptionButton()
    {
        UIManager.instance.CreateDetailedDescriptionUI(weaponId);
    }

    protected override void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.CloseCombinePopUpUI();
        });
        
    }
}
