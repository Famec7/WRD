using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoManager : Singleton<SkillInfoManager>
{
    public class SkillInfo
    {
        public string Name;
        public string Info;
        public string Type;
        public string CoolTime;

        public SkillInfo(string name, string info, string type, string coolTime)
        {
            Name = name;
            Info = info;
            Type = type;
            CoolTime = coolTime;
        }
    }

    public Dictionary<int, List<SkillInfo>> WeaponSkills;

    protected override void Init()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("DataTable/Skill/SkillInfo");
        WeaponSkills = new Dictionary<int, List<SkillInfo>>();

        for (int i = 0; i < data.Count; i++)
        {
            int weaponNum = int.Parse(data[i]["weapon_num"].ToString());
            string skillName = data[i]["skill_name"].ToString();
            string skillInfo = data[i]["skill_info"].ToString();
            string skillType = data[i]["skill_type_1"].ToString();
            string coolTime = data[i]["skill_cooltime"].ToString();
            if (!WeaponSkills.ContainsKey(weaponNum))
            {
                WeaponSkills[weaponNum] = new List<SkillInfo>();
            }

            WeaponSkills[weaponNum].Add(new SkillInfo(skillName, skillInfo, skillType,coolTime));
        }
    }
}
