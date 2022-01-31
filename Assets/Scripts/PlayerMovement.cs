using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private const float GroundDetectionPosition = 1f;

    [SerializeField] private Transform _orientation;

    [Header("Movement")]
    public float MovementSpeed;
    [SerializeField] private float _airMultiplier = 0.4f;
    private const float SpeedMultiplier = 10f;

    [Header("Jumping")]
    public float JumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("Drag")]
    [SerializeField] private float _groundDrag = 6f;
    [SerializeField] private float _airDrag = 2f;

    private float _verticalMovement;
    private float _horizontalMovement;

    [Header("Ground Detection")]
    [SerializeField] LayerMask _groundMask;
    private bool _isGrounded;
    private float _groundDistance = 0.4f;

    private Vector3 _moveDirection;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position -
            new Vector3(0, GroundDetectionPosition, 0), _groundDistance, _groundMask);
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

        _moveDirection = _orientation.forward * _verticalMovement + _orientation.right * _horizontalMovement;
    }

    private void ControlDrag()
    {
        _rigidBody.drag = _isGrounded ? _groundDrag : _airDrag;
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
