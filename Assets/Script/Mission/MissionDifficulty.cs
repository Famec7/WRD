using System;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    EASY = 0,
    NORMAL = 1,
    HARD = 2,
    Empty = -1,
}

public static class MissionDifficulty
{
    /// <summary>
    ///  Get the difficulty of the mission
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Difficulty GetMissionDifficulty(int index)
    {
        WeaponTuple<WeaponTier, int> highestWeapon = InventoryManager.instance.GetHighestTier();
        WeaponTuple<WeaponTier, int> requiredGrade = MissionDataManager.Instance.GetRequiredGrade(index);
        List<WeaponTuple<WeaponTier, int>> easyRequiredGrades = MissionDataManager.Instance.GetEasyRequiredGrade(index);
        
        // 2 : Hard, 1 : Normal, 0 : Easy
        Difficulty difficulty = Difficulty.Empty;
        
        // 쉬움 난이도 등급 이상의 무기를 가지고 있을 경우
        foreach (var easyRequiredGrade in easyRequiredGrades)
        {
            if (highestWeapon.grade >= easyRequiredGrade.grade)
            {
                if (highestWeapon.count >= easyRequiredGrade.count)
                    difficulty = Difficulty.EASY;
                else
                    difficulty = Difficulty.NORMAL;
            }
            else
                difficulty = Difficulty.NORMAL;
        }

        // 적정 등급 이상의 무기를 가지고 있을 경우
        if (highestWeapon.grade >= requiredGrade.grade)
        {
            if (highestWeapon.count >= requiredGrade.count)
                difficulty = Difficulty.NORMAL;
            else
                difficulty = Difficulty.HARD;
        }
        else
            difficulty = Difficulty.HARD;
        return difficulty;
    }
}