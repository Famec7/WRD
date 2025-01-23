using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : Singleton<TestManager>
{
    public GameObject TestPopUpArea;
    public Image TestButtonImage;
    public Image AutoWaveProgressionButtonImage;

    public TMP_InputField WaveInputField;
    protected override void Init()
    {
        ;
    }

    public void ClickAutoWaveProgressionButton()
    {
        MonsterSpawnManager.instance.isAutoWaveProgression = !MonsterSpawnManager.instance.isAutoWaveProgression;
        AutoWaveProgressionButtonImage.color = !MonsterSpawnManager.instance.isAutoWaveProgression ? Color.gray : Color.white;
    }

    public void ClickTestButton()
    {
        TestPopUpArea.SetActive(!TestPopUpArea.activeSelf);
        TestButtonImage.color = TestPopUpArea.activeSelf ? Color.gray : Color.white;
    }

    public void ClickMoveWave()
    {
        string inputString = WaveInputField.text;

        // 2) 문자열을 int로 변환 (TryParse 사용)
        int wave;
        if (int.TryParse(inputString, out wave))
        {
            if (MonsterSpawnManager.instance.isBossWave)
                MonsterSpawnManager.instance.targetBoss.HasAttacked(MonsterSpawnManager.instance.targetBossStatus.maxHP);

            MonsterSpawnManager.instance.ProgressWave(wave - GameManager.Instance.wave);
        }
        else
        {
            MessageManager.Instance.ShowMessage("유효한 값을 입력해주세요.", new Vector2(0, 218), 1f, 0.5f);
        }
    }

    public void ClickRestartWave()
    {
        UnitManager.instance.RemoveAllMonster();
        MonsterSpawnManager.instance.ProgressWave(0);
    }

    public void ClickPrevWave()
    {
        if (MonsterSpawnManager.instance.isBossWave)
        {
            MonsterSpawnManager.instance.targetBoss.HasAttacked(MonsterSpawnManager.instance.targetBossStatus.maxHP);
        }
        if (GameManager.Instance.wave > 1)
            MonsterSpawnManager.instance.ProgressWave(-1);
        else
            MessageManager.Instance.ShowMessage("1스테이지에서 누르지마세요", new Vector2(0, 218), 1f, 0.5f);
    }
}
