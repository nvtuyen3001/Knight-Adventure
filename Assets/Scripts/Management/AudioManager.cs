using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip swordSwingSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip playerHurtSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip sceneTransitionSound;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip buttonClickSound;
    
    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioClip defeatMusic;
    
    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 0.8f;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Create audio sources if not assigned
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("Music Source");
            musicObj.transform.parent = transform;
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }
        
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFX Source");
            sfxObj.transform.parent = transform;
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.volume = sfxVolume;
        }
    }
    
    private void Start()
    {
        // Play appropriate music based on current scene
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if (currentScene == "Win")
        {
            PlayMusic(mainMenuMusic);
        }
        else if (currentScene == "Die")
        {
            PlayMusic(defeatMusic);
        }
        else if (currentScene == "Scene3") // Assuming Scene3 is boss scene
        {
            PlayMusic(bossMusic);
        }
        else
        {
            PlayMusic(gameplayMusic);
        }
    }
    
    // Play a sound effect once
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }
    
    // Change background music
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null && musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }
    
    // Convenience methods for common sound effects
    public void PlaySwordSwing()
    {
        PlaySFX(swordSwingSound);
    }
    
    public void PlayDash()
    {
        PlaySFX(dashSound);
    }
    
    public void PlayPlayerHurt()
    {
        PlaySFX(playerHurtSound);
    }
    
    public void PlayPlayerDeath()
    {
        PlaySFX(playerDeathSound);
    }
    
    public void PlaySceneTransition()
    {
        PlaySFX(sceneTransitionSound);
    }
    
    public void PlayEnemyHit()
    {
        PlaySFX(enemyHitSound);
    }
    
    public void PlayEnemyDeath()
    {
        PlaySFX(enemyDeathSound);
    }
    
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }
    
    // Change volume settings
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
    
    // Stop all sounds
    public void StopAllSounds()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
        
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }
} 