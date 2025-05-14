using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : UIPopUp
{
    [SerializeField] private TextMeshProUGUI missionDescription;
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionReward;

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
        WeaponTier requireGrade = (WeaponTier)MissionDataManager.Instance.GetRequiredGrade(_index).grade;
        int requireNumber = MissionDataManager.Instance.GetRequiredGrade(_index).count;

        switch(_index)
        {
            case 0:
                missionName.text = "흙흙이";
                missionDescription.text = "온순하고 약한 녀석입니다. 고급무기 두개면 손쉽게 해치울 수 있습니다.";
                missionReward.text = "원소 마스터키 5개";
                break;

            case 1:
                missionName.text = "미역이";
                missionDescription.text = "적당히 강한 녀석입니다. 희귀무기 하나면 적당하게 처리할 수 있습니다.";
                missionReward.text = "원소 마스터키 10개";
                break;

            case 2:
                missionName.text = "얼음이";
                missionDescription.text = "한눈에 봐도 단단하게 생긴 녀석입니다. 영웅무기 하나면 어찌저찌 처리될 것 같습니다.";
                missionReward.text = "희귀 마스터키 1개";
                break;

            case 3:
                missionName.text = "마왕이";
                missionDescription.text = "딱 봐도 날렵하게 생긴 녀석입니다. 영웅무기와 스턴 무기로 꼼짝 못하게 할 수 있습니다.";
                missionReward.text = "희귀 마스터키 2개";
                break;

            case 4:
                missionName.text = "용감이";
                missionDescription.text = "용감해 보이는 녀석입니다. 전설무기 하나면 뚫어낼 수 있습니다.";
                missionReward.text = "영웅 마스터키 1개";
                break;

            case 5:
                missionName.text = "죽음이";
                missionDescription.text = "아주 강력한 녀석입니다. 전설무기 하나와 스턴 무기면 덤벼볼만 하겠군요.";
                missionReward.text = "영웅 마스터키 2개";
                break;

            default:
                missionName.text = "";
                missionDescription.text = "";
                missionReward.text = "";
                break;
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