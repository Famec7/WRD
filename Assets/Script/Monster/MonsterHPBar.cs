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

    public Transform hpBarFillTransform;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        hpBarFillTransform.localScale = new Vector2(ownerStatus.HP / ownerStatus.maxHP,1);
        transform.position = transform.parent.position + new Vector3(0,0.2f,0);

        if (ownerStatus.HP >= ownerStatus.maxHP * 0.5f)
            spriteRenderer.color = Color.green;

            else if (ownerStatus.HP >= ownerStatus.maxHP * 0.25f)
            spriteRenderer.color = Color.yellow;
            else
            spriteRenderer.color = Color.red;

        if (ownerStatus.HP <= 0)
        {
            MonsterHPBarPool.ReturnObject(this);
        }
    }

    public void Init()
    {
        owner = null;
        ownerStatus = null;
        hpBarFillTransform.localScale = new Vector2(1, 1);
    }
}
