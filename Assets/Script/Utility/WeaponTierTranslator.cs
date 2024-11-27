public class WeaponTierTranslator
{
    public static string TranslateToKorean(WeaponTier tier)
    {
        switch (tier)
        {
            case WeaponTier.Element:
                return "흔함";
            case WeaponTier.UNNORMAL:
                return "안흔함";
            case WeaponTier.RARE:
                return "특별함";
            case WeaponTier.EPIC:
                return "희귀함";
            case WeaponTier.LEGENDARY:
                return "전설";
            case WeaponTier.MYTH:
                return "신화";
            default:
                return "알 수 없음";
        }
    }
}