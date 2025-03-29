using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour
{
    private EventTrigger _trigger;
    private Image _iconImage;
    private float _pressStartTime;
    private bool _isLongPress;
    private const float LongPressDuration = 1.0f;
    
    private SkillBase _currentSkill;
    [SerializeField] private SkillDescription _descriptionPopup;
    [SerializeField] private CoolTimeUI _coolTimeUI;
    [SerializeField] private Image _outlineImage;

    private void Awake()
    {
        _trigger = GetComponent<EventTrigger>();
        _iconImage = GetComponent<Image>();
    }

    public void SetSkill(SkillBase skill)
    {
        _currentSkill = skill;
        
        if (_currentSkill == null)
        {
            return;
        }

        // 아이콘 이미지 설정
        _iconImage.enabled = true;
        _iconImage.sprite = skill.SkillIcon;

        // 스킬 버튼 땠을 때 발생하는 이벤트
        var pointerDownEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown(); });

        var pointerUpEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUpEntry.callback.AddListener((data) => { OnPointerUp(); });

        // 위 이벤트를 트리거에 추가
        _trigger.triggers.Add(pointerDownEntry);
        _trigger.triggers.Add(pointerUpEntry);
        
        if (_currentSkill is ActiveSkillBase activeSkill)
        {
            _coolTimeUI.SetSkill(activeSkill);
        }
        else
        {
            _outlineImage.enabled = true;
        }
    }

    public void RemoveSkill(SkillBase skill)
    {
        // 트리거 초기화
        _trigger.triggers.Clear();

        // 아이콘 이미지 초기화
        _iconImage.enabled = false;
        _iconImage.sprite = null;
        
        _outlineImage.enabled = false;
        _coolTimeUI.SetSkill(null);

        _currentSkill = null;
    }

    private void OnPointerDown()
    {
        _pressStartTime = Time.time;
        _isLongPress = false;
        Invoke(nameof(ShowDescriptionPopup), LongPressDuration);
    }

    private void OnPointerUp()
    {
        CancelInvoke(nameof(ShowDescriptionPopup));
        _descriptionPopup.HideDescription();

        if (_isLongPress)
        {
            return;
        }

        float pressDuration = Time.time - _pressStartTime;
        if (pressDuration < LongPressDuration)
        {
            if (_currentSkill is ActiveSkillBase activeSkill)
            {
                if (activeSkill.CurrentCoolTime > 0)
                {
                    return;
                }
                activeSkill.UseSkill();
            }
        }
    }

    private void ShowDescriptionPopup()
    {
        switch (_currentSkill)
        {
            case ActiveSkillBase activeSkill:
                _descriptionPopup.SetSkill(activeSkill.Data);
                break;
            case PassiveSkillBase passiveSkill:
                _descriptionPopup.SetSkill(passiveSkill.Data);
                break;
            case PassiveAuraSkillBase passiveAuraSkill:
                _descriptionPopup.SetSkill(passiveAuraSkill.Data);
                break;
        }

        _descriptionPopup.ShowDescription();
        _isLongPress = true;
    }
}