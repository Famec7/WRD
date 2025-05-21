using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    private Canvas HelpCanvas;

    [SerializeField]
    private Canvas RankingCanvas;
    public void OnClickGameStartButton()
    {
        SceneManager.LoadScene("BaseScene");
    }


    public void OnClickGameQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void OnClickHelpButton()
    {
        HelpCanvas.gameObject.SetActive(true);
    }


    public void OnClickRankingButton()
    {
        RankingCanvas.gameObject.SetActive(true);
    }
}

