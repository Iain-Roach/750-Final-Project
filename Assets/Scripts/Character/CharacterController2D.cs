using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerController
{
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;
    public Vector2 FrameInput { get; }
}

public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public Vector2 Move;
    public Vector2 Rotate;
    public bool Shoot;
}

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class CharacterController2D : MonoBehaviour, IPlayerController
{

    [SerializeField] GameObject pauseMenu;
    GameObject gun;
    Vector2 lastGunAngle;


    //[SerializeField] public CharacterStats _stats;
    //private Rigidbody2D _rb;
    private Rigidbody _rb;
    //private CapsuleCollider2D _col;
    [SerializeField]
    private BoxCollider _col;
    public float magicDistNum = 0.0026f;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    private bool _chachedQueryStartInColliders;


    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;
    


    private float _time;


    private bool _jumpToConsume = false;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + JumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + CoyoteTime;


    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded;

    [Header("LAYERS")]
    [Tooltip("Set this to the layer your player is on")]
    public LayerMask PlayerLayer;

    [Header("INPUT")]
    [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keybaord parity.")]
    public bool SnapInput = true;

    [Tooltip("Minimum input required before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
    public float VerticalDeadZoneThreshold = 0.3f;

    [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
    public float HorizontalDeadZoneThreshold = 0.1f;

    [Header("MOVEMENT")]
    [Tooltip("The top horizontal movement speed")]
    public float MaxSpeed = 14;

    [Tooltip("The player's capacity to gain horizontal speed")]
    public float Acceleration = 120;

    [Tooltip("The pace at which the player comes to a stop")]
    public float GroundDeceleration = 60;

    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 30;

    [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
    public float GroundingForce = -1.5f / 5;

    [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
    public float GrounderDistance = 0.05f;

    [Header("JUMP")]
    [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 36;

    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 10;

    [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
    public float FallAcceleration = 110/4;

    [Tooltip("The gravity multiplier added when jump is released early")]
    public float JumpEndEarlyGravityModifier = 3;

    [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public float CoyoteTime = .15f;

    [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
    public float JumpBuffer = .2f;

    Vector2 move;
    PlayerControls controls;




    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Shoot.performed += ctx => ShootGun();
        
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<BoxCollider>();
        gun = gameObject.transform.GetChild(0).gameObject;
        

        // _chachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        
    }

    public void Start()
    {
        //controls = new PlayerControls();
        _jumpToConsume = false;
        _bufferedJumpUsable = false;
        _grounded = false;
        _coyoteUsable = false;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        GatherInput();
    }

    
    private void GatherInput()
    {
        Gamepad gamepad = Gamepad.current;
        
        if(gamepad == null)
        {
            Debug.Log("No Gamepad");
            _frameInput = new FrameInput
            {

                //JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space),
                JumpDown = Input.GetKeyDown(KeyCode.W),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.W),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };
        }
        else
        {
            _frameInput = new FrameInput
            {

                //JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space),
                JumpDown = controls.Gameplay.Jump.triggered,
                JumpHeld = controls.Gameplay.Jump.IsPressed(),
                Move = controls.Gameplay.Move.ReadValue<Vector2>(),
                Rotate = controls.Gameplay.Rotate.ReadValue<Vector2>(),
                Shoot = controls.Gameplay.Shoot.triggered
            };
        }

        

        if (lastGunAngle != Vector2.zero && _frameInput.Rotate == Vector2.zero)
        {
            _frameInput.Rotate = lastGunAngle;
        }
        lastGunAngle = _frameInput.Rotate;

        if (SnapInput)
        {
            _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
            _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
        }

        if (_frameInput.JumpDown)
        {
            Debug.Log("Jump Input");
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (pauseMenu.activeSelf)
        //    {
        //        pauseMenu.GetComponent<UIManager>().UnPauseGame();
        //    }
        //    else
        //    {
        //        pauseMenu.GetComponent<UIManager>().PauseGame();
        //    }
        //    pauseMenu.SetActive(!pauseMenu.activeSelf);

        //}
    }

    void RotateGun()
    {
        Gamepad gamepad = Gamepad.current;
        float rotationSpeed = 5.0f;
        float orbitRadius = 1.0f;
        if(gamepad != null)
        {
            Vector2 direction = new Vector2(_frameInput.Rotate.x, _frameInput.Rotate.y);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float radians = angle * Mathf.Deg2Rad;

            Vector3 orbitPosition = new Vector3(Mathf.Cos(radians) * orbitRadius, Mathf.Sin(radians) * orbitRadius, 0);

            gun.transform.localPosition = orbitPosition;

            gun.transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
            //gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.deltaTime);
        }
        
    }

    void ShootGun()
    {
        
            Debug.Log("Destruction Gun Fire");
  
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        
        RotateGun();
        HandleJump();
        HandleDirection();
        HandleGravity();

        ApplyMovement();
    }


    private void CheckCollisions()
    {
        // Physics2D.queriesStartInColliders = false;

        //bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, GrounderDistance, ~PlayerLayer);
        //bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, GrounderDistance, ~PlayerLayer);

        bool groundHit = Physics.BoxCast(_col.bounds.center + Vector3.up, _col.bounds.extents, Vector3.down, Quaternion.identity, 1 + magicDistNum, ~PlayerLayer);
        RaycastHit hit;
        bool groundCheck = Physics.BoxCast(_col.bounds.center + Vector3.up, _col.bounds.extents, Vector3.down, out hit,Quaternion.identity, 1 + magicDistNum, ~PlayerLayer);


        if (groundCheck)
        {
            if(hit.transform.tag == "Ground")
            {
                Debug.Log("Hit Ground");
            }
        }

        // Need to fix this still
        bool ceilingHit = Physics.BoxCast(_col.bounds.center, _col.bounds.extents, Vector3.up, Quaternion.identity, 1 + magicDistNum, ~PlayerLayer);
        
        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }

        // Physics2D.queriesStartInColliders = _chachedQueryStartInColliders;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position + Vector3.down * (transform.localScale.y / 2f), transform.localScale);
        //Gizmos.DrawRay(transform.position + Vector3.down * (transform.localScale.y / 2f), Vector3.down / 2);
        
    }

    private void HandleJump()
    {
        Debug.Log("Grounded? " + _grounded);
        Debug.Log("Jump To Consume? " + _jumpToConsume);

        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if ((_grounded || CanUseCoyote) && _jumpToConsume) ExecuteJump();

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        Debug.Log("JUMPED");
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = JumpPower;
        Jumped?.Invoke();
    }



    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            float deceleration = _grounded ? GroundDeceleration : AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * MaxSpeed, Acceleration * Time.fixedDeltaTime);
        }
    }

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = GroundingForce;
        }
        else
        {
            float inAirGravity = FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= JumpEndEarlyGravityModifier;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    private void ApplyMovement() => _rb.velocity = _frameVelocity;

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // if (_stats == null) Debug.LogWarning("Please Assign a ScriptableStats asset to the Player Controller's Stats slot", this);
    }
#endif
}

