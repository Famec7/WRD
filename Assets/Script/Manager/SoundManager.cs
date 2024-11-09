using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    protected override void Init()
    {
        _bgmSource = CreateAudioSource("BGM");
        
        for (int i = 0; i < _sfxSourceCount; i++)
        {
            _sfxSources.Enqueue(CreateAudioSource($"SFX_{i}"));
        }
    }
    
    private AudioSource CreateAudioSource(string sourceName)
    {
        GameObject audioObject = new GameObject(sourceName);
        audioObject.transform.SetParent(transform);
        return audioObject.AddComponent<AudioSource>();
    }

    #region BGM
    
    private AudioSource _bgmSource;

    /// <summary>
    /// BGM을 재생하는 함수
    /// </summary>
    /// <param name="clip"> 재생할 AudioClip </param>
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        
        _bgmSource.clip = clip;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }
    
    /// <summary>
    /// BGM을 일시정지하는 함수
    /// </summary>
    public void PauseBGM()
    {
        _bgmSource.Pause();
    }

    /// <summary>
    /// BGM을 정지하는 함수
    /// </summary>
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    #endregion

    #region SFX

    private const int _sfxSourceCount = 10;
    private readonly Queue<AudioSource> _sfxSources = new Queue<AudioSource>();
    
    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        AudioSource sfxSource = _sfxSources.Dequeue();
        
        if (clip == null)
        {
            return;
        }
        
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
        
        _sfxSources.Enqueue(sfxSource);
    }
    
    public void StopSFX(AudioClip clip)
    {
        foreach (AudioSource sfxSource in _sfxSources)
        {
            if (sfxSource.clip == clip)
            {
                sfxSource.Stop();
            }
        }
    }
    
    public void StopAllSFX()
    {
        foreach (AudioSource sfxSource in _sfxSources)
        {
            sfxSource.Stop();
        }
    }

    #endregion
}