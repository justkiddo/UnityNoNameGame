using root;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class RootInstaller : MonoInstaller
{
    [SerializeField] private MainMenu mainMenu;
    
    
    public override void InstallBindings()
    {
        Container.Bind<IUnityLocalization>().To<UnityLocalization>().AsSingle().NonLazy();
        Container.BindInterfacesTo<MainMenu>().FromInstance(mainMenu).AsSingle().NonLazy();
    }
}
