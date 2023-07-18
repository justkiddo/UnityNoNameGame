using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace root
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private bool follow;
        [SerializeField]private EnemyHitCollider enemyHitColliderL1;
        [SerializeField]private EnemyHitCollider enemyHitColliderR1;
        private float _health;
        private float _speed;
        private float _damage;
        private EnemyInfo _enemyInfo;
        private IPlayer _player;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private float _tempHealth;
        private bool _damageTaken;
        private Collider2D _enemyCollider2D;
        private Rigidbody2D rb;
        private bool _attacking;
        
        private Vector3 _spawnPos;
        private bool _patrolling;
        
        private bool _firsPos = true;
        private bool _secondPos;
        
        
        [Inject]
        private void Construct(EnemyInfo enemyInfo, IPlayer player)
        {
            _player = player;
            _enemyInfo = enemyInfo;
        }
        
        private void Awake()
        {
            _spawnPos = transform.position;
            _health = _enemyInfo.Health;
            _speed = _enemyInfo.Speed;
            _damage = _enemyInfo.Damage;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            _tempHealth = _health;
            _patrolling = true;
        }



        private void Update()
        {
            DamageCheck();
            DeathCheck();
            AttackPlayer();
            DistanceCheck();
        }

        private void DistanceCheck()
        {
            if (Vector2.Distance(transform.position, _player.GetCurrentPosition()) 
                < _enemyInfo.FollowDistance && _player.GetHealth() > 0)
            {
                follow = true;
                _patrolling = false;
                FollowPlayer();
            }
            else
            {
                follow = false;
                Patrolling();
            }
        }

        private void Patrolling()
        {
            Vector3 posOne = new Vector3(_spawnPos.x - 5,_spawnPos.y, _spawnPos.z);
            Vector3 posTwo = new Vector3(_spawnPos.x + 5,_spawnPos.y, _spawnPos.z);
            
            _animator.SetInteger("AnimState", 2);
            
            if (Vector2.Distance(transform.position, _spawnPos) > 1 && !_patrolling && !follow)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, _spawnPos, Time.deltaTime * _speed);
                FLipCheck(_spawnPos);
            }
            else
            {
                _patrolling = true;
                var position = transform.position;
                if (_firsPos)
                {
                    position = Vector2.MoveTowards(position, posOne, Time.deltaTime * _speed);
                    transform.position = position;
                    FLipCheck(posOne);
                    if (Vector2.Distance(position, posOne) < 0.4f)
                    {
                        _firsPos = false;
                        _secondPos = true;
                    }
                }
                else if (_secondPos)
                {
                    transform.position = Vector2.MoveTowards(transform.position, posTwo, 
                        Time.deltaTime * _speed);
                    FLipCheck(posTwo);
                    if (Vector2.Distance(position, posTwo) < 0.4f)
                    {
                        _firsPos = true;
                        _secondPos = false;
                    }
                }
            }

        }



        private void AttackPlayer()
        {
            if (enemyHitColliderL1.playerNear | enemyHitColliderR1.playerNear && !_attacking && _player.GetHealth() > 0)
            {
                _attacking = true;
                _animator.SetTrigger("Attack");
                
                StartCoroutine(ResetTriggerAttack());
            }
        }

        private IEnumerator ResetTriggerAttack()
        {
            if (_damageTaken)
            {
                _damageTaken = false;
                _attacking = false;
                yield break;
                
            }
            yield return new WaitForSeconds(0.6f);
            
            _attacking = false;
            if (enemyHitColliderL1.playerNear | enemyHitColliderR1.playerNear && !_attacking && _player.GetHealth() > 0)
            {
                _player.TakeDamage(_damage);
            }
        }

        private void DeathCheck()
        {
            if (_health <= 0)
            {
                follow = false;
                _patrolling = false;
                _attacking = true;
                rb.simulated = false;
                _animator.SetTrigger("Death");
                StopCoroutine(ResetTriggerAttack());
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
                _damageTaken = true;
                _animator.SetTrigger("Hurt");
                _tempHealth = _health;
                
                Vector3 dir = (_player.GetCurrentPosition() - transform.position);//bounce ----------------------
                rb.velocity = -dir * 2;//bounce ----------------------
                
                
                StartCoroutine(ResetTriggerHurt());
            }
        }
        

        
        private IEnumerator ResetTriggerHurt()
        {
            yield return new WaitForSeconds(1);
            _animator.ResetTrigger("Hurt");
            _damageTaken = false;
        }
        

        private void FLipCheck(Vector3 direction)
        {
            if (transform.position.x > direction.x)
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
            if (follow 
                && _player.GetHealth() >= 0 && _health > 0)
            {
                FLipCheck(_player.GetCurrentPosition());
                _patrolling = false;
                _animator.SetInteger("AnimState", 2);
                transform.position = Vector2.MoveTowards(transform.position, _player.GetCurrentPosition(),
                    Time.deltaTime * _speed);
            }
            else
            {
                follow = false;
                _animator.SetInteger("AnimState", 1);
            }
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
        }
    }
}