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
    
    public void AssignWeapon(int code)
    {
        data = WeaponDataManager.instance.Data[code-1];
        if(Enum.TryParse<WeaponTier>(data.weaponClass, true, out data.tier))
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
