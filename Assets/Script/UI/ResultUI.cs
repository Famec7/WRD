using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public GameObject EquipWeaponPrefab;
    public Transform WeaponSectionTransform;

    public TextMeshProUGUI PlayTime;
    public TextMeshProUGUI Wave;
    public TextMeshProUGUI Enemy;

    public Sprite OverHeader;
    public Sprite ClearHeader;
    public Image Header;

    public void Update()
    {
        transform.SetAsLastSibling();
        Time.timeScale = 0f;
    }
    public void SetResultUI(bool isClear)
   {
        if (isClear)
            Header.sprite = ClearHeader;
        else
            Header.sprite = OverHeader;

        int minutes = Mathf.FloorToInt(UIManager.instance.elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(UIManager.instance.elapsedTime % 60f);

        PlayTime.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        Wave.text = GameManager.Instance.wave.ToString();
        Enemy.text = MonsterSpawnManager.instance.currentMonsterNum.ToString();

        foreach (var weapon in WeaponManager.Instance._equippedWeapons)
        {
            if (weapon == null) continue;
            WeaponData data = weapon.Data;
            GameObject EquipWeapon = Instantiate(EquipWeaponPrefab, WeaponSectionTransform);
            EquipWeapon.GetComponent<Image>().color = WeaponTierTranslator.GetClassColor(data.WeaponClass);
            string weaponIconPath = "WeaponIcon/" + data.num.ToString();
            EquipWeapon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
        }
    }
}
    