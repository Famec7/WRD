using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;
using static ElementManager;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager instance;
    
    public TextMeshProUGUI waveNum;
    public TextMeshProUGUI currentMonsterNum;
    public TextMeshProUGUI limitMonsterNum;
    public TextMeshProUGUI currentWaveTime;

    public TextMeshProUGUI[] elementCnt;

    public Toggle autoSkipToggle;
    public Stack<CombinePopUpUI>combinePopupUIStack;
    public Stack<DetailedDescriptionUI> descriptionPopUpUIStack;

    public Vector3 touchPos;
    public GameObject[] PopUpPrefabs;
    public GameObject inventory;
    public GameObject allShowInventoryContent;
    public GameObject normalInventoryContent;
    public GameObject backButton;
    public GameObject longClickPopUpUI;
    
    private Stack<UIPopUp> popUpStack;
    [SerializeField]
    private Canvas _popupCanvas;
    [SerializeField]
    private Image _blockImage;
    private UIPopUp[] _popups;

    [SerializeField] private GameObject missionUI;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;

        combinePopupUIStack = new Stack<CombinePopUpUI>();
        descriptionPopUpUIStack = new Stack<DetailedDescriptionUI>(); 
    }

    private void Start()
    {
        combinePopupUIStack = new Stack<CombinePopUpUI>();
        popUpStack = new Stack<UIPopUp>();
        _popups = new UIPopUp[UIType.COUNT.GetHashCode() + 1];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            touchPos = Input.mousePosition;

        for (int i = 0; i < 5; i++)
        {
            elementCnt[i].text = GameManager.instance.weaponCnt[i].ToString();
        }

    }

    public void CreateCombineUI(int weaponID , bool isBlock = true)
    {
        if (Mathf.Abs(touchPos.x - Input.mousePosition.x) >= 20 || Mathf.Abs(touchPos.y - Input.mousePosition.y) >= 20) return;
        //if (inventory.activeSelf) return;
        if (descriptionPopUpUIStack.Count > 0) return;
        
        CloseCombinePopUpUI();
        Debug.Log("dd");
        GameObject combineUI = WeaponCombinationUIGenerator.Instance.combineWeaponUIList[weaponID - 1];
        
        combinePopupUIStack.Push(combineUI.GetComponent<CombinePopUpUI>());
        combineUI.SetActive(true);
        _blockImage.gameObject.SetActive(isBlock);

    }
    
    public void CreateDetailedDescriptionUI(int weaponID , bool isBlock = true)
    {
        CloseCombinePopUpUI();
        
        GameObject detailedDescriptionUI = DetailedDescriptionUIGenerator.Instance.detailedDescriptionUIList[weaponID - 1];
        detailedDescriptionUI.transform.SetAsLastSibling();
        detailedDescriptionUI.SetActive((true));
        descriptionPopUpUIStack.Push(detailedDescriptionUI.GetComponent<DetailedDescriptionUI>());
        
        if(descriptionPopUpUIStack.Count > 1) {
            backButton.SetActive(true);
            backButton.transform.SetAsLastSibling();

            foreach (var popup in descriptionPopUpUIStack)
            {
                if (popup == descriptionPopUpUIStack.Peek()) continue;
                
                GameObject current = popup.gameObject;
                current.SetActive((false));
            }
        }
        
        _blockImage.gameObject.SetActive(isBlock);
    }
    public void CloseCombinePopUpUI()
    {
        if (combinePopupUIStack.Count > 0)
        {
            GameObject current = combinePopupUIStack.Peek().gameObject;
            current.SetActive(false);
            combinePopupUIStack.Pop();
        }
        _blockImage.gameObject.SetActive(false);
    }
    
    public void CloseDetailedDescriptionPopUpUI()
    {
        foreach (var popup in descriptionPopUpUIStack)
        {
            GameObject current = popup.gameObject;
            current.SetActive((false));
        }
        
        descriptionPopUpUIStack.Clear();
        backButton.SetActive(false);
        _blockImage.gameObject.SetActive(false);
    }

   public void ChangeAutoSkipToggle()
    {
        GameManager.instance.isSKip = autoSkipToggle.isOn;
    }

    public void OpenInventory()
    {
        inventory.SetActive(true);
        CloseCombinePopUpUI();

        if (InventoryManager.instance.isLastAllShow) 
            allShowInventoryContent.SetActive(true);
        else
            normalInventoryContent.SetActive(true);

        if (InventoryManager.instance.isClassSorted)
            InventoryManager.instance.ClickClassShowButton();
    }

    public void ClickBackButton()
    {
        GameObject top = descriptionPopUpUIStack.Peek().gameObject;
        
        top.SetActive(false);
        descriptionPopUpUIStack.Pop();
        descriptionPopUpUIStack.Peek().gameObject.SetActive(true);
        
        if (descriptionPopUpUIStack.Count < 2)
            backButton.SetActive(false);
    }
    
    /// <summary>
    ///    리소스 디렉토리에서 해당하는 팝업을 찾아서 생성한다.
    /// </summary>
    /// <param name="type">프리펩 이름과 타입이름이 일치해야합니다.</param>
    /// <param name="isBlock">팝업을 띄울 때 터치를 막을지 여부</param>
    public void OpenPopUpUI(UIType type, bool isBlock = true)
    {
        ref UIPopUp popUp = ref _popups[(int)type];
        // _popups에 해당 타입의 팝업이 없으면 리소스에서 로드해서 생성한다.
        if (popUp == null)
        {
            string path = "UI/PopUp/" + type.ToString();
            _popups[type.GetHashCode()] = ResourceManager.Instance.Instantiate(path, _popupCanvas.transform).GetComponent<UIPopUp>();
        }
        
        _blockImage.gameObject.SetActive(isBlock);
        // 팝업을 생성하고 스택에 넣는다.
        popUp.Init();
        popUpStack.Push(popUp);
        _popupCanvas.sortingOrder = popUpStack.Count;
    }
    
    public UIPopUp GetPopUpUI()
    {
        if (popUpStack.Count == 0) return null;
        return popUpStack.Peek();
    }
    
    /// <summary>
    ///     스택에 있는 팝업을 닫는다.
    /// </summary>
    /// <param name="type">닫을 팝업의 타입</param>
    public void ClosePopUp()
    {
        UIPopUp popUp = GetPopUpUI();
        if (popUp == null) return;
        popUpStack.Pop();
        popUp.gameObject.SetActive(false);
        _blockImage.gameObject.SetActive(false);
    }
    
    public void ChangeBottomUI()
    {
        missionUI.gameObject.SetActive(!missionUI.activeSelf);
    }

    public void SetActiveBlockImage(bool isBlock)
    {
        _blockImage.gameObject.SetActive(isBlock);
    }
}
