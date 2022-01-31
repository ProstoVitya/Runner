using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float _xSensitivity;
    [SerializeField] private float _ySensitivity;

    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Transform _orientation;

    private float _mouseX;
    private float _mouseY;

    private float _multiplier = 0.01f;

    private float _yRotation;
    private float _xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();
        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _orientation.transform.localRotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private void HandleInput()
    {
        _mouseX = Input.GetAxisRaw("Mouse X");
        _mouseY = Input.GetAxisRaw("Mouse Y");

        _yRotation += _mouseX * _xSensitivity * _multiplier;
        _xRotation -= _mouseY * _ySensitivity * _multiplier;

        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
    }
}
