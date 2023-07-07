using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Zenject;
using Object = UnityEngine.Object;

namespace root
{
    public class Player : MonoBehaviour, IPlayer
    {

        float _mSpeed = 4.0f;
        float _mJumpForce = 7.5f;
        private float _health;
        [SerializeField] float m_rollForce = 6.0f;
        private PlayerHitCollider _hitColliderL1;
        private PlayerHitCollider _hitColliderR1;
        [SerializeField] private PhysicsMaterial2D playerMat;
        
        private Animator m_animator;
        private Rigidbody2D m_body2d;
        private PlayerSensor _mGroundPlayerSensor;
        // private PlayerSensor _mWallPlayerSensorR1;
        // private PlayerSensor _mWallPlayerSensorR2;
        // private PlayerSensor _mWallPlayerSensorL1;
        // private PlayerSensor _mWallPlayerSensorL2;
        private bool m_isWallSliding = false;
        private bool _isGrounded = false;
        private bool _isRolling = false;
        private int m_facingDirection = 1;
        private int m_currentAttack = 0;
        private float m_timeSinceAttack = 0.0f;
        private float m_delayToIdle = 0.0f;
        private float m_rollDuration = 8.0f / 14.0f;
        private float m_rollCurrentTime;
        public bool _isBlocking = false;
        private bool _rollingAvailiable = true;
        private PlayerInfo _playerInfo;
        
        
        //[SerializeField] private Collider2D enemyCollider;
        private Enemy _enemy;
        private bool isDead;
        [SerializeField] private GameObject endgameMenu;


        [Inject]
        private void Construct(PlayerInfo playerInfo, Enemy enemy)
        {
            _enemy = enemy;
            _playerInfo = playerInfo;
        }
        
        // Use this for initialization
        void Start()
        {
            m_animator = GetComponent<Animator>();
            m_body2d = GetComponent<Rigidbody2D>();
            _mGroundPlayerSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
            // _mWallPlayerSensorR1 = transform.Find("WallSensor_R1").GetComponent<PlayerSensor>();
            // _mWallPlayerSensorR2 = transform.Find("WallSensor_R2").GetComponent<PlayerSensor>();
            // _mWallPlayerSensorL1 = transform.Find("WallSensor_L1").GetComponent<PlayerSensor>();
            // _mWallPlayerSensorL2 = transform.Find("WallSensor_L2").GetComponent<PlayerSensor>();
            
            _hitColliderL1 = transform.Find("HitCollider L1").GetComponent<PlayerHitCollider>();
            _hitColliderR1 = transform.Find("HitCollider R1").GetComponent<PlayerHitCollider>();
            _health = _playerInfo.Health;
            _mSpeed = _playerInfo.Speed;
            _mJumpForce = _playerInfo.JumpHeight;
        }




        // Update is called once per frame
        void Update()
        {
            if (!isDead)
            {
                // Increase timer that controls attack combo
                m_timeSinceAttack += Time.deltaTime;

                // Increase timer that checks roll duration
                if (_isRolling)
                    m_rollCurrentTime += Time.deltaTime;

                // Disable rolling if timer extends duration
                if (m_rollCurrentTime > m_rollDuration)
                    _isRolling = false;

                //Check if character just landed on the ground
                if (!_isGrounded && _mGroundPlayerSensor.State())
                {
                    _isGrounded = true;
                    m_animator.SetBool("Grounded", _isGrounded);
                }

                //Check if character just started falling
                if (_isGrounded && !_mGroundPlayerSensor.State())
                {
                    _isGrounded = false;
                    m_animator.SetBool("Grounded", _isGrounded);
                }
            
            
                // -- Handle input and movement --
                float inputX = Input.GetAxis("Horizontal");

                // Swap direction of sprite depending on walk direction
                if (inputX > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    m_facingDirection = 1;
                }

                else if (inputX < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    m_facingDirection = -1;
                }

                // Move
                if (!_isRolling && !_isBlocking)
                {
                    m_body2d.velocity = new Vector2(inputX * _mSpeed, m_body2d.velocity.y);
                }

                //Set AirSpeed in animator
                m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);



                //Attack
                if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !_isRolling)
                {
                    m_currentAttack++;

                    // Loop back to one after third attack
                    if (m_currentAttack > 3)
                        m_currentAttack = 1;

                    // Reset Attack combo if time since last attack is too large
                    if (m_timeSinceAttack > 1.0f)
                        m_currentAttack = 1;

                    // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                    m_animator.SetTrigger("Attack" + m_currentAttack);

                    // Reset timer
                    m_timeSinceAttack = 0.0f;

                    // enemy hit check
                    if (Input.GetMouseButtonDown(0) && GetComponent<SpriteRenderer>().flipX)
                    {
                        foreach (var enemy in _hitColliderL1.enemies)
                        {
                            enemy._health -= 5;
                        }
                    }
                
                    if (Input.GetMouseButtonDown(0) && GetComponent<SpriteRenderer>().flipX == false)
                    {
                        foreach (var enemy in _hitColliderR1.enemies)
                        {
                            enemy._health -= 5;
                        }
                    }
                }
            
            
            

                // Block
                else if (Input.GetMouseButtonDown(1) && !_isRolling && _isGrounded)
                {
                    _isBlocking = true;
                    _rollingAvailiable = false;
                    m_animator.SetTrigger("Block");
                    m_animator.SetBool("IdleBlock", true);
                }

                else if (Input.GetMouseButtonUp(1))
                {
                    _isBlocking = false;
                    _rollingAvailiable = true;
                    m_animator.SetBool("IdleBlock", false);
                }
                // Roll
                else if (Input.GetKeyDown("left shift") && !_isRolling && !m_isWallSliding && _rollingAvailiable)
                {
                    _isRolling = true;
                    m_animator.SetTrigger("Roll");
                    m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
                }


                //Jump
                else if (Input.GetKeyDown("space") && _isGrounded && !_isRolling)
                {
                    m_animator.SetTrigger("Jump");
                    _isGrounded = false;
                    m_animator.SetBool("Grounded", _isGrounded);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, _mJumpForce);
                    _mGroundPlayerSensor.Disable(0.2f);
                }

                //Run
                else if (Mathf.Abs(inputX) > Mathf.Epsilon)
                {
                    // Reset timer
                    m_delayToIdle = 0.05f;
                    m_animator.SetInteger("AnimState", 1);
                }

                //Idle
                else
                {
                    // Prevents flickering transitions to idle
                    m_delayToIdle -= Time.deltaTime;
                    if (m_delayToIdle < 0)
                        m_animator.SetInteger("AnimState", 0);
                }
            
                //   FrictionSetup();
            }
        }



        private void FrictionSetup()
        {
            if (_isGrounded | _isBlocking)
            {
                playerMat.friction = 55;
            }
            else
            {
                playerMat.friction = 0;
            }
        }


        public Vector3 GetCurrentPosition() => transform.position;
        public void TakeDamage(float damage)
        {
            if (!isDead)
            {
                _health -= damage;
                m_animator.SetTrigger("Hurt");
                if (_health <= 0)
                {
                    isDead = true;
                    m_animator.ResetTrigger("Hurt");
                    m_animator.SetTrigger("Death");
                    endgameMenu.SetActive(true);
                }
            }
        }

        public float GetHealth() => _health;
    }
}