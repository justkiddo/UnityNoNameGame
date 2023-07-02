using root;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class EndGamePanel : MonoBehaviour, IInitializable
{
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI mainMenuText;
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
        restartText.text = _localization.Translate("endgame.restart");
        mainMenuText.text = _localization.Translate("endgame.main.menu");
        quitText.text = _localization.Translate("endgame.quit");
    }
    
    private void OnDestroy()
    {
        _disposable.Dispose();
    }
}
