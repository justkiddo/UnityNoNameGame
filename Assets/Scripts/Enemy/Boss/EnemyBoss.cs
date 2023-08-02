using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace root
{
    public class EnemyBoss : MonoBehaviour, IEnemy
    {
        [SerializeField] private List<Transform> points;
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform shootPointLeft;
        [SerializeField] private Transform shootPointRight;
        [SerializeField] private SpriteRenderer deadBoss;
    
        private Collider2D _collider2D;
        private Animator _animator;
        private BossInfo _bossInfo;
        private SpriteRenderer _spriteRenderer;    
        private Vector3 _playerPos;
        private bool _ftp = true;
        private float _startTime;
        private IPlayer _player;
    
        public bool startFight = false;
        private float _health;
        private int _damage;
        private float _fireballSpeed;
        private float _tempHealth;
        private bool _damageTaken;
        public bool isDead;
    
        [Inject]
        private void Construct(IPlayer player, BossInfo bossInfo)
        {
            _bossInfo = bossInfo;
            _player = player;
        }

        private void Awake()
        {
            _startTime = Time.time;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _health = _bossInfo.health;
            _damage = _bossInfo.damage;
            _fireballSpeed = _bossInfo.fireballSpeed;
            _tempHealth = _health;
        }

        private void Update()
        {
            if (startFight)
            {
                DamageCheck();
                DeathCheck();
                if (!isDead)
                {
                    Teleport();
                    FlipCheck();
                }
            }
        }

        private void DamageCheck()
        {
            if (Math.Abs(_health - _tempHealth) > 0)
            {
                _damageTaken = true;
                _animator.SetTrigger("Hurt");
                _tempHealth = _health;
                StartCoroutine(ResetTriggerHurt());
            }
        }
    
        private IEnumerator ResetTriggerHurt()
        {
            yield return new WaitForSeconds(1);
            _animator.ResetTrigger("Hurt");
            _damageTaken = false;
        }
    
        private void DeathCheck()
        {
            if (_health <= 0)
            {
                isDead = true;
                _animator.SetTrigger("Death");
                StopCoroutine(ResetTriggerAttack());
                StartCoroutine(DeathAwait());
            }
        }

        private IEnumerator DeathAwait()
        {
            yield return new WaitForSeconds(2);
            if (_spriteRenderer.flipX)
            {
                var boss = Instantiate(deadBoss, transform.position, Quaternion.identity);
                boss.flipX = true;
            }
            else
            {
                var boss = Instantiate(deadBoss, transform.position, Quaternion.identity);
                boss.flipX = false;
            }
            Destroy(this.gameObject);
        }

        private IEnumerator ResetTriggerAttack()
        {
            if (_damageTaken)
            {
                _damageTaken = false;
                yield break;
                
            }
            yield return new WaitForSeconds(0.6f);
        }

        private void FlipCheck()
        {
            if (_player.GetCurrentPosition().x > transform.position.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }
        }

        private void Teleport()
        {
            var tempTime = Time.time;
            if (_ftp)
            {
                Random r = new Random();
                var n = r.Next(0, points.Capacity);
                transform.position = points[n].position;
                Shoot();
                _ftp = false;
            }
            else
            {
                if (tempTime - _startTime > 5)
                {
                    _startTime = Time.time;
                    _ftp = true;
                }
            }
        }
    
        private void Shoot()
        {
            var direction = _player.GetCurrentPosition() - transform.position;
            if (_spriteRenderer.flipX)
            {
                var bullet = Instantiate(fireballPrefab, shootPointLeft.position, Quaternion.identity);
                var rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector3(direction.x,direction.y, direction.z) * _fireballSpeed;
            }
            else if(!_spriteRenderer.flipX)
            {
                var bullet = Instantiate(fireballPrefab, shootPointRight.position, Quaternion.identity);
                var rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector3(direction.x,direction.y, direction.z) * _fireballSpeed;
            }
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
        }
    }
}