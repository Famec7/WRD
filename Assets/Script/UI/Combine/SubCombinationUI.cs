using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;


public class SubCombinationUI : MonoBehaviour
{
    // Start is called before the first frame update
    public int mainweaponID;
    public Vector3 originPosition;
    public int[] materialWeapons;
    public CraftButton[] craftButtons;


    public void SetCraftButtons()
    {
        craftButtons[0].isMain = true;
        craftButtons[0].weaponID = mainweaponID;
        craftButtons[0].materialWeapons = materialWeapons;

        int i = 1;
        int index = 1;

        foreach (int materialNum in materialWeapons)
        {
            int materialID = WeaponDataManager.Instance.Database.GetWeaponIdByNum(materialNum);
            craftButtons[index++].weaponID = materialID;
            
            if (materialID < 6) continue;

            string combi = WeaponDataManager.Instance.Database.GetWeaponData(materialID).Combi;
            string mainCombi = WeaponDataManager.Instance.Database.GetWeaponData(materialID).MainCombi;
            string[] combis = combi.Split('\x020');
            craftButtons[i].materialWeapons = new int[combis.Length];
            int j = 0;
            
            foreach (string s in combis)
            {
                craftButtons[i].materialWeapons[j++] = Convert.ToInt32(s);
            }
            i++;
        }
    }
}
