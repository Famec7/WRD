using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedCombinationButton : MonoBehaviour
{
    public int weaponID;
    public Image BackGround; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BackGround.color = WeaponTierTranslator.GetClassColor(WeaponDataManager.Instance.Database.GetWeaponData(weaponID).WeaponClass);
        if (GameManager.Instance.weaponCnt[weaponID - 1] == 0)
            BackGround.color *= 0.7f;
    }
}
