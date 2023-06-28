using UnityEngine;
using Zenject;

namespace root
{
    public class Enemy : MonoBehaviour
    {
        private float _health;
        private float _speed;
        private float _damage;
        private EnemyInfo _enemyInfo;
        private IPlayer _player;

        [Inject]
        private void Construct(EnemyInfo enemyInfo, IPlayer player)
        {
            _player = player;
            _enemyInfo = enemyInfo;
        }
        
        private void Awake()
        {
            _health = _enemyInfo.Health;
            _speed = _enemyInfo.Speed;
            _damage = _enemyInfo.Damage;
        }



        private void Update()
        {
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.GetCurrentPosition(),
                Time.deltaTime * _speed);
        }
    
    }
}