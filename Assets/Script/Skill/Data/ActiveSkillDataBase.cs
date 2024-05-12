using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillDataBase", menuName = "Scriptable Object/Skill/ActiveSkillDataBase")]
public class ActiveSkillDataBase : ScriptableObject
{
    
}

public class ActiveSkillData
{
    public int skillNumber;
    public float skillChance;
    public float skillDamage;
    public float scopeRange;
    public float checkCooltime;
    public float checkUptime;
    public float skillCooltime;
    public float skillUptime;
    public bool canUse;
    public float skillRange;
    public SkillType skillType;
}