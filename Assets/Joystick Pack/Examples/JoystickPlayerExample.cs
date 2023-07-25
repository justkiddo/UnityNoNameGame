using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
    [SerializeField] private float speed = 4f;
    
    public VariableJoystick variableJoystick;
    public Rigidbody2D _body2d;
    private float _mJumpForce = 6f;
    private bool _isGrounded;
    private PlayerSensor _mGroundPlayerSensor;
    private Animator _animator;

    private void Start()
    {
        _mGroundPlayerSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
        _animator = GetComponent<Animator>();
    }

    
    private void Awake()
    {
        
    }


    private void Update()
    {
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
        
        Move();
        Jump();
    }

    private void Move()
    {
        _body2d.velocity = new Vector2(variableJoystick.Horizontal * speed, _body2d.velocity.y);
        
    }

    private void Jump()
    {
        if (variableJoystick.Vertical > 0 && _isGrounded )
        {
            _body2d.velocity = new Vector2(variableJoystick.Vertical, _mJumpForce);
        }
    }
}