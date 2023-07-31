using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace root
{
    public class GameplayInstaller : MonoInstaller
    {

        [SerializeField] private List<EnemyInfo> enemyInfos;
        [SerializeField] private List<PlayerHitCollider> playerHitColliders;
        [SerializeField] private List<BossInfo> _bossInfos;
        [SerializeField] private Enemy enemy;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private PlayerInfo playerInfo;
        [SerializeField] private EndGamePanel endgamePanel;
        [SerializeField] private AudioSystem audioSystem;
        [SerializeField] private EnemyBoss boss;
        [SerializeField] private BossTrigger bossTrigger;
        [SerializeField] private PauseMenu pauseMenu;

        public override void InstallBindings()
        {
            Container.Bind<BossTrigger>().FromInstance(bossTrigger).AsSingle().NonLazy();
            Container.Bind<EnemyBoss>().FromInstance(boss).AsSingle().NonLazy();
            Container.Bind<IUnityLocalization>().To<UnityLocalization>().AsSingle().NonLazy();
            Container.Bind<PlayerInfo>().FromInstance(playerInfo);
            Container.Bind<Enemy>().FromInstance(enemy).AsSingle().NonLazy();
            Container.Bind<IPlayer>().FromInstance(playerPrefab);
            Container.BindInterfacesTo<EndGamePanel>().FromInstance(endgamePanel).AsSingle().NonLazy();
            Container.Bind<AudioSystem>().FromInstance(audioSystem).AsSingle().NonLazy();
            Container.Bind<PauseMenu>().FromInstance(pauseMenu).AsSingle().NonLazy();
            foreach (var playerHitCollider in playerHitColliders)
            {
                Container.Bind<PlayerHitCollider>().FromInstance(playerHitCollider);
            }
            foreach (var enemyInfo in enemyInfos)
            {
                Container.Bind<EnemyInfo>().FromInstance(enemyInfo);
            }
            foreach (var bossInfo in _bossInfos)
            {
                Container.Bind<BossInfo>().FromInstance(bossInfo);
            }
            
        }
    }
}