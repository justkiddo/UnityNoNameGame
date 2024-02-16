using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject pauseMenu;
    

    private void Awake()
    {
        startButton?.onClick.AddListener(OnStartButtonsClick);
        quitButton?.onClick.AddListener(OnQuitButtonsClick);
        backButton?.onClick.AddListener(OnBackButtonClick);
        restartButton?.onClick.AddListener(OnRestartButtonClick);
        continueButton?.onClick.AddListener(OnContinueButtonClick);
        
    }

    private void OnContinueButtonClick()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
        // additive
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    private void OnBackButtonClick()
    {
            SceneManager.LoadScene("MainMenuScene");
            Time.timeScale = 1f;
    }

    private void OnQuitButtonsClick()
    {
            Application.Quit();
    }


    private void OnStartButtonsClick()
    {
            //additive
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}

