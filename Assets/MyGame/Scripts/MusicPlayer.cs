using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] float _volume = 1f;

    private void Start()
    {
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
