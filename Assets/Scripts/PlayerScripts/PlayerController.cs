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
    private GameObject _popUpText;

    [SerializeField]
    private GameObject _pauseMenu;

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

    private Interactable _interactable;

    private GameObject _heldItem;

    private PlacePosition _placePosition;

    private bool _holding = false;

    private bool _isPaused = false;

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
        if (!_holding && _interactable != null)
        {
            _heldItem = _interactable.gameObject;
            _heldItem.transform.SetParent(_holdPosition.transform, false);
            _heldItem.transform.localPosition = Vector3.zero;
            _interactable = null;
            _holding = true;
        }
        if (_holding && _placePosition != null)
        {
            _heldItem.transform.SetParent(_placePosition.transform, false);
            _heldItem.transform.localPosition = Vector3.zero;
            Destroy(_heldItem.GetComponent<Interactable>());
            Destroy(_placePosition.GetComponent<PlacePosition>());
            _holding = false;
        }
    }

    public void Pause(CallbackContext context)
    {
        if (!_isPaused)
        {
            _isPaused = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _pauseMenu.SetActive(true);
        }
        else
        {
            _isPaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _pauseMenu.SetActive(false);
        }
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
        if (!_isPaused)
        {
            transform.Rotate(0, deltaLook.x, 0);
            if (Math.Abs(_currentRotation + deltaLook.y) < _maxRotation)
            {
                _camera.transform.Rotate(-deltaLook.y, 0, 0);
                _currentRotation += deltaLook.y;
            }
        }

        //Check if looking at interactable object or place position
        RaycastHit hit;
        _popUpText.SetActive(false);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward), out hit, 2f) )
        {
            if (!_holding)
            {
                _interactable = hit.collider.GetComponent<Interactable>();
                if (_interactable != null)
                {
                    _popUpText.SetActive(true);
                }
            }
            if (_holding)
            {
                _placePosition = hit.collider.GetComponent<PlacePosition>();
                if (_placePosition != null)
                {
                    _popUpText.SetActive(true);
                }
            }
        }
    }
}
