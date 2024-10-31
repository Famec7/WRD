using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : Singleton<IndicatorManager>
{
    [SerializeField] private GameObject _usualbleIndicator;

    [SerializeField] private List<SkillIndicator> _skillIndicators;

    public enum Type
    {
        Circle,
        Triangle,
        Square,
        Count
    }

    protected override void Init()
    {
        ;
    }

    public void ShowUsableIndicator(Vector2 position, float size)
    {
        _usualbleIndicator.transform.position = position;
        _usualbleIndicator.transform.localScale = new Vector3(size, size, 1);
        _usualbleIndicator.SetActive(true);
    }

    public void HideUsableIndicator()
    {
        _usualbleIndicator.SetActive(false);
    }

    public void ShowIndicator(Vector3 position, Type type, bool isFixedPosition = false)
    {
        SkillIndicator indicator = GetIndicator(type);
        indicator.ShowIndicator(position, isFixedPosition);
    }

    public void HideIndicator(Type type)
    {
        GetIndicator(type).HideIndicator();
    }

    public SkillIndicator GetIndicator(Type type)
    {
        if (_skillIndicators[(int)type] is null)
        {
#if UNITY_EDITOR
            Debug.LogError("Indicator is null");
#endif

            return null;
        }

        return _skillIndicators[(int)type];
    }
}