using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MonoBehaviour
{
    public GameObject boxIndicator;
    public GameObject circleIndicator;
    public GameObject indicator;
    public Vector3 targetPoint;

    void Start()
    {
        boxIndicator = GameObject.Find("box_indicator");
        circleIndicator = GameObject.Find("circle_indicator");
        boxIndicator.SetActive(false);
        circleIndicator.SetActive(false);
    }
    void Update()
    {
        if(indicator)
        {
            if(Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                indicator.transform.position = mousePosition;
            }
            if(Input.GetMouseButtonUp(0))
            {
                targetPoint = indicator.transform.position;
                indicator.transform.localScale = Vector3.one;
                indicator = null;
            }
        }
    }

    public void Set_Circle_Indicator(float radius)
    {
        circleIndicator.SetActive(true);
        indicator = circleIndicator;
        indicator.transform.localScale *= radius;
    }
}
