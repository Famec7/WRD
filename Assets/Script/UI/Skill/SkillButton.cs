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

    [SerializeField] private Image _coolTimeImage;
    [SerializeField] private SkillBase _currentSkill;
    [SerializeField] private SkillDescription _descriptionPopup;

    private void Awake()
    {
        _trigger = GetComponent<EventTrigger>();
        _iconImage = GetComponent<Image>();

        _coolTimeImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        ActiveSkillBase activeSkill = _currentSkill as ActiveSkillBase;
        if (activeSkill == null)
        {
            return;
        }

        if (_currentSkill != null)
        {
            _coolTimeImage.fillAmount = activeSkill.CurrentCoolTime / activeSkill.Data.CoolTime;
            _coolTimeImage.gameObject.SetActive(_coolTimeImage.fillAmount > 0);
        }
        else
        {
            _coolTimeImage.fillAmount = 0;
            _coolTimeImage.gameObject.SetActive(false);
        }
    }

    public void SetSkill(SkillBase skill)
    {
        _currentSkill = skill;

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

        ActiveSkillBase activeSkill = _currentSkill as ActiveSkillBase;
        if (activeSkill != null)
        {
            activeSkill.OnButtonActivate += SetActive;
        }
    }

    public void RemoveSkill(SkillBase skill)
    {
        ActiveSkillBase activeSkill = _currentSkill as ActiveSkillBase;
        if (activeSkill != null)
        {
            activeSkill.OnButtonActivate -= SetActive;
        }

        // 트리거 초기화
        _trigger.triggers.Clear();

        // 아이콘 이미지 초기화
        _iconImage.enabled = false;
        _iconImage.sprite = null;

        // 쿨타임 이미지 초기화
        _coolTimeImage.fillAmount = 0;
        _coolTimeImage.gameObject.SetActive(false);

        _currentSkill = null;
    }

    private void SetActive(bool active)
    {
        _trigger.enabled = active;
        GetComponent<Button>().interactable = active;
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
                activeSkill.UseSkill();
            }
        }
    }

    private void ShowDescriptionPopup()
    {
        _descriptionPopup.SetSkill(_currentSkill);
        _descriptionPopup.ShowDescription();
        _isLongPress = true;
    }
}