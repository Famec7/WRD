using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image image;
    public bool hasItem;
    public bool isEquiped;
    public InventoryItem _weapon;
    public TextMeshProUGUI equipText;
    public InventoryItem weapon
    {
        get { return _weapon; }
        set
        {
            _weapon = value;
            if (_weapon != null)
            {
                image.sprite = weapon.image;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }
    

    public void CreateCombineUI()
    {
        if (weapon == null) return;
        UIManager.instance.CreateCombineUI(weapon.data.ID);
        Debug.Log("¿ä±â¿ë");
        UIManager.instance.CreateInventoryDescriptionUI(weapon.data.ID);
    }

    public void Init()
    {
        weapon = null;
        hasItem = false;
        isEquiped = false;
        equipText.gameObject.SetActive(false);
    }
}
