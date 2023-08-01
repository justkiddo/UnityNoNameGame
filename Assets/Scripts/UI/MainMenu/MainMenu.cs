using UniRx;
using UnityEngine;
using Zenject;
using TMPro;

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
            startText.text = _localization.Translate(BaseIds.StartButton);
            quitText.text = _localization.Translate(BaseIds.QuitButton);
            controlsText.text = _localization.Translate(BaseIds.SettingsControls);
            menuButtonText.text = _localization.Translate(BaseIds.TabMenu);
            controlsButtonText.text = _localization.Translate(BaseIds.TabControls);
            settingsButtonText.text = _localization.Translate(BaseIds.TabSettings);
            settingsRuLocaleText.text = _localization.Translate(BaseIds.SettingsLocalesRu);
            settingsEnLocaleText.text = _localization.Translate(BaseIds.SettingsLocalesEn);
        }



        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}