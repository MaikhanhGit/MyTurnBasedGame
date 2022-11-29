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
    
    [SerializeField] float _startGameDelay = .3f;


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
        AudioHelper.PlayClip2D(_onButtonClickSFX, 1);
    }

    public void OnButtonHoverSFX()
    {
        AudioHelper.PlayClip2D(_onButtonHoverSFX, 1);
    }

    public void OnVolumeButtonDownSFX()
    {
        AudioHelper.PlayClip2D(_onVolumeButtonDownSFX, .5f);
    }

    public void OnVolumeButtonUpSFX()
    {
        AudioHelper.PlayClip2D(_onVolumeButtonUpSFX, .5f);
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
}
