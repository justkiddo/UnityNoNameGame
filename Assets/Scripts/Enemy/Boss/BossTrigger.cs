using UnityEngine;
using Zenject;

namespace root
{
    public class BossTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject bossWalls;
        private Player _player;
        private EnemyBoss _enemyBoss;


        [Inject]
        private void Construct(EnemyBoss enemyBoss)
        {
            _enemyBoss = enemyBoss;
        }
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                _enemyBoss.startFight = true;
                bossWalls.SetActive(true);
            }    
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                bossWalls.SetActive(false);
            }
        }
    }
}