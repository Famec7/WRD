using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : UIPopUp
{
    [SerializeField] private TextMeshProUGUI missionDescription;
    private int _index;
    private MissionSlot _missionSlot;
    [SerializeField]
    private Image _missionImage;
    [SerializeField] private Button ChallengeButton;

    public MissionSlot MissionSlot
    {
        set => _missionSlot = value;
    }
    
    public int Index
    {
        set => _index = value;
    }

    public override void Init()
    {
        base.Init();
        _missionImage.sprite = Resources.Load<Sprite>($"MissionImage/{_index}");
        Difficulty curDifficulty = MissionDifficulty.GetMissionDifficulty(_index);
        WeaponTier requireGrade = (WeaponTier)MissionDataManager.Instance.GetRequiredGrade(_index).grade;
        int requireNumber = MissionDataManager.Instance.GetRequiredGrade(_index).count;

        missionDescription.text = "권장 무기: " + WeaponTierTranslator.TranslateToKorean(requireGrade) + " " + requireNumber + "개" + " 이상";
        
        if (curDifficulty == Difficulty.HARD)
        {
            missionDescription.text += "\r\n<color=red>권장 무기를 가지고 있지 않습니다.</color>";
        }
    }

    protected override void SetClosePopUp()
    {
        base.SetClosePopUp();
    }

    private void OnDisable()
    {
        _missionSlot.ChangeOriginColor();
    }

    public void ClickChallengeButton()
    {
        UIManager.instance.OpenPopUpUI(UIType.MissionCheck);
        MissionCheckUI checkUI = UIManager.instance.GetPopUpUI() as MissionCheckUI;
        checkUI.Index = _index;
        checkUI.Init();
        checkUI.gameObject.SetActive(true);
    }
}