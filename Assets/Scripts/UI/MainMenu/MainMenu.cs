using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace root
{
    public class MainMenu : MonoBehaviour, IInitializable
    {
        
        [SerializeField] private TextMeshProUGUI startText;
        [SerializeField] private TextMeshProUGUI quitText;
        [SerializeField] private TextMeshProUGUI controlsText;
        [SerializeField] private TextMeshProUGUI menuButtonText;
        [SerializeField] private TextMeshProUGUI controlsButtonText;
        [SerializeField] private TextMeshProUGUI settingsButtonText;
        [SerializeField] private TextMeshProUGUI settingsRuLocaleText;
        [SerializeField] private TextMeshProUGUI settingsEnLocaleText;
        

        
        
        private CompositeDisposable _disposable;
        private IUnityLocalization _localization;

        [Inject]
        private void Construct(IUnityLocalization localization)
        {
            _localization = localization;
        }
        
        public void Initialize()
        {
            _disposable = new CompositeDisposable();
            AddListeners();
            
        }


        private void AddListeners()
        {
            startText.text = _localization.Translate("start.button");
            quitText.text = _localization.Translate("quit.button");
            controlsText.text = _localization.Translate("settings.controls");
            menuButtonText.text = _localization.Translate("tab.menu");
            controlsButtonText.text = _localization.Translate("tab.controls");
            settingsButtonText.text = _localization.Translate("tab.settings");
            settingsRuLocaleText.text = _localization.Translate("settings.locales.ru");
            settingsEnLocaleText.text = _localization.Translate("settings.locales.en");
        }



        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}