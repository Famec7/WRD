using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIHelper
{
    public static bool IsPointerOverUILayer(int layer, Touch touch = default)
    {
        // touch가 default 값이라면 position을 Input.mousePosition으로 설정
        if (touch.Equals(default(Touch)))
        {
            touch = new Touch
            {
                position = Input.mousePosition
            };
        }
        
        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = touch.position
        };

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.layer == layer)
            {
                return true;
            }
        }

        return false;
    }
}