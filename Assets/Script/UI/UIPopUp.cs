using UnityEngine;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour
{
    [Header("닫기 버튼")]
    [SerializeField]
    protected Button closeButton;
    
    /// <summary>
    /// 팝업 초기화
    /// 팝업창과 관련된 초기화 작업을 수행한다.
    /// </summary>
    public virtual void Init()
    {
        this.gameObject.SetActive(true);
        SetClosePopUp();
    }
    
    /// <summary>
    /// 닫기 버튼 이벤트 설정
    /// 닫기 버튼을 눌렀을 때 팝업을 닫는 이벤트를 설정한다.
    /// 이후 이벤트를 추가하고 싶다면 이 메서드를 오버라이드하여 사용한다.
    /// </summary>
    protected virtual void SetClosePopUp()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.instance.ClosePopUp();
        });
    }
}