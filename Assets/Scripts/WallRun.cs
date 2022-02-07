using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WallRun : MonoBehaviour
{
    [SerializeField] Transform _orientation;

    [Header("Detection")]
    [Tooltip("Max Distance to wall")]
    [SerializeField] private float _wallDistance = 5f;
    [Tooltip("Wall jump height")]
    [SerializeField] private float _minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [Tooltip("Gravity acting on the player during wall run")]
    [SerializeField] private float _wallRunGravity;
    [Tooltip("Wall jump force")]
    [SerializeField] private float _wallRunJumpForce;

    [Header("Camera")]
    [Tooltip("Player camera")]
    [SerializeField] private Camera CameraObj;
    [Tooltip("Default field of view")]
    [SerializeField] private float _fov;
    [Tooltip("Field of view during wall run")]
    [SerializeField] private float _wallRunFov;
    [Tooltip("Time to lerp field of view")]
    [SerializeField] private float _wallRunFovTime;
    [SerializeField] private float _cameraTilt;
    [SerializeField] private float _cameraTiltTime;

    private bool _isWallLeft = false;
    private bool _isWallRight = false;

    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;

    private Rigidbody _rigidBody;

    public float Tilt { get; private set; }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (_isWallLeft)
                StartWallRun();
            else if (_isWallRight)
                StartWallRun();
            else
                StopWallRun();
        }
        else
            StopWallRun();
    }

    private void CheckWall()
    {
        _isWallLeft = Physics.Raycast(transform.position, -_orientation.right, out _leftWallHit, _wallDistance);
        _isWallRight = Physics.Raycast(transform.position, _orientation.right, out _rightWallHit, _wallDistance);
    }

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, _minimumJumpHeight);
    }

    private void StartWallRun()
    {
        _rigidBody.useGravity = false;
        _rigidBody.AddForce(Vector3.down * _wallRunGravity, ForceMode.Force);

        CameraObj.fieldOfView = Mathf.Lerp(CameraObj.fieldOfView, _wallRunFov, _wallRunFovTime * Time.deltaTime);

        if (_isWallLeft)
            Tilt = Mathf.Lerp(Tilt, -_cameraTilt, _cameraTiltTime * Time.deltaTime);
        if (_isWallRight)
            Tilt = Mathf.Lerp(Tilt, _cameraTilt, _cameraTiltTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void StopWallRun()
    {
        _rigidBody.useGravity = true;
        CameraObj.fieldOfView = Mathf.Lerp(CameraObj.fieldOfView, _fov, _wallRunFovTime * Time.deltaTime);
        Tilt = Mathf.Lerp(Tilt, 0, _cameraTiltTime * Time.deltaTime);
    }

    private void Jump()
    {
        Vector3 wallRunJumpDirection = Vector3.zero;

        if (_isWallLeft)
            wallRunJumpDirection = transform.up + _leftWallHit.normal;
        else if (_isWallRight)
            wallRunJumpDirection = transform.up + _rightWallHit.normal;

        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
        _rigidBody.AddForce(wallRunJumpDirection * _wallRunJumpForce * 100, ForceMode.Force);
    }

}
