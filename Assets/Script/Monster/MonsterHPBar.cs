using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class MonsterHPBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Status ownerStatus;
    public GameObject owner = null;

    public Image hpBarFillImage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        hpBarFillImage.fillAmount = ownerStatus.HP / ownerStatus.maxHP;
     
        //Camera mainCamera = Camera.main;

        //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, owner.transform.position + new Vector3(0.01f, 0.1f, 0));
        //RectTransform canvasRectTransForm = GameObject.Find("Canvas").GetComponent<RectTransform>();
        //Vector2 localPoint;

        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransForm, screenPoint, mainCamera, out localPoint))
        //    GetComponent<RectTransform>().localPosition = localPoint;

        Vector3 hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(owner.transform.position.x, owner.transform.position.y + 0.1f, 0));
        transform.position = hpBarPos;

        if (ownerStatus.HP >= ownerStatus.maxHP * 0.5f)
                hpBarFillImage.color = Color.green;

            else if (ownerStatus.HP >= ownerStatus.maxHP * 0.25f)
                hpBarFillImage.color = Color.yellow;
            else
                hpBarFillImage.color = Color.red;

        if (ownerStatus.HP <= 0)
        {
            MonsterHPBarPool.ReturnObject(this);
        }
    }
}
