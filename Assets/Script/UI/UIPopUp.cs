using UnityEngine;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour
{
    [Header("닫기 버튼")]
    [SerializeField]
    protected Button closeButton;
    
    /// <summary>
    /// 팝업 초기화
    /// 팝업창과 관련된 초기화 작업을 수행한다.
    /// </summary>
    public virtual void Init()
    {
        this.gameObject.SetActive(true);
        SetClosePopUp();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var popUPresults = UIManager.instance.GetRayCastResult(true);
            var results = UIManager.instance.GetRayCastResult(false);
            // 없으면 return

            if (popUPresults.Count == 0 && results.Count == 0)
            {
                UIManager.instance.CloseCombinePopUpUI();
                UIManager.instance.CloseInventoryDescriptionPopUpUI();
                UIManager.instance.CloseDetailedDescriptionPopUpUI();
                UIManager.instance.CloseDetailedCombinationPopUpUI();
                UIManager.instance.longClickPopUpUI.SetActive(false);
                UIManager.instance.WeaponPickerPopUpUI.SetActive(false);
                foreach (var pickerUI in MasterKeyManager.Instance.WeaponPickerList)
                    pickerUI.SetActive(false);
            }

            bool isButton = false;
            foreach (var result in popUPresults)
            {
                if (result.gameObject.CompareTag("LongClickPopUpUI") || popUPresults.Count >= 3 || result.gameObject.CompareTag("DetailedDescriptionUI") ||
                    result.gameObject.CompareTag("Mission") || result.gameObject.CompareTag("InventoryDescriptionUI") || result.gameObject.CompareTag("WeaponPicker") || result.gameObject.CompareTag("CombinedWeaponImage"))
                    isButton = true;
            }

            if (isButton) return;

            foreach (var result in results)
            {
                Debug.Log(result.gameObject.name);

                if ((result.gameObject.name == "Slot" && !result.gameObject.transform.GetChild(0).GetComponent<InventorySlot>().isEquiped) ||
                    results.Count < 4)
                {
                    UIManager.instance.CloseAllPopUpUI();
                    UIManager.instance.longClickPopUpUI.SetActive(false);
                    foreach (var pickerUI in MasterKeyManager.Instance.WeaponPickerList)
                        pickerUI.SetActive(false);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 닫기 버튼 이벤트 설정
    /// 닫기 버튼을 눌렀을 때 팝업을 닫는 이벤트를 설정한다.
    /// 이후 이벤트를 추가하고 싶다면 이 메서드를 오버라이드하여 사용한다.
    /// </summary>
    protected virtual void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.ClosePopUp();
        });
    }
}