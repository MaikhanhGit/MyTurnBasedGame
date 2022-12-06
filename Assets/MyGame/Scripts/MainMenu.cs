using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioClip _onButtonClickSFX;
    [SerializeField] AudioClip _onButtonHoverSFX;
    [SerializeField] AudioClip _onVolumeButtonDownSFX;
    [SerializeField] AudioClip _onVolumeButtonUpSFX;
    [SerializeField] AudioClip _backgroundMusic;
    [SerializeField] AudioClip _ambience;
    [SerializeField] float _bgMusicVolume = .3f;
    [SerializeField] float _ambienceVolume = .5f;
    [SerializeField] float _startGameDelay = .3f;

    private void Start()
    {
        MusicPlayer.Instance.PlayNewSong(_backgroundMusic, _ambience, _bgMusicVolume, _ambienceVolume);        
    }
       

    public void StartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(StartGameDelay(_startGameDelay));
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnButtonClickSFX()
    {
        if (_onButtonClickSFX)
        {
            AudioHelper.PlayClip2D(_onButtonClickSFX, 1);
        }        
    }

    public void OnButtonHoverSFX()
    {
        if (_onButtonHoverSFX)
        {
            AudioHelper.PlayClip2D(_onButtonHoverSFX, 1);
        }        
    }

    public void OnVolumeButtonDownSFX()
    {
        AudioHelper.PlayClip2D(_onVolumeButtonDownSFX, .5f);
    }

    public void OnVolumeButtonUpSFX()
    {
        if (_onVolumeButtonUpSFX)
        {
            AudioHelper.PlayClip2D(_onVolumeButtonUpSFX, .5f);
        }
    }

    public void QuitGame()
    {       
        Application.Quit();
    }

    IEnumerator StartGameDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeMusicVolume(float newVolume)
    {
        MusicPlayer.Instance.UpdateMusicVolume(newVolume);
    }

    public void ChangeAmbienceVolume(float newVolume)
    {
        MusicPlayer.Instance.UpdateAmbienceVolume(newVolume);
    }
}
