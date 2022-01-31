using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float _xSensitivity;
    [SerializeField] private float _ySensitivity;

    private Camera _playerCamera;
    private float _mouseX;
    private float _mouseY;

    private float _multiplier = 0.01f;

    private float _xRotation;
    private float _yRotation;

    private void Start()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();
        _playerCamera.transform.localRotation = Quaternion.Euler(_yRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(0, _xRotation, 0);
    }

    private void HandleInput()
    {
        _mouseX = Input.GetAxisRaw("Mouse X");
        _mouseY = Input.GetAxisRaw("Mouse Y");

        _xRotation += _mouseX * _xSensitivity * _multiplier;
        _yRotation -= _mouseY * _ySensitivity * _multiplier;

        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
    }
}
