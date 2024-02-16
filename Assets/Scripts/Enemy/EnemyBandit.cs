using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace root
{
    public class EnemyBandit : MonoBehaviour
    {
        [SerializeField]private EnemyHitCollider enemyHitColliderL1;
        [SerializeField]private EnemyHitCollider enemyHitColliderR1;
        
        private EnemyState _enemyState;
        private EnemyInfo _enemyInfo;
        private IPlayer _player;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Rigidbody2D _rb;
        private Vector3 _spawnPos;
        private bool _firsPos = true;
        private bool _secondPos;
        private float _health;
        private float _speed;
        private float _damage;
        
        private enum EnemyState
        {
            Patrolling,
            Following,
            Attacking,
            Returning
        }

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
            
        }

        private void StateCheck()
        {
            if (_enemyState == EnemyState.Patrolling)
            {
                Patrolling();
            }
            else if (_enemyState == EnemyState.Following && _player.GetHealth() > 0)
            {
                Following();
            }
            else if (_enemyState == EnemyState.Attacking && _player.GetHealth() > 0)
            {
                
            }
            else if(_enemyState == EnemyState.Returning)
            {
                
            }
        }

        private void Following()
        {
            FLipCheck(_player.GetCurrentPosition());
                _animator.SetInteger("AnimState", 2);
                
                transform.position = Vector2.MoveTowards(transform.position, _player.GetCurrentPosition(),
                    Time.deltaTime * _speed);
        }

        private void Patrolling()
        {
            Vector3 posOne = new Vector3(_spawnPos.x - 5,_spawnPos.y, _spawnPos.z);
            Vector3 posTwo = new Vector3(_spawnPos.x + 5,_spawnPos.y, _spawnPos.z);
            
            _animator.SetInteger("AnimState", 2);
            
            if (Vector2.Distance(transform.position, _spawnPos) > 1)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, _spawnPos, Time.deltaTime * _speed);
                FLipCheck(_spawnPos);
            }
            else
            {
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