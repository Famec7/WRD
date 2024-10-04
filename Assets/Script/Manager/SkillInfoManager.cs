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

        public SkillInfo(string name, string info, string type)
        {
            Name = name;
            Info = info;
            Type = type;
        }
    }

    public Dictionary<int, List<SkillInfo>> WeaponSkills;

    protected override void Init()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("DataTable/Skill/SkillInfo");
        WeaponSkills = new Dictionary<int, List<SkillInfo>>();

        for (int i = 0; i < data.Count; i++)
        {
            int weaponId = int.Parse(data[i]["weapon_id"].ToString());
            string skillName = data[i]["skill_name"].ToString();
            string skillInfo = data[i]["skill_info"].ToString();
            string skillType = data[i]["skill_type_1"].ToString();

            if (!WeaponSkills.ContainsKey(weaponId))
            {
                WeaponSkills[weaponId] = new List<SkillInfo>();
            }

            WeaponSkills[weaponId].Add(new SkillInfo(skillName, skillInfo, skillType));
        }
    }
}
