using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class DetailedCombinationPopupUI : UIPopUp
{
    public Image weaponImage_;
    private int weaponNum_;
    private int weaponID_;

    protected override void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.CloseDetailedCombinationPopUpUI();
        });
    }

    public void Init(int weaponNum)
    {
        weaponNum_ = weaponNum;
        var path = "WeaponIcon/" + weaponNum_;
        weaponImage_.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
        weaponID_ = WeaponDataManager.Instance.Database.GetWeaponIdByNum(weaponNum_);
        SetClosePopUp();
    }

    public void OnClickDetailedButton()
    {
        int weaponID = WeaponDataManager.Instance.Database.GetWeaponIdByNum(weaponNum_);
        UIManager.instance.CreateDetailedDescriptionUI(weaponID, closeCombine:false);
    }
}
