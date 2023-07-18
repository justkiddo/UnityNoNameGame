using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace root
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInfo playerInfo;
        [SerializeField] private List<EnemyInfo> enemyInfos;
        [SerializeField] private Enemy enemy;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private List<PlayerHitCollider> playerHitColliders;
        
        private IPlayer _playerPrefab;
        [SerializeField] private EndGamePanel endgamePanel;
        [SerializeField] private AudioSystem audioSystem;


        public override void InstallBindings()
        {
            Container.Bind<IUnityLocalization>().To<UnityLocalization>().AsSingle().NonLazy();
            Container.Bind<PlayerInfo>().FromInstance(playerInfo);
            Container.Bind<Enemy>().FromInstance(enemy).AsSingle().NonLazy();
            Container.Bind<IPlayer>().FromInstance(playerPrefab);
            Container.BindInterfacesTo<EndGamePanel>().FromInstance(endgamePanel).AsSingle().NonLazy();
            Container.Bind<AudioSystem>().FromInstance(audioSystem).AsSingle().NonLazy();
            foreach (var pCollider in playerHitColliders)
            {
                Container.Bind<PlayerHitCollider>().FromInstance(pCollider);
            }
            foreach (var enemyInfo in enemyInfos)
            {
                Container.Bind<EnemyInfo>().FromInstance(enemyInfo);
            }
        }
    }
}