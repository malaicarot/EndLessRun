using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class PauseMenusController : MonoBehaviour
{

    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject pauseMenus;
    [SerializeField] GameObject ScorePanel;
    [SerializeField] TextMeshProUGUI GameOverText;
    public string mainGameSceneName = "GamePlay";
    public string mainMenusSceneName = "Start";
    public string EndMenusSceneName = "End";

    void Start()
    {

        if (SceneManager.GetActiveScene().name == "End")
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(mainGameSceneName);

    }

    public void MainMenus()
    {
        SceneManager.LoadScene(mainMenusSceneName);
    }

    public void GameOverMenus()
    {
        SceneManager.LoadScene(EndMenusSceneName);

    }

    public void ActivePauseMenus()
    {
        pauseMenus.SetActive(true);
        Pause();

    }

    void HidePauseMenus()
    {
        pauseMenus.SetActive(false);
    }

    void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        HidePauseMenus();
    }

    public void NewGame()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }

    private void GameOver()
    {
        GameOverText.text = $"Your Score: {ScoreManagement.score}";
    }

}
