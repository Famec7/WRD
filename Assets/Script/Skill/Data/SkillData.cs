using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillData
{
    private List<float> _values = new();

    public string Name { get; set; }

    private float _range;
    public float Range { get => _range; set => _range = value; }


    public float GetValue(int index)
    {
        if (index < 0 || index >= _values.Count)
        {
            Debug.LogError($"{Name}'s {index} is out of range");
        }
        
        return _values[index];
    }
    
    public void AddValue(float value)
    {
        _values.Add(value);
    }

}

[Serializable]
public class PassiveSkillData : SkillData
{
    private int _chance;

    public int Chance
    {
        get => _chance;
        set => _chance = value;
    }
}

[Serializable]
public class PassiveAuraSkillData : SkillData
{
    // PassiveAuraSkill은 SkillData와 동일한 구조이므로 별도의 변수 없음
}

[Serializable]
public class ActiveSkillData : SkillData
{
    private float _coolTime;

    public float CoolTime
    {
        get => _coolTime;
        set => _coolTime = value;
    }

    private float _availableRange;
    public float AvailableRange { get => _availableRange; set => _availableRange = value; }
    
    public enum ActiveType
    {
        none,
        immediate,
        click,
        doubleClick,
        subordinate
    }

    private ActiveType _type;
    
    public ActiveType Type
    {
        get => _type;
        set => _type = value;
    }
}

