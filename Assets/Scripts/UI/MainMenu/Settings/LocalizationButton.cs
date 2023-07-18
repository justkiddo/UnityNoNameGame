


using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalizationButton : MonoBehaviour
{
    [SerializeField] private Button ruButton;
    [SerializeField] private Button enButton;
    [SerializeField] private Locale ruLocale;
    [SerializeField] private Locale enLocale;

    private void Awake()
    {
        ruButton.onClick.AddListener(OnRuButtonClicked);
        enButton.onClick.AddListener(OnEnButtonClicked);
    }

    
    
    private void OnEnButtonClicked()
    {
        LocalizationSettings.SelectedLocale = enLocale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnRuButtonClicked()
    {
        LocalizationSettings.SelectedLocale = ruLocale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
