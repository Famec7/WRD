using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MissionUI : UIPopUp
{
    [SerializeField] private TextMeshProUGUI missionDescription;
    private int _index;
    private MissionSlot _missionSlot;
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
        MonsterPool.instance.GetPooledObject(UnitCode.MISSIONBOSS1 + _index);
        UIManager.instance.ClosePopUp();
    }
}