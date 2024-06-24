using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlotSelectUI : Singleton<WeaponSlotSelectUI>
{
    // Start is called before the first frame update
    public Transform[] slots;
    public GraphicRaycaster gr;
    protected override void Init()
    {
        return;
    }
    void Start()
    {
        slots =  GetComponentsInChildren<Transform>();
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
            int order = -1;
            bool isAnotherTouch = true;
            
            foreach (var result in results)
            {
                if (result.gameObject.CompareTag("WeaponSlotSelectUI"))
                    order = int.Parse(result.gameObject.name);
                Debug.Log(result.gameObject.name);
            }

            if (order >= 0)
            {
                WeaponUI.Instance.AddItem(order);
                gameObject.SetActive(false);
            }
        }
    }
}
