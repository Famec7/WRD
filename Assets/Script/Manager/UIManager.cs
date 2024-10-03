using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
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
    public Stack<InventoryDescriptionPopUpUI> inventoryDescriptionPopUpUiStack;

    public Vector3 touchPos;
    public GameObject[] PopUpPrefabs;
    public GameObject inventory;
    public GameObject allShowInventoryContent;
    public GameObject normalInventoryContent;
    public GameObject backButton;
    public GameObject longClickPopUpUI;
    public GameObject BookmarkSlotUI;
    public GameObject WeaponSlotSelectUI;
    public GameObject BookmarkSlotSelectUI;

    public GraphicRaycaster PopUpGr;
    public GraphicRaycaster Gr;

    public Button[] elementUI;
    public WeaponSlotUI[] weaponSlotUI;
    
    private Stack<UIPopUp> popUpStack;
    
    public Canvas _popupCanvas;
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
        inventoryDescriptionPopUpUiStack = new Stack<InventoryDescriptionPopUpUI>();

        for (int i = 0; i < elementUI.Length; i++)
        {
            int index = i;
            elementUI[i].GetComponent<Button>().onClick.AddListener(() => CreateCombineUI(index+1));
        }

        autoSkipToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ChangeAutoSkipToggle(); });
    }

    private void Start()
    {
        combinePopupUIStack = new Stack<CombinePopUpUI>();
        popUpStack = new Stack<UIPopUp>();
        _popups = new UIPopUp[UIType.MissionCheck.GetHashCode() + 1];

        
    }

    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            elementCnt[i].text = GameManager.Instance.weaponCnt[i].ToString();
        }
    }

    public void CreateCombineUI(int weaponID, bool isBlock = true, bool isInventory = false, bool isEquiped = false)
    {
        //if (inventory.activeSelf) return;
        if (descriptionPopUpUIStack.Count > 0) return;
        CloseInventoryDescriptionPopUpUI();
        CloseCombinePopUpUI();

        GameObject combineUI = WeaponCombinationUIGenerator.Instance.combineWeaponUIList[weaponID - 1];
        combineUI.transform.parent = _popupCanvas.transform;
        if (combineUI == null) return;
        combineUI.GetComponent<CombinePopUpUI>().ChangeInventoryMode(isInventory);
        combinePopupUIStack.Push(combineUI.GetComponent<CombinePopUpUI>());
        combineUI.SetActive(true);
        // _blockImage.gameObject.SetActive(isBlock);
 
    }
    
    public GameObject CreateInventoryDescriptionUI(int weaponID , bool isBlock = true)
    {
        longClickPopUpUI.SetActive(true);

        GameObject inventoryDescriptionUI = InventoryDescriptionUIGenerator.Instance.inventoryDescriptionUIList[weaponID - 1];
        
        inventoryDescriptionPopUpUiStack.Push(inventoryDescriptionUI.GetComponent<InventoryDescriptionPopUpUI>());
        inventoryDescriptionUI.SetActive(true);
        // _blockImage.gameObject.SetActive(isBlock);

        return inventoryDescriptionUI;
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
        
      //  _blockImage.gameObject.SetActive(isBlock);
    }
    public void CloseCombinePopUpUI()
    {
        if (combinePopupUIStack.Count > 0)
        {
            GameObject current = combinePopupUIStack.Peek().gameObject;
            current.SetActive(false);
            combinePopupUIStack.Pop();
        }
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
    }
    
    public void CloseInventoryDescriptionPopUpUI()
    {
        CloseCombinePopUpUI();

        InventoryManager.instance.inventorySelectUI.SetActive(false);
        foreach (var popup in inventoryDescriptionPopUpUiStack)
        {
            GameObject current = popup.gameObject;
            current.SetActive((false));
        }
        
        inventoryDescriptionPopUpUiStack.Clear();
        backButton.SetActive(false);
    }
    
    public void CloseAllPopUpUI()
    {
        CloseCombinePopUpUI();
        CloseInventoryDescriptionPopUpUI();
        CloseDetailedDescriptionPopUpUI();
        InitLongClickPopupUI();
        longClickPopUpUI.SetActive(false);
    }
   public void ChangeAutoSkipToggle()
    {
        GameManager.Instance.isSKip = autoSkipToggle.isOn;
    }

    public void OpenInventory()
    {
        if (!inventory.activeSelf)
        {
            inventory.SetActive(true);
            InventoryManager.instance.FreshSlot();

            CloseCombinePopUpUI();

            if (InventoryManager.instance.isLastAllShow)
                allShowInventoryContent.SetActive(true);
            else
                normalInventoryContent.SetActive(true);

            if (InventoryManager.instance.isClassSorted)
                InventoryManager.instance.ClickClassShowButton();
        }
        else
            InventoryManager.instance.CloseButton();

        if (missionUI.activeSelf)
            ToggleMissionUI();
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
       //_blockImage.gameObject.SetActive(isBlock);
    }

    public void InitLongClickPopupUI()
    {
        longClickPopUpUI.GetComponent<LongClickPopUpUi>().inventorySlot = null;
        longClickPopUpUI.GetComponent<LongClickPopUpUi>().weaponSlot = null;
    }

    public List<RaycastResult> GetRayCastResult(bool isPopUp = false)
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        if(!isPopUp)
            Gr.Raycast(ped, results);
        else
            PopUpGr.Raycast(ped, results);

        return results;
    }

    public void ToggleMissionUI()
    {
        missionUI.SetActive(!missionUI.activeSelf);
        if (inventory.activeSelf)
            InventoryManager.instance.CloseButton();
    }

    public Vector2 ConvertToPercentageCoordinates(Vector2 pos)
    {
        // 기준 해상도
        float referenceWidth = 1080f;
        float referenceHeight = 1920f;

        // Canvas Scaler의 스케일 팩터 가져오기
       
        CanvasScaler canvasScaler = _popupCanvas.GetComponent<CanvasScaler>();

        float scaleFactor = 1f;

        if (canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
        {
            Vector2 referenceResolution = canvasScaler.referenceResolution;
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float matchWidthOrHeight = canvasScaler.matchWidthOrHeight;
            float widthScale = screenWidth / referenceResolution.x;
            float heightScale = screenHeight / referenceResolution.y;

            // 스케일 팩터 계산
            scaleFactor = Mathf.Pow(widthScale, 1 - matchWidthOrHeight) * Mathf.Pow(heightScale, matchWidthOrHeight);
        }

        // 실제 위치 계산 (스케일 팩터 반영)
        Vector2 scaledPosition = pos * scaleFactor;

        // 퍼센트 계산
        float xPercent = scaledPosition.x / (referenceWidth * scaleFactor);
        float yPercent = scaledPosition.y / (referenceHeight * scaleFactor);

        // 현재 해상도에서의 위치 계산
        float currentX = xPercent * Screen.width;
        float currentY = yPercent * Screen.height;

        return new Vector2(currentX, currentY);
    }

    public IEnumerator ActiveBookMarekdSelectUI()
    {
        yield return new WaitForSeconds(0.4f);
        BookmarkSlotSelectUI.SetActive(true);
    }
}
