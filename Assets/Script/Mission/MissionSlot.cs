using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSlot : MonoBehaviour
{
    private int _index;

    private Image _icon;
    private TextMeshProUGUI _name;
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _clearText;

    private Color originColor;

    public bool isClaer;
    private void Awake()
    {
        _icon = GetComponent<Image>();
        _button = GetComponent<Button>();
        //_name = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void InitMissionSlot(int index)
    {
        _index = index;
        Difficulty difficulty = MissionDifficulty.GetMissionDifficulty(_index);
        switch (difficulty)
        {
            case Difficulty.EASY:
                originColor = Color.gray;
                break;
            case Difficulty.NORMAL:
                originColor = Color.black;
                break;
            case Difficulty.HARD:
                originColor = Color.yellow;
                break;
        }

        _icon.color = originColor;
        string bossName = MissionDataManager.Instance.GetName(_index);
        if (bossName == null)
            return;
        //_name.text = bossName;
        _button.onClick.AddListener(OpenMissionUI);
    }

    private void OpenMissionUI()
    {
        if (MissionManager.Instance.IsRunning)
        {
            MessageManager.Instance.ShowMessage("미션이 진행중입니다.", new Vector2(0, 200), 2f, 0.5f);
            return;
        }
        
        UIManager.instance.OpenPopUpUI(UIType.MissionDescription);
        MissionUI missionUI = UIManager.instance.GetPopUpUI() as MissionUI;

        missionUI.Index = _index;
        missionUI.Init();
        missionUI.MissionSlot = this;

        _icon.color = Color.blue;
    }

    public void ChangeOriginColor()
    {
        _icon.color = originColor;
    }

    public void Clear(bool isClear)
    {
        this.isClaer = isClear;
        _clearText.gameObject.SetActive(true);

        if (isClaer)
        {
            _clearText.text = "Clear";
        }
        else
        {
            _clearText.text = "Fail";
            _clearText.color = Color.red;
        }
        GetComponent<Button>().enabled = false;
    }
}