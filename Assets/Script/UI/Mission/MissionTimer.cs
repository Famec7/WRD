using UnityEngine;
using TMPro;

public class MissionTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // 타이머를 표시할 TextMeshProUGUI
    private float currentTime;
    private bool isRunning;

    void Start()
    {
        // 타이머 초기화
        ResetTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                // 타이머 종료
                currentTime = 0;
                isRunning = false;
                timerText.gameObject.SetActive(false); // 타이머 텍스트 비활성화
            }

            UpdateTimerText();
        }
    }

    // 타이머 시작 메서드
    public void StartTimer(float currentTime)
    {
        this.currentTime = currentTime; // 10초로 설정
        isRunning = true;
        timerText.gameObject.SetActive(true); // 타이머 텍스트 활성화
        UpdateTimerText();
    }

    // 타이머 리셋 메서드
    public void ResetTimer()
    {
        currentTime = 0f;
        isRunning = false;
        timerText.gameObject.SetActive(false);
    }

    // 타이머 텍스트 업데이트 메서드
    private void UpdateTimerText()
    {
        timerText.text = Mathf.Ceil(currentTime).ToString(); // 남은 시간을 정수로 표시
    }
}
