using UnityEngine;

/// <summary>
/// to do:
/// Add tooltip to the _groundCheck field
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private const float HalfOfThePlayerHeight = 1f;

    [Tooltip("Orientation tracking object")]
    [SerializeField] private Transform _orientation;

    [Header("Movement")]
    [Tooltip("Player movement speed on the ground")]
    public float MovementSpeed;
    [Tooltip("Player movement speed on the air")]
    [SerializeField] private float _airMultiplier = 0.4f;
    private const float SpeedMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] private float WalkSpeed = 4f;
    [SerializeField] private float SprintSpeed = 6f;
    [SerializeField] private float Acceleration = 6f;

    [Header("Jumping")]
    [Tooltip("The force that pushes the player up")]
    public float JumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [Tooltip("Drag on on the ground")]
    [SerializeField] private float _groundDrag = 6f;
    [Tooltip("Drag on on the air")]
    [SerializeField] private float _airDrag = 2f;

    private float _verticalMovement;
    private float _horizontalMovement;

    [Header("Ground Detection")]
    //Add tooltip
    [SerializeField] Transform _groundCheck;
    [Tooltip("Layer of ground objects")]
    [SerializeField] LayerMask _groundMask;
    private bool _isGrounded;
    private float _groundDistance = 0.4f;

    private Vector3 _moveDirection;
    private Vector3 _slopeMoveDiretion;

    private Rigidbody _rigidBody;
    RaycastHit slopeHit;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        HandleInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            Jump();
        }

        _slopeMoveDiretion = Vector3.ProjectOnPlane(_moveDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, HalfOfThePlayerHeight + 0.5f))
        {
            return slopeHit.normal != Vector3.up;
        }
        return false;
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
        _rigidBody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void HandleInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _moveDirection = _orientation.forward * _verticalMovement + _orientation.right * _horizontalMovement;
    }

    private void ControlSpeed()
    {
            MovementSpeed = Mathf.Lerp(MovementSpeed, Input.GetKey(_sprintKey) && _isGrounded ?
                SprintSpeed : WalkSpeed, Acceleration * Time.deltaTime);
    }

    private void ControlDrag()
    {
        _rigidBody.drag = _isGrounded ? _groundDrag : _airDrag;
    }

    private void MovePlayer()
    {
        if (_isGrounded)
        {
            _rigidBody.AddForce((OnSlope() ? _slopeMoveDiretion.normalized : _moveDirection.normalized)
                * MovementSpeed * SpeedMultiplier, ForceMode.Acceleration);
        }
        else
            _rigidBody.AddForce(_moveDirection.normalized * MovementSpeed * SpeedMultiplier *
                _airMultiplier, ForceMode.Acceleration);
    }
}
