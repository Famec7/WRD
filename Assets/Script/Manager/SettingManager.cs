public class SettingManager : Singleton<SettingManager>
{
    protected override void Init()
    {
        CurrentActiveSettingType = ActiveSettingType.Auto;
    }
    
    /********************************Active Skill Setting********************************/
    public enum ActiveSettingType
    {
        // 자동, 반자동, 수동
        Auto,
        SemiAuto,
        Manual,
    }
    
    public ActiveSettingType CurrentActiveSettingType { get; set; }
}