using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _speed = 11f;

    private Vector3 _moveDirection;
    public CharacterController CharacterController { get => _characterController; }
    public void SetMove(CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector3>();
    }

    public void SetLook(CallbackContext context)
    {
        throw new NotImplementedException("No Camera Movement Made");
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
        CharacterController.Move(_moveDirection * _speed * Time.deltaTime);
    }
}
