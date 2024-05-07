using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LongClickPopUpUi : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public GraphicRaycaster gr;

    public GameObject _bookmarkButton;
    public GameObject _deatiledDescriptionButton;
    public int weaponID;
    
    private bool isBookmarked;
    private bool isInventory;
    private bool isWeaponSlot;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            gr.Raycast(ped, results);
            // 없으면 return
            if (results.Count <= 0) return;
            bool isAnotherTouch = true;
            foreach (var result in results)
            {   
                Debug.Log(result.gameObject.tag);

                if ((result.gameObject.CompareTag("LongClickPopUpUI")))
                    isAnotherTouch = false;
            }

            if (isAnotherTouch)
            {
                gameObject.SetActive(false);
                UIManager.instance.SetActiveBlockImage(false);
            }
        }
    }

    public void SetFavoriteButtonText(bool isBookmarked, bool isInventory, bool isWeaponSlot)
    {
        this.isBookmarked = isBookmarked;
        this.isInventory = isInventory;
        this.isWeaponSlot = isWeaponSlot;
        var bookmarkButtomTMP = _bookmarkButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        weaponNameText.text = WeaponDataManager.instance.Data[weaponID - 1].weaponName;
        if (isBookmarked)
        {
            bookmarkButtomTMP.text = "즐겨찾기 해제";
        }
        else if (isInventory)
        {
            bookmarkButtomTMP.text = "장착";
        
        }
        else if (isWeaponSlot)
        {
            bookmarkButtomTMP.text = "장착 해제";
        }
    }

    public void ClickDetailedDescriptionButton()
    {
        UIManager.instance.CreateDetailedDescriptionUI(weaponID);
        gameObject.SetActive(false);
        UIManager.instance.SetActiveBlockImage(false);
    }
}
    