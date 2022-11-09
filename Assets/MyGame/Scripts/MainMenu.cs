using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioClip _onButtonClickSFX;
    [SerializeField] float _startGameDelay = .3f;
   public void StartGame()
    {
        StartCoroutine(StartGameDelay(_startGameDelay));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnButtonClickSFX()
    {
        AudioHelper.PlayClip2D(_onButtonClickSFX, 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator StartGameDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
