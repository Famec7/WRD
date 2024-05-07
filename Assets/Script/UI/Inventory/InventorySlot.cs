using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image image;
    public bool hasItem;
    private InventoryItem _weapon;
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCombineUI()
    {
        if (weapon == null) return;
        UIManager.instance.CreateCombineUI(weapon.data.id);
    }
}
