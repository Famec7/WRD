using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllShowInventorySlot : MonoBehaviour
{
    // Start is called before the first frame update
    public int weaponID;
    public bool isAcitve = false;
    Image slotImage;
    Image weaponImage;

    TextMeshProUGUI countText;

    void Start()
    {
       slotImage = GetComponent<Image>();
       weaponImage = transform.GetChild(0).GetComponent<Image>();
       countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.weaponCnt[weaponID - 1] > 0)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            countText.text = GameManager.instance.weaponCnt[weaponID - 1].ToString();
            slotImage.color = new Color32(255, 255, 255,255);
            weaponImage.color = new Color32(255, 255, 255,255);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            slotImage.color = new Color32(140,140, 140,255);
            weaponImage.color = new Color32(140, 140, 140,255);
        }
        
    }
}
