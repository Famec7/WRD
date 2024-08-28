using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotSelectUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] slots;

    public bool isBookmarked = false;
    private InventoryItem _item;
  
    void Start()
    {
        //slots =  GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            var results = UIManager.instance.GetRayCastResult(true);

            // 없으면 끄기
            if (results.Count <= 0)
            {
                UIManager.instance.InitLongClickPopupUI();
                gameObject.SetActive(false);
            }
            
            int order = -1;
            bool isAnotherTouch = true;
            
            foreach (var result in results)
            {               
                if (result.gameObject.CompareTag("WeaponSlotSelectUI") && !isBookmarked)
                    order = int.Parse(result.gameObject.name);
                else if (result.gameObject.CompareTag("BookMarkedSelectSlot") && isBookmarked)
                    order = int.Parse(result.gameObject.name);               

            }

            if (order >= 0)
            {
                if (!isBookmarked)
                {
                    GameManager.instance.UpdateUseableWeaponCnt();
                    WeaponUI.Instance.AddItem(order,_item);
                }
                else 
                    BookMakredSlotUI.Instance.AddItem(order);

            }

            UIManager.instance.InitLongClickPopupUI();
            gameObject.SetActive(false);
        }
    }

    public void SetItem(InventoryItem item)
    {
        _item = item;
    }
}
