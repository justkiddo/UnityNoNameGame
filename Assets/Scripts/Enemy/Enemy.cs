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
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        
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
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }



        private void Update()
        {
            FollowPlayer();
            FLipCheck();
        }

        private void FLipCheck()
        {
            
            if (transform.position.x > _player.GetCurrentPosition().x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }
        }

        private void FollowPlayer()
        {
            _animator.SetInteger("AnimState", 2);
            transform.position = Vector3.MoveTowards(transform.position, _player.GetCurrentPosition(),
                Time.deltaTime * _speed);
        }
    
    }
}