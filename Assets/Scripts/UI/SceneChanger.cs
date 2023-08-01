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
    
    private bool _clickBlocker = false;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        _clickBlocker = false;
    }

    private void OnBackButtonClick()
    {
        if (_clickBlocker == false)
        {
            _clickBlocker = true;
            StartCoroutine(ClickAwait());
            SceneManager.LoadScene("Scenes/MainMenuScene");
            Time.timeScale = 1f;
            _clickBlocker = false;
        }
    }

    private void OnQuitButtonsClick()
    {
        if (_clickBlocker == false)
        {
            _clickBlocker = true;
            StartCoroutine(ClickAwait());
            Application.Quit();
            _clickBlocker = false;
        }
    }


    private void OnStartButtonsClick()
    {
        if (_clickBlocker == false)
        {
            _clickBlocker = true;
            StartCoroutine(ClickAwait());
        SceneManager.LoadScene("Scenes/GameScene");
        _clickBlocker = false;
        Time.timeScale = 1f;
        }
    }

    private IEnumerator ClickAwait()
    {
        yield return new WaitForSeconds(1);
        
    }
}
