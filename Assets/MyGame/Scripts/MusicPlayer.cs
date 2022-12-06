using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : SingletonMB<MusicPlayer>
{       
    AudioSource _audioSource;
    float _volume = .5f;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
    }

   public void PlayNewSong(AudioClip newSong)
    {
        if (newSong == null) return;    // guard clause

        _audioSource.clip = newSong;        
        _audioSource.Play();
    }

    private void Update()
    {
        _audioSource.volume = _volume;
    }

    public void UpdateVolume(float volume)
    {
        _volume = volume;        
    }
}
