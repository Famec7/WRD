using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour
{
    private EventTrigger _trigger;

    [SerializeField] private Image _image;
    [SerializeField] private ActiveSkillBase _currentSkill;

    private void Awake()
    {
        _trigger = GetComponent<EventTrigger>();
    }

    private void Update()
    {
        if (_currentSkill != null && _currentSkill.IsCoolTime)
        {
            _image.fillAmount = _currentSkill.CurrentCoolTime / _currentSkill.Data.CoolTime;
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
    }

    public void RemoveSkill(ActiveSkillBase skill)
    {
        _currentSkill.OnButtonActivate -= SetActive;
        _currentSkill = null;

        // 트리거 초기화
        _trigger.triggers.Clear();
    }

    private void SetActive(bool active)
    {
        _trigger.enabled = active;
        GetComponent<Button>().interactable = active;
    }
}