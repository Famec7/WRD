using UnityEngine;

public class Fixed : MonoBehaviour
{
    public int setWidth = 1080; // 사용자 설정 너비
    public int setHeight = 1920; // 사용자 설정 높이

    private void Start()
    {
        SetResolution(); // 초기에 게임 해상도 고정
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        if (deviceWidth > deviceHeight)
            Screen.SetResolution(setWidth,setHeight,true); // SetResolution 함수 제대로 사용하기

    }
}