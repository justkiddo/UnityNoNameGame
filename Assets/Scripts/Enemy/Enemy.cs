using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace root
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private bool follow;
        public float _health;
        private float _speed;
        private float _damage;
        private EnemyInfo _enemyInfo;
        private IPlayer _player;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private float _tempHealth;
        private bool _damageTaken;
        private Collider2D _enemyCollider2D;
        
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
            _tempHealth = _health;
        }



        private void Update()
        {
            FollowPlayer();
            FLipCheck();
            DamageCheck();
            DeathCheck();
        }

        private void DeathCheck()
        {
            if (_health <= 0)
            {
                _animator.SetTrigger("Death");
                StartCoroutine(DestroyEnemy());
            }
        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(1);
            Destroy(this.gameObject);
        }

        private void DamageCheck()
        {
            if (Math.Abs(_health - _tempHealth) > 0)
            {
                _animator.SetTrigger("Hurt");
                _tempHealth = _health;
                StartCoroutine(ResetTrigger());
            }
            
        }

        private IEnumerator ResetTrigger()
        {
            yield return new WaitForSeconds(1);
            _animator.ResetTrigger("Hurt");
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
            if (follow)
            {
                _animator.SetInteger("AnimState", 2);
                transform.position = Vector3.MoveTowards(transform.position, _player.GetCurrentPosition(),
                    Time.deltaTime * _speed);
            }
        }

        public Collider2D GetEnemyCollider() => _enemyCollider2D;
    }
}