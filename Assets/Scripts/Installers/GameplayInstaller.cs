using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace root
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInfo playerInfo;
        [SerializeField] private List<EnemyInfo> enemyInfos;
        [SerializeField] private Enemy enemy;
        [SerializeField] private Player playerPrefab;
        private IPlayer _playerPrefab;
        

        public override void InstallBindings()
        {
            Container.Bind<PlayerInfo>().FromInstance(playerInfo);
            Container.Bind<Enemy>().FromInstance(enemy).AsSingle().NonLazy();
            Container.Bind<IPlayer>().FromInstance(playerPrefab);
            foreach (var enemyInfo in enemyInfos)
            {
                Container.Bind<EnemyInfo>().FromInstance(enemyInfo);
            }
        }
    }
}