using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace root
{
    public class Player : MonoBehaviour, IPlayer
    {
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
        private static readonly int AnimState = Animator.StringToHash("AnimState");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int Block = Animator.StringToHash("Block");
        private static readonly int IdleBlock = Animator.StringToHash("IdleBlock");
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Death = Animator.StringToHash("Death");
        
        [SerializeField] private float rollForce = 6.0f;
        [SerializeField] private GameObject endgameMenu;
        [SerializeField] private CheckpointSystem checkpointSystem;
        [SerializeField] private Button resetButton;
        [SerializeField] private GameObject checkpointParticlePrefab;
        [SerializeField] private GameObject saveText;
        [SerializeField] private PlayerHitCollider hitColliderL1;
        [SerializeField] private PlayerHitCollider hitColliderR1;
        [SerializeField] private PlayerSensor mGroundPlayerSensor;
        
        private PlayerInfo _playerInfo;
        private AudioSystem _audioSystem;
        private Enemy _enemy;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Rigidbody2D _body2d;
        private BossInfo _bossInfo;
        
        private readonly bool _isWallSliding = false;
        private bool _isGrounded;
        private bool _isRolling;
        private int _facingDirection = 1;
        private int _currentAttack;
        private float _timeSinceAttack;
        private float _delayToIdle;
        private readonly float _rollDuration = 8.0f / 14.0f;
        private float _rollCurrentTime;
        private bool _isBlocking;
        private bool _rollingAvailable = true;
        private bool _isDead;
        private bool _immunity;
        private string _attackTrigger;
        private float _mSpeed = 4.0f;
        private float _mJumpForce = 7.5f;
        private float _health;
        private float _damage;
        private GameplayInfo _gameplayInfo;
        private int _currentCheckpoint;
        

        [Inject]
        private void Construct(PlayerInfo playerInfo, AudioSystem audioSystem, BossInfo bossInfo, GameplayInfo gameplayInfo)
        {
            _gameplayInfo = gameplayInfo;
            _bossInfo = bossInfo;
            _audioSystem = audioSystem;
            _playerInfo = playerInfo;
        }
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _body2d = GetComponent<Rigidbody2D>();
            _health = _playerInfo.Health;
            _mSpeed = _playerInfo.Speed;
            _mJumpForce = _playerInfo.JumpHeight;
            _damage = _playerInfo.Damage;
            resetButton.onClick.AddListener(ResetPrefs);
        }

        private void ResetPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        private void Awake()
        {
            transform.position = checkpointSystem._checkpointsList[PlayerPrefs.GetInt(BaseIds.SavedCheckpointKey)].transform.position;
        }

        private void AddListeners()
        {
            _gameplayInfo.SavedCheckpoint.Subscribe(_ => OnCheckpointChange());
        }

        private void OnCheckpointChange()
        {
            _gameplayInfo.SavedCheckpoint.Value = _currentCheckpoint;
        }


        void Update()
        {
            AddListeners();
            if (!_isDead)
            {
                _timeSinceAttack += Time.deltaTime;
                float inputX = Input.GetAxis("Horizontal");
                
                if (_isRolling)
                    _rollCurrentTime += Time.deltaTime;

                if (_rollCurrentTime > _rollDuration)
                    _isRolling = false;

                if (!_isGrounded && mGroundPlayerSensor.State())
                {
                    _isGrounded = true;
                    _animator.SetBool(Grounded, _isGrounded);
                }

                if (_isGrounded && !mGroundPlayerSensor.State())
                {
                    _isGrounded = false;
                    _animator.SetBool(Grounded, _isGrounded);
                }

                if (inputX > 0)
                {
                    _spriteRenderer.flipX = false;
                    _facingDirection = 1;
                }

                else if (inputX < 0)
                {
                    _spriteRenderer.flipX = true;
                    _facingDirection = -1;
                }

                if (!_isRolling && !_isBlocking)
                {
                    _body2d.velocity = new Vector2(inputX * _mSpeed, _body2d.velocity.y);
                }

                _animator.SetFloat(AirSpeedY, _body2d.velocity.y);
                
                PlayerRoll();
                PlayerJump();
                PlayerBlock();
                PlayerRun(inputX);
                Attack();
            }
        }

        private void Attack()
        {
            if (!_isDead && Input.GetKeyDown(KeyCode.Z))
            {
                if (_timeSinceAttack > 0.25f && !_isRolling)
                {
                    var hit = false;
                    _currentAttack++;
                    if (_currentAttack > 3)
                        _currentAttack = 1;

                    if (_timeSinceAttack > 1.0f)
                        _currentAttack = 1;

                    _attackTrigger = "Attack";
                    _animator.SetTrigger(_attackTrigger + _currentAttack);

                    _timeSinceAttack = 0.0f;

                    hit = PlayerAttack(hit);

                    if (hit)
                    {
                        _audioSystem.Attack();
                    }
                    else
                    {
                        _audioSystem.MissedAttack();
                    }
                }
            }
        }

        private void PlayerRun(float inputX)
        {
            if (!_isDead)
            {
                if (Mathf.Abs(inputX) > Mathf.Epsilon)
                {
                    _delayToIdle = 0.05f;
                    _animator.SetInteger(AnimState, 1);
                }

                else
                {
                    _delayToIdle -= Time.deltaTime;
                    if (_delayToIdle < 0)
                        _animator.SetInteger(AnimState, 0);
                }
            }
        }

        private void PlayerJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_isRolling)
            {
                _animator.SetTrigger(Jump);
                _isGrounded = false;
                _animator.SetBool(Grounded, _isGrounded);
                _body2d.velocity = new Vector2(_body2d.velocity.x, _mJumpForce);
                mGroundPlayerSensor.Disable(0.2f);
            }
        }

        private void PlayerRoll()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_isRolling && !_isWallSliding && _rollingAvailable)
            {
                _isRolling = true;
                _animator.SetTrigger(Roll);
                _body2d.velocity = new Vector2(_facingDirection * rollForce, _body2d.velocity.y);
            }
        }

        private void PlayerBlock()
        {
            if (Input.GetKeyDown(KeyCode.C) && !_isRolling && _isGrounded && !_isDead)
            {
                _isBlocking = true;


                if (hitColliderL1.enemies.Find(x => x.CompareTag("Enemy")) && _spriteRenderer.flipX)
                {
                    _immunity = true;
                }
                else if (hitColliderR1.enemies.Find(x => x.CompareTag("Enemy")) &&
                         _spriteRenderer.flipX == false)
                {
                    _immunity = true;
                }
                else
                {
                    _immunity = false;
                }

                _rollingAvailable = false;
                _animator.SetTrigger(Block);
                _animator.SetBool(IdleBlock, true);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                _isBlocking = false;
                _immunity = false;
                _rollingAvailable = true;
                _animator.SetBool(IdleBlock, false);
            }
        }

        private bool PlayerAttack(bool hit)
        {
            if ( _spriteRenderer.flipX)
            {

                    foreach (var enemy in hitColliderL1.enemies)
                    {
                        if (hitColliderL1.enemy)
                        {
                            enemy.TakeDamage(_damage);
                            hit = true;
                        }
                    }
                    foreach (var boss in hitColliderL1.enemyBosses)
                    {
                        if (hitColliderL1.boss)
                        {
                            boss.TakeDamage(_damage);
                            hit = true;
                        }
                    }
                
            }
            else if ( _spriteRenderer.flipX == false)
            {
                foreach (var enemy in hitColliderR1.enemies)
                {
                    if (hitColliderR1.enemy)
                    {
                        enemy.TakeDamage(_damage);
                        hit = true;
                    }
                }
                
                foreach (var boss in hitColliderR1.enemyBosses)
                {
                    if (hitColliderR1.boss)
                    {
                        boss.TakeDamage(_damage);
                        hit = true;
                    }
                }
            }
            return hit;
        }
        
        public Vector3 GetCurrentPosition() => transform.position;
        
        public void TakeDamage(float damage)
        {
            if (!_isDead && !_immunity) 
            {
                _health -= damage;
                _animator.SetTrigger(Hurt);
                
                _body2d.velocity = transform.up * 2; 
                
                if (_health <= 0)
                {
                    _isDead = true;
                    _animator.ResetTrigger(Hurt);
                    _animator.SetTrigger(Death);
                    endgameMenu.SetActive(true);
                }
            }
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Checkpoint"))
            {
                _currentCheckpoint = Int32.Parse(col.gameObject.name);
                if (col.gameObject.name != _gameplayInfo.SavedCheckpoint.Value.ToString())
                {
                    Instantiate(checkpointParticlePrefab, transform.position, Quaternion.identity);
                    saveText.SetActive(true);
                    StartCoroutine(SaveTextAwait());
                }
            }
        }

        private IEnumerator SaveTextAwait()
        {
            yield return new WaitForSeconds(2);
            saveText.SetActive(false);
        }


        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Fireball"))
            {
                TakeDamage(_bossInfo.Damage);
            }
        }

        public float GetHealth() => _health;
        public bool IsDead() => _isDead;
    }
}