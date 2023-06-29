using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Zenject;

namespace root
{
    public class Player : MonoBehaviour, IPlayer
    {

        float _mSpeed = 4.0f;
        float _mJumpForce = 7.5f;
        private float _health;
        [SerializeField] float m_rollForce = 6.0f;
        [SerializeField] bool m_noBlood = false;
        [SerializeField] GameObject m_slideDust;
        private PlayerHitCollider hitColliderL1;
        private PlayerHitCollider hitColliderR1;
        
        private Animator m_animator;
        private Rigidbody2D m_body2d;
        private PlayerSensor _mGroundPlayerSensor;
        private PlayerSensor _mWallPlayerSensorR1;
        private PlayerSensor _mWallPlayerSensorR2;
        private PlayerSensor _mWallPlayerSensorL1;
        private PlayerSensor _mWallPlayerSensorL2;
        private bool m_isWallSliding = false;
        private bool m_grounded = false;
        private bool m_rolling = false;
        private int m_facingDirection = 1;
        private int m_currentAttack = 0;
        private float m_timeSinceAttack = 0.0f;
        private float m_delayToIdle = 0.0f;
        private float m_rollDuration = 8.0f / 14.0f;
        private float m_rollCurrentTime;
        private bool _isBlocking = false;
        private bool _rollingAvailiable = true;
        private PlayerInfo _playerInfo;
        
        
        //[SerializeField] private Collider2D enemyCollider;
        private Enemy _enemy;


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
            _mWallPlayerSensorR1 = transform.Find("WallSensor_R1").GetComponent<PlayerSensor>();
            _mWallPlayerSensorR2 = transform.Find("WallSensor_R2").GetComponent<PlayerSensor>();
            _mWallPlayerSensorL1 = transform.Find("WallSensor_L1").GetComponent<PlayerSensor>();
            _mWallPlayerSensorL2 = transform.Find("WallSensor_L2").GetComponent<PlayerSensor>();
            
            hitColliderL1 = transform.Find("HitCollider L1").GetComponent<PlayerHitCollider>();
            hitColliderR1 = transform.Find("HitCollider R1").GetComponent<PlayerHitCollider>();
            _health = _playerInfo.Health;
            _mSpeed = _playerInfo.Speed;
            _mJumpForce = _playerInfo.JumpHeight;
        }




        // Update is called once per frame
        void Update()
        {
            // Increase timer that controls attack combo
            m_timeSinceAttack += Time.deltaTime;

            // Increase timer that checks roll duration
            if (m_rolling)
                m_rollCurrentTime += Time.deltaTime;

            // Disable rolling if timer extends duration
            if (m_rollCurrentTime > m_rollDuration)
                m_rolling = false;

            //Check if character just landed on the ground
            if (!m_grounded && _mGroundPlayerSensor.State())
            {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }

            //Check if character just started falling
            if (m_grounded && !_mGroundPlayerSensor.State())
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
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
            if (!m_rolling && !_isBlocking)
            {
                m_body2d.velocity = new Vector2(inputX * _mSpeed, m_body2d.velocity.y);
            }

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

            // -- Handle Animations --
            //Wall Slide
            m_isWallSliding = (_mWallPlayerSensorR1.State() && _mWallPlayerSensorR2.State()) ||
                              (_mWallPlayerSensorL1.State() && _mWallPlayerSensorL2.State());
            m_animator.SetBool("WallSlide", m_isWallSliding);

            //Death
            if (Input.GetKeyDown("e") && !m_rolling)
            {
                m_animator.SetBool("noBlood", m_noBlood);
                m_animator.SetTrigger("Death");
            }

            //Hurt
            else if (Input.GetKeyDown("q") && !m_rolling)
                m_animator.SetTrigger("Hurt");

            //Attack
            else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
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
                    foreach (var enemy in hitColliderL1.enemies)
                    {
                        enemy._health -= 5;
                    }
                }
                
                if (Input.GetMouseButtonDown(0) && GetComponent<SpriteRenderer>().flipX == false)
                {
                    foreach (var enemy in hitColliderR1.enemies)
                    {
                        enemy._health -= 5;
                    }
                }
            }
            
            
            

            // Block
            else if (Input.GetMouseButtonDown(1) && !m_rolling)
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
            else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding && _rollingAvailiable)
            {
                m_rolling = true;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            }


            //Jump
            else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
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
        }
        


        // Animation Events
        // Called in slide animation.
        void AE_SlideDust()
        {
            Vector3 spawnPosition;

            if (m_facingDirection == 1)
                spawnPosition = _mWallPlayerSensorR2.transform.position;
            else
                spawnPosition = _mWallPlayerSensorL2.transform.position;

            if (m_slideDust != null)
            {
                // Set correct arrow spawn position
                GameObject dust =
                    Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
                // Turn arrow in correct direction
                dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
            }
        }

        public Vector3 GetCurrentPosition() => transform.position;
    

}
}