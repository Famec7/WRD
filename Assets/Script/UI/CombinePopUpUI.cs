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
  

    public void ClickDescriptionButton()
    {
        UIManager.instance.CreateDetailedDescriptionUI(weaponId);
    }

    public void ChangeInventoryMode(bool isInventory)
    {
        weaponName.gameObject.SetActive(!isInventory);
        weaponIcon.gameObject.SetActive(!isInventory);
        closeButton.gameObject.SetActive(!isInventory);
        descriptionButton.gameObject.SetActive(!isInventory);
        Transform[] subCombinationUIs = transform.GetComponentsInChildren<Transform>();
        if (isInventory)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector3(165, -1266);
            GetComponent<RectTransform>().sizeDelta = new Vector2(500,300);
            foreach (var subCombinationUI in subCombinationUIs)
            {
                
                if (subCombinationUI.GetComponent<SubCombinationUI>() != null)
                    subCombinationUI.GetComponent<RectTransform>().anchoredPosition = subCombinationUI.GetComponent<SubCombinationUI>().originPosition + new Vector3(0, 160);
            }
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector3(288, -980);
            GetComponent<RectTransform>().sizeDelta = new Vector2(401, 240f);
            foreach (var subCombinationUI in subCombinationUIs)
            {
                if (subCombinationUI.GetComponent<SubCombinationUI>() != null)
                    subCombinationUI.GetComponent<RectTransform>().anchoredPosition = subCombinationUI.GetComponent<SubCombinationUI>().originPosition;
            }
        }

      

    }

    protected override void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.CloseCombinePopUpUI();
        });
    }
}
