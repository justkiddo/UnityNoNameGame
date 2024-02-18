using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace root
{
    public class DemoEnemyBandit : MonoBehaviour
    {
        [SerializeField] private EnemyHitCollider enemyHitColliderL1;
        [SerializeField] private EnemyHitCollider enemyHitColliderR1;

        private EnemyState _enemyState = EnemyState.Patrolling;
        private DemoEnemyInfo _demoEnemyInfo;
        private IDemoPlayer _demoPlayer;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Rigidbody2D _rb;
        private Vector3 _spawnPosition;
        private bool _firsPos = true;
        private bool _secondPos;
        private float _health;
        private float _speed;
        private float _damage;
        private bool _attacking = false;
        private bool _hit = false;
        
        private enum EnemyState
        {
            Patrolling,
            Following,
            Attacking
        }

        [Inject]
        private void Construct(DemoEnemyInfo enemyInfo, IDemoPlayer player)
        {
            _demoPlayer = player;
            _demoEnemyInfo = enemyInfo;
        }
        
        private void Awake()
        {
            _spawnPosition = transform.position;
            _health = _demoEnemyInfo.Health;
            _speed = _demoEnemyInfo.Speed;
            _enemyState = EnemyState.Patrolling;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!(_health > 0)) return;
            DistanceCheck();
            StateCheck();

        }

        private void DistanceCheck()
        {
            if (Vector2.Distance(transform.position, _demoPlayer.GetCurrentPosition()) < 
                _demoEnemyInfo.FollowDistance &&
                _demoPlayer.GetHealth() > 0 && !_attacking)
            {
                Debug.Log("follow");
                _enemyState = EnemyState.Following;
            }
            else if (Vector2.Distance(transform.position, _demoPlayer.GetCurrentPosition()) < 1f &&
                     _demoPlayer.GetHealth() > 0)
            {
                Debug.Log("attack");
                _enemyState = EnemyState.Attacking;
            }
            else if (Vector2.Distance(transform.position, _demoPlayer.GetCurrentPosition()) > 
                     _demoEnemyInfo.FollowDistance)
            {
                Debug.Log("patrol");
                _enemyState = EnemyState.Patrolling;
            }
        }

        private void StateCheck()
        {
            if (_enemyState == EnemyState.Patrolling)
            {
                Patrolling();
            }
            else if (_enemyState == EnemyState.Following && _demoPlayer.GetHealth() > 0)
            {
                Following();
            }
            else if (_enemyState == EnemyState.Attacking && _demoPlayer.GetHealth() > 0)
            {
                Attacking();
            }
        }

        private void Attacking()
        {
            _animator.SetInteger("AnimState", 1);
            if (Vector2.Distance(transform.position, _demoPlayer.GetCurrentPosition()) > 1f)
            {
                StartCoroutine(AttackAwait());
            }

            if (!_hit)
            {
                _animator.SetTrigger("Attack");
                StartCoroutine(AttackReset());
            }

        }

        private IEnumerator AttackReset()
        {
            yield return new WaitForSeconds(2);
            _animator.ResetTrigger("Attack");
            _hit = false;
        }

        private IEnumerator AttackAwait()
        {
            yield return new WaitForSeconds(0.5f);
            _attacking = false;
        }
        

        private void Following()
        {
            FLipCheck(_demoPlayer.GetCurrentPosition());
            _animator.SetInteger("AnimState", 2);
            transform.position = Vector2.MoveTowards(transform.position, _demoPlayer.GetCurrentPosition(),
                    Time.deltaTime * _speed);
            if (Vector2.Distance(transform.position, _demoPlayer.GetCurrentPosition()) < 1f)
            {
                _attacking = true;
            }
            else
            {
                _attacking = false;
            }
        }

        private void Patrolling()
        {
            
            Vector3 posOne = new Vector3(_spawnPosition.x - 5,_spawnPosition.y, _spawnPosition.z);
            Vector3 posTwo = new Vector3(_spawnPosition.x + 5,_spawnPosition.y, _spawnPosition.z);
            
            _animator.SetInteger("AnimState", 2);
            var position = transform.position;
                if (_firsPos)
                {
                    position = Vector2.MoveTowards(position, posOne, 
                        Time.deltaTime * _speed);
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
    }
}