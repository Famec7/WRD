using UnityEngine;

public class LobbyScene : SceneBase
{
    [Header("로비 BGM")]
    [SerializeField]
    private AudioClip bgm;
    
    protected override void Initialize()
    {
        if (bgm != null)
        {
           SoundManager.Instance.PlayBGM(bgm);
        }
        
        Application.targetFrameRate = 60;
    }

    protected override void Cleanup()
    {
        
    }
}