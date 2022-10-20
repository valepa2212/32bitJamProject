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
    private GameObject _holdPosition;

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

    private PickUp _pickUp;

    private bool _holding = false;

    public CharacterController CharacterController { get => _characterController; }

    public void SetMove(CallbackContext context)
    {
        _moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0 , context.ReadValue<Vector2>().y);
    }

    public void SetLook(CallbackContext context)
    {
        deltaLook = context.ReadValue<Vector2>() * _sensitivity;
    }

    public void Use(CallbackContext context)
    {
        if (!_holding && _pickUp != null)
        {
            _pickUp.transform.SetParent(_holdPosition.transform, false);
            _pickUp.transform.localPosition = Vector3.zero;
            _holding = true;
        }
    }

    public void Pause(CallbackContext context)
    {
        throw new NotImplementedException("No Pause Made");
    }


    private void Start()
    {
        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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


        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward), out hit, 2f) )
        {
            Debug.DrawRay(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            _pickUp = hit.collider.GetComponent<PickUp>();
        }
    }
}
