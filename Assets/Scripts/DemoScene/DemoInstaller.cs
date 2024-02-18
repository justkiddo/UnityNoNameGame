using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace root
{
    public class DemoInstaller : MonoInstaller
    {
        [SerializeField] private List<DemoEnemyInfo> enemyInfos;
        [SerializeField] private DemoEnemyBandit enemy;
        [SerializeField] private DemoPlayer playerPrefab;
        [SerializeField] private DemoPlayerInfo playerInfo;

        public override void InstallBindings()
        {
            Container.Bind<DemoPlayerInfo>().FromInstance(playerInfo);
            Container.Bind<DemoEnemyBandit>().FromInstance(enemy).AsSingle().NonLazy();
            Container.Bind<IDemoPlayer>().FromInstance(playerPrefab);
            
            foreach (var enemyInfo in enemyInfos)
            {
                Container.Bind<DemoEnemyInfo>().FromInstance(enemyInfo);
            }
        }
    
    
    }
}