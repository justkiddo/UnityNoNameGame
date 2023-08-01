using System.Collections.Generic;
using UnityEngine;

namespace root
{
    public class PlayerHitCollider : MonoBehaviour
    {
        public List<Enemy> enemies;
        public List<EnemyBoss> enemyBosses;
        public bool enemy;
        public bool boss;
        


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                enemy = true;
                enemies.Add(col.GetComponent<Enemy>());
            }
            else if (col.CompareTag("EnemyBoss"))
            {
                boss = true;
                enemyBosses.Add(col.GetComponent<EnemyBoss>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemy = false;
                enemies.Remove(other.GetComponent<Enemy>());
            }
            else if (other.CompareTag("EnemyBoss"))
            {
                boss = false;
                enemyBosses.Remove(other.GetComponent<EnemyBoss>());
            }
        }
    }
}