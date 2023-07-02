using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button restartButton;
    private bool _clickBlocker = false;

    private void Awake()
    {
        startButton?.onClick.AddListener(OnStartButtonsClick);
        settingsButton?.onClick.AddListener(OnSettingsButtonsClick);
        quitButton?.onClick.AddListener(OnQuitButtonsClick);
        backButton?.onClick.AddListener(OnBackButtonClick);
        restartButton?.onClick.AddListener(OnRestartButtonClick);
    }

    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBackButtonClick()
    {
        if (_clickBlocker == false)
        {
            _clickBlocker = true;
            StartCoroutine(ClickAwait());
            SceneManager.LoadScene("Scenes/MainMenuScene");
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

    private void OnSettingsButtonsClick()
    {
        if (_clickBlocker == false)
        {
            _clickBlocker = true;
            StartCoroutine(ClickAwait());
        SceneManager.LoadScene("Scenes/SettingsScene");
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
        }
    }

    private IEnumerator ClickAwait()
    {
        yield return new WaitForSeconds(5);
    }
}
