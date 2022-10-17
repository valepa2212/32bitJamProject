using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _speed = 11f;

    [SerializeField]
    private float _maxRotation = 90f;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float _sensitivity = 1;

    private float _currentRotation = 0f;

    private Vector2 deltaLook;

    private Vector3 _moveDirection;
    public CharacterController CharacterController { get => _characterController; }
    public void SetMove(CallbackContext context)
    {
        _moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0 , context.ReadValue<Vector2>().y);
    }

    public void SetLook(CallbackContext context)
    {
        deltaLook = context.ReadValue<Vector2>() * _sensitivity;
    }

    public void Jump(CallbackContext context)
    {
        throw new NotImplementedException("No Jump Made");
    }

    public void Use(CallbackContext context)
    {
        throw new NotImplementedException("No Use Made");
    }

    public void Pause(CallbackContext context)
    {
        throw new NotImplementedException("No Pause Made");
    }

    public void Inventory(CallbackContext context)
    {
        throw new NotImplementedException("No Inventory Made");
    }

    private void Update()
    {
        //Movement
        CharacterController.Move(_speed * Time.deltaTime * transform.TransformDirection(_moveDirection));

        //Camera Rotation
        transform.Rotate(0, deltaLook.x, 0);
        if (Math.Abs(_currentRotation + deltaLook.y) < _maxRotation)
        {
            _camera.transform.Rotate(-deltaLook.y, 0, 0);
            _currentRotation += deltaLook.y;
        }
    }
}
