using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour
{
    private EventTrigger _trigger;
    private Image _iconImage;

    [SerializeField] private Image _coolTimeImage;
    [SerializeField] private ActiveSkillBase _currentSkill;

    private void Awake()
    {
        _trigger = GetComponent<EventTrigger>();
        _iconImage = GetComponent<Image>();
        
        _coolTimeImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_currentSkill != null && _currentSkill.IsCoolTime)
        {
            _coolTimeImage.fillAmount = _currentSkill.CurrentCoolTime / _currentSkill.Data.CoolTime;
            _coolTimeImage.gameObject.SetActive(_coolTimeImage.fillAmount > 0);
        }
        else
        {
            _coolTimeImage.fillAmount = 0;
            _coolTimeImage.gameObject.SetActive(false);
        }
    }

    public void SetSkill(ActiveSkillBase skill)
    {
        _currentSkill = skill;
        _currentSkill.OnButtonActivate += SetActive;

        // 스킬 버튼 땠을 때 발생하는 이벤트
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        entry.callback.AddListener((data) => { skill.UseSkill(); });

        // 위 이벤트를 트리거에 추가
        _trigger.triggers.Add(entry);
        
        // 아이콘 이미지 설정
        _iconImage.enabled = true;
        _iconImage.sprite = skill.SkillIcon;
    }

    public void RemoveSkill(ActiveSkillBase skill)
    {
        _currentSkill.OnButtonActivate -= SetActive;
        _currentSkill = null;

        // 트리거 초기화
        _trigger.triggers.Clear();
        
        // 아이콘 이미지 초기화
        _iconImage.enabled = false;
        _iconImage.sprite = null;
        
        // 쿨타임 이미지 초기화
        _coolTimeImage.fillAmount = 0;
        _coolTimeImage.gameObject.SetActive(false);
    }

    private void SetActive(bool active)
    {
        _trigger.enabled = active;
        GetComponent<Button>().interactable = active;
    }
}