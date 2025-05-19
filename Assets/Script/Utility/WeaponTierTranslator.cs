using UnityEngine;

public class WeaponTierTranslator
{
    public static string TranslateToKorean(WeaponTier tier)
    {
        switch (tier)
        {
            case WeaponTier.Normal:
                return "흔함";
            case WeaponTier.UNNORMAL:
                return "고급";
            case WeaponTier.RARE:
                return "희귀";
            case WeaponTier.EPIC:
                return "영웅";
            case WeaponTier.LEGENDARY:
                return "전설";
            case WeaponTier.MYTH:
                return "신화";
            case WeaponTier.MODIFIED:
                return "변이";
            default:
                return "알 수 없음";
        }
    }

    public static Color32 GetClassColor(string classStr)
    {
        Color32 color = new Color32(56, 56, 56, 255);

        switch (classStr)
        {
            case "normal":
                color = new Color32(0, 0, 0, 255);
                break;
            case "unnormal":
                color = new Color32(84, 130, 53, 255);
                break;
            case "rare":
                color = new Color32(68, 114, 196, 255);
                break;
            case "epic":
                color = new Color32(112, 48, 160, 255);
                break;
            case "legendary":
                color = new Color32(255, 192, 0, 255);
                break;
            case "myth":
                color = new Color32(255, 255, 255, 255);
                break;
        }

        return color;
    }

    public static Color32 GetClassColor(WeaponTier tier)
    {
        Color32 color = new Color32(56, 56, 56, 255);

        switch (tier)
        {
            case WeaponTier.Normal:
                color = new Color32(0, 0, 0, 255);
                break;
            case WeaponTier.UNNORMAL:
                color = new Color32(84, 130, 53, 255);
                break;
            case WeaponTier.RARE:
                color = new Color32(68, 114, 196, 255);
                break;
            case WeaponTier.EPIC:
                color = new Color32(112, 48, 160, 255);
                break;
            case WeaponTier.LEGENDARY:
                color = new Color32(255, 192, 0, 255);
                break;
            case WeaponTier.MYTH:
                color = new Color32(255, 255, 255, 255);
                break;
        }

        return color;
    }
}