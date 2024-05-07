using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectButton : MonoBehaviour
{
    // Start is called before the first frame update
    public int grade;
    public bool select = false;
    public bool isNotHeldShow = false;
    public ClassSelectButton coupleButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickClassSelectButton()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("ClassSelectButton");
        
        foreach(GameObject button in buttons)
        {
            if (button == gameObject)
            {
                button.GetComponent<Image>().color = new Color32(237, 205, 109, 255);
                button.GetComponent<ClassSelectButton>().coupleButton.GetComponent<Image>().color =  new Color32(237, 205, 109, 255);
            }

            else
            {
                button.GetComponent<Image>().color = new Color32(219, 219, 219, 255);
                button.GetComponent<ClassSelectButton>().select = false;
                button.GetComponent<ClassSelectButton>().coupleButton.GetComponent<Image>().color = new Color32(219, 219, 219, 255);
            }

        }

        select = true;
        coupleButton.select = true;
        
        if(!isNotHeldShow)
            InventoryManager.instance.InventorySort(grade);
        else
            InventoryManager.instance.NotHeldSort(grade);
    }
}
