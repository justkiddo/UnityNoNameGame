using UnityEngine.Localization.Settings;
using Zenject;

namespace root
{
    public class GameplayController: IInitializable
    {
        private IUnityLocalization _localization;
        
        [Inject]
        private void Construct( IUnityLocalization localization)
        {
            _localization = localization;
        }

        public void Initialize()
        {
        }
    }
}