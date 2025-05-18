using UnityEngine;

public class GameScene : SceneBase
{
    [SerializeField]
    private AudioClip bgm;
    
    protected override void Initialize()
    {
        if (bgm != null)
        {
            SoundManager.Instance.PlayBGM(bgm);
        }
    }

    protected override void Cleanup()
    {
        ;
    }
}