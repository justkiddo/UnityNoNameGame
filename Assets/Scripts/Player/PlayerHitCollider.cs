using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace root
{
    public class PlayerHitCollider : MonoBehaviour
    {
        private PlayerInfo _playerInfo;
        public List<Enemy> enemies;

        [Inject]
        private void Construct( PlayerInfo playerInfo)
        {
            _playerInfo = playerInfo;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                enemies.Add(col.GetComponent<Enemy>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemies.Remove(other.GetComponent<Enemy>());
            }
        }
    }
}