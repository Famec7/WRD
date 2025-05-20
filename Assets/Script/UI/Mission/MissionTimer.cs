using UnityEngine;
using TMPro;
using System;

public class MissionTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // 타이머를 표시할 TextMeshProUGUI
    private float currentTime;
    private bool isRunning;
    private int missionIndex;
    public event Action OnTimerEnd; // 타이머 종료 시 호출되는 이벤트


    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                // 타이머 종료
                currentTime = 0;
                MissionManager.Instance.missionInfo._missionSlots[missionIndex].Clear(false);
                isRunning = false;
                gameObject.SetActive(false);

                OnTimerEnd?.Invoke();
            }

            UpdateTimerText();
        }
    }

    // 타이머 시작 메서드
    public void StartTimer(float currentTime, int missionIndex)
    {
        this.currentTime = currentTime;
        this.missionIndex = missionIndex;
        isRunning = true;
        gameObject.SetActive(true);
        UpdateTimerText();
    }

    // 타이머 리셋 메서드
    public void ResetTimer()
    {
        currentTime = 0f;
        isRunning = false;
        gameObject.SetActive(false);
    }

    // 타이머 텍스트 업데이트 메서드
    private void UpdateTimerText()
    {
        string timeText = currentTime.ToString("F2");
        timerText.text = $"미션 {missionIndex + 1}\n({timeText}초)";
        
    }
}
