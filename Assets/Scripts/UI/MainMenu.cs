using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace root
{
    public class MainMenu : MonoBehaviour, IInitializable
    {
        
        [SerializeField] private TextMeshProUGUI startText;
        [SerializeField] private TextMeshProUGUI settingsText;
        [SerializeField] private TextMeshProUGUI quitText;
        
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
            settingsText.text = _localization.Translate("settings.button");
            quitText.text = _localization.Translate("quit.button");
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}