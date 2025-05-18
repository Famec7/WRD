using UnityEngine;

public abstract class SceneBase : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }
    
    private void OnDestroy()
    {
        Cleanup();
    }

    /// <summary>
    /// 씬을 초기화합니다. (Awake에서 호출)
    /// </summary>
    protected abstract void Initialize();

    /// <summary>
    /// 씬을 정리합니다. (OnDestroy에서 호출)
    /// </summary>
    protected abstract void Cleanup();
}