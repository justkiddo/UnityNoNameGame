using UnityEngine;
using Zenject;

namespace root
{
    public class BossTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject bossWalls;
        private EnemyBoss _enemyBoss;


        [Inject]
        private void Construct(EnemyBoss enemyBoss)
        {
            _enemyBoss = enemyBoss;
        }

        private void Update()
        {
            DestroyWalls();
        }

        private void DestroyWalls()
        {
            if (_enemyBoss.isDead)
            {
                bossWalls.SetActive(false);
            }
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                _enemyBoss.startFight = true;
                bossWalls.SetActive(true);
            }    
        }


    }
}