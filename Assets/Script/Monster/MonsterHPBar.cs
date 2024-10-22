using UnityEngine;
using UnityEngine.UI;
public class MonsterHPBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Status ownerStatus;
    public GameObject owner = null;

    public Transform hpBarFillTransform;

    public SpriteRenderer spriteRenderer;

    // Update is called once per frame
    private void Update()
    {

        hpBarFillTransform.localScale = new Vector2(ownerStatus.HP / ownerStatus.maxHP,1);
        transform.position = transform.parent.position + new Vector3(0,0.2f,-1);

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
