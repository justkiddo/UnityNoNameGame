using UnityEngine;
using Zenject;

namespace root
{
    public class Player : MonoBehaviour, IPlayer
    {
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
        
        
        [SerializeField] float rollForce = 6.0f;
        [SerializeField] private PhysicsMaterial2D playerMat;
        [SerializeField] private GameObject endgameMenu;
        
        private PlayerInfo _playerInfo;
        private AudioSystem _audioSystem;
        private Enemy _enemy;
        private SpriteRenderer _spriteRenderer;
        private float _mSpeed = 4.0f;
        private float _mJumpForce = 7.5f;
        private float _health;
        private float _damage;
        
        private PlayerHitCollider _hitColliderL1;
        private PlayerHitCollider _hitColliderR1;
       
        
        private Animator _animator;
        private Rigidbody2D _body2d;
        private PlayerSensor _mGroundPlayerSensor;
        
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
        
        // private PlayerSensor _mWallPlayerSensorR1;
        // private PlayerSensor _mWallPlayerSensorR2;
        // private PlayerSensor _mWallPlayerSensorL1;
        // private PlayerSensor _mWallPlayerSensorL2;




        [Inject]
        private void Construct(PlayerInfo playerInfo, Enemy enemy, AudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
            _enemy = enemy;
            _playerInfo = playerInfo;
        }
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _body2d = GetComponent<Rigidbody2D>();
            _mGroundPlayerSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
            _hitColliderL1 = transform.Find("HitCollider L1").GetComponent<PlayerHitCollider>();
            _hitColliderR1 = transform.Find("HitCollider R1").GetComponent<PlayerHitCollider>();
            _health = _playerInfo.Health;
            _mSpeed = _playerInfo.Speed;
            _mJumpForce = _playerInfo.JumpHeight;
            _damage = _playerInfo.Damage;
            
            // _mWallPlayerSensorR1 = transform.Find("WallSensor_R1").GetComponent<PlayerSensor>();
            // _mWallPlayerSensorR2 = transform.Find("WallSensor_R2").GetComponent<PlayerSensor>();
            // _mWallPlayerSensorL1 = transform.Find("WallSensor_L1").GetComponent<PlayerSensor>();
            // _mWallPlayerSensorL2 = transform.Find("WallSensor_L2").GetComponent<PlayerSensor>();
        }




        void Update()
        {
            if (!_isDead)
            {
                _timeSinceAttack += Time.deltaTime;

                if (_isRolling)
                    _rollCurrentTime += Time.deltaTime;

                if (_rollCurrentTime > _rollDuration)
                    _isRolling = false;

                if (!_isGrounded && _mGroundPlayerSensor.State())
                {
                    _isGrounded = true;
                    _animator.SetBool(Grounded, _isGrounded);
                }

                if (_isGrounded && !_mGroundPlayerSensor.State())
                {
                    _isGrounded = false;
                    _animator.SetBool(Grounded, _isGrounded);
                }
                
                float inputX = Input.GetAxis("Horizontal");
                
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



                if (Input.GetMouseButtonDown(0) && _timeSinceAttack > 0.25f && !_isRolling)
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

            
            
            

                PlayerBlock();
                PlayerRoll();
                PlayerJump();
                PlayerRun(inputX);
            }
        }

        private void PlayerRun(float inputX)
        {
            if (Mathf.Abs(inputX) > Mathf.Epsilon)
            {
                _delayToIdle = 0.05f;
                _animator.SetInteger("AnimState", 1);
            }

            else
            {
                _delayToIdle -= Time.deltaTime;
                if (_delayToIdle < 0)
                    _animator.SetInteger("AnimState", 0);
            }
        }

        private void PlayerJump()
        {
            if (Input.GetKeyDown("space") && _isGrounded && !_isRolling)
            {
                _animator.SetTrigger("Jump");
                _isGrounded = false;
                _animator.SetBool("Grounded", _isGrounded);
                _body2d.velocity = new Vector2(_body2d.velocity.x, _mJumpForce);
                _mGroundPlayerSensor.Disable(0.2f);
            }
        }

        private void PlayerRoll()
        {
            if (Input.GetKeyDown("left shift") && !_isRolling && !_isWallSliding && _rollingAvailable)
            {
                _isRolling = true;
                _animator.SetTrigger("Roll");
                _body2d.velocity = new Vector2(_facingDirection * rollForce, _body2d.velocity.y);
            }
        }

        private void PlayerBlock()
        {
            if (Input.GetMouseButtonDown(1) && !_isRolling && _isGrounded)
            {
                _isBlocking = true;


                if (_hitColliderL1.enemies.Find(x => x.CompareTag("Enemy")) && _spriteRenderer.flipX)
                {
                    _immunity = true;
                }
                else if (_hitColliderR1.enemies.Find(x => x.CompareTag("Enemy")) &&
                         _spriteRenderer.flipX == false)
                {
                    _immunity = true;
                }
                else
                {
                    _immunity = false;
                }

                _rollingAvailable = false;
                _animator.SetTrigger("Block");
                _animator.SetBool("IdleBlock", true);
            }

            else if (Input.GetMouseButtonUp(1))
            {
                _isBlocking = false;
                _immunity = false;
                _rollingAvailable = true;
                _animator.SetBool("IdleBlock", false);
            }
        }

        private bool PlayerAttack(bool hit)
        {
            if (Input.GetMouseButtonDown(0) && _spriteRenderer.flipX)
            {
                foreach (var enemy in _hitColliderL1.enemies)
                {
                    enemy.TakeDamage(_damage);
                    hit = true;
                }
            }
            else if (Input.GetMouseButtonDown(0) && _spriteRenderer.flipX == false)
            {
                foreach (var enemy in _hitColliderR1.enemies)
                {
                    enemy.TakeDamage(_damage);
                    hit = true;
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
                _animator.SetTrigger("Hurt");
                
                _body2d.velocity = transform.up * 2; 
                
                if (_health <= 0)
                {
                    _isDead = true;
                    _animator.ResetTrigger("Hurt");
                    _animator.SetTrigger("Death");
                    endgameMenu.SetActive(true);
                }
            }
        }

        public float GetHealth() => _health;
    }
}