using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace root
{
    public class MainMenuSettings : MonoBehaviour
    {
        [SerializeField] private Button ruButton;
        [SerializeField] private Button enButton;
        [SerializeField] private Locale ruLocale;
        [SerializeField] private Locale enLocale;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private AudioSource audioSource;
        
        private void Awake()
        {
            ruButton.onClick.AddListener(OnRuButtonClicked);
            enButton.onClick.AddListener(OnEnButtonClicked);
        }

        private void Update()
        {
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float volume)
        {
            audioSource.volume = volume;
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
}