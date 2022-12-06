using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : SingletonMB<MusicPlayer>
{       
    AudioSource _audioSource01;
    AudioSource _audioSource02;
    float _musicVolume = .3f;
    float _ambienceVolume = .5f;

    private void Awake()
    {
        _audioSource01 = gameObject.AddComponent<AudioSource>();
        _audioSource01.loop = true;
        _audioSource02 = gameObject.AddComponent<AudioSource>();
        _audioSource02.loop = true;
    }

    // this music player is specialized to play 2 audio clips at once
   public void PlayNewSong(AudioClip newSong01, AudioClip newSong02, float volume01, float volume02)
    {
        if (newSong01 == null && newSong02 == null) return;    // guard clause

        if(newSong01 != null)
        {
            _audioSource01.clip = newSong01;
            _audioSource01.volume = volume01;
            _audioSource01.Play();
        }
        if (newSong02 != null)
        {
            _audioSource02.clip = newSong02;
            _audioSource02.volume = volume02;
            _audioSource02.Play();
        }


    }

    private void Update()
    {        
        _audioSource01.volume = _musicVolume;
        _audioSource02.volume = _ambienceVolume;
    }

    public void UpdateMusicVolume(float volume)
    {
        _musicVolume = volume;        
    }

    public void UpdateAmbienceVolume(float volume)
    {
        _ambienceVolume = volume;
    }
}
