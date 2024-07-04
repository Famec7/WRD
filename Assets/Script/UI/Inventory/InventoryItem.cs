using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem 
{
    // Start is called before the first frame update

    public WeaponData data;
    public Sprite image;
    public DateTime earnTime;
    public int equipPos = -1;
    
    public void AssignWeapon(int code)
    {
        data = WeaponDataManager.Instance.Database.GetWeaponData(code);
        if(Enum.TryParse<WeaponTier>(data.WeaponClass, true, out data.tier))
        {
            //Debug.Log("Success");
        }
        else
        {
            data.tier = WeaponTier.Empty;
        }
        earnTime = DateTime.Now;
    }

    public void ReleaseWeapon()
    {
        data = null;
    }
}
