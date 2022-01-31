using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private const float GroundDrag = 6f;
    private const float AirDrag = 2f;
    private const float PlayerHeight = 2f;

    [Header("Movement")]
    public float MovementSpeed;
    [SerializeField] private float _airMultiplier = 0.4f;
    private const float SpeedMultiplier = 10f;

    [Header("Jumping")]
    public float JumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    private float _verticalMovement;
    private float _horizontalMovement;
    private Vector3 _moveDirection;
    private Rigidbody _rigidBody;
    private bool _isGrounded;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight / 2f + 0.1f);

        HandleInput();
        ControlDrag();

        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rigidBody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void HandleInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _moveDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement;
    }

    private void ControlDrag()
    {
        _rigidBody.drag = _isGrounded ? GroundDrag : AirDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();    
    }

    private void MovePlayer()
    {
        if(_isGrounded)
            _rigidBody.AddForce(_moveDirection.normalized * MovementSpeed * SpeedMultiplier,
                ForceMode.Acceleration);
        else
            _rigidBody.AddForce(_moveDirection.normalized * MovementSpeed * SpeedMultiplier *
                _airMultiplier, ForceMode.Acceleration);
    }
}
