using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.InputSystem;
using Vector2 = System.Numerics.Vector2;

public class PlayerController : MonoBehaviour
{
    [Header("Конфига передвижения")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float lookSensitivity = 1f;

    [Space]
    [Header("Камера")]
    [SerializeField] Transform cameraTransform;
    
    [Space]
    [Header("Руки")]
    [SerializeField] Transform hands;
    [SerializeField] float idleSwingIntensity = 0.02f;
    [SerializeField] float walkSwingIntensityHorizontal = 0.05f;
    [SerializeField] float walkSwingIntensityVertical = 0.08f;
    [SerializeField] float runSwingIntensity = 0.12f;
    [SerializeField] float idleSwingSpeed = 1f;
    [SerializeField] float walkSwingSpeed = 4f;
    [SerializeField] float runSwingSpeed = 8f;

    private Controls _controls;
    private CharacterController _characterController;
    private float _verticalSpeed;
    private float _gravity = 9.8f;
    private float _cameraVerticalRotation = 0f;
    private bool _isRunning = false;
    private bool _isJumping = false;
    public bool _isPlayerStopMovement = true;
    private Vector3 initialHandPosition;
    private Vector3 lastHandPosition;

    private void Awake()
    {
        _controls = new Controls();
        _characterController = GetComponent<CharacterController>();
        initialHandPosition = hands.localPosition;
    }

    private void OnEnable()
    {
        _controls.Main.Enable();
        _controls.Main.Jump.performed += OnJump;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        _controls.Main.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (_isPlayerStopMovement)
        {
            HandleMovement();
            HandleLook();
            HandleHandSwing();
        }
    }

    private void HandleMovement()
    {
        float horizontalMove = _controls.Main.MoveHorizontal.ReadValue<float>();
        float verticalMove = _controls.Main.MoveVerticle.ReadValue<float>();
    
        Vector3 move = transform.right * horizontalMove + transform.forward * verticalMove;

        float currentSpeed = _isRunning ? runSpeed : moveSpeed;

        if (_characterController.isGrounded)
        {
            if (_isJumping)
            {
                _verticalSpeed = jumpForce;
                _isJumping = false;
            }
            else
            {
                _verticalSpeed = -1f;
            }
        }
        else
        {
            _verticalSpeed -= _gravity * Time.deltaTime;
        }

        move.y = _verticalSpeed;
        _characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        float lookX = Mouse.current.delta.x.ReadValue() * lookSensitivity;
        float lookY = Mouse.current.delta.y.ReadValue() * lookSensitivity;

        transform.Rotate(Vector3.up * lookX);

        _cameraVerticalRotation -= lookY;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_cameraVerticalRotation, 0f, 0f);
    }

    private void HandleHandSwing()
    {
        float swingSpeed;
        float horizontalIntensity;
        float verticalIntensity;
        float swingIntensity;

        // Запоминаем текущую позицию рук для плавного перехода
        if (lastHandPosition == Vector3.zero)
        {
            lastHandPosition = hands.localPosition;
        }

        // Устанавливаем параметры для разных состояний
        if (_characterController.velocity.magnitude < 0.1f)
        {
            swingIntensity = idleSwingIntensity;
            swingSpeed = idleSwingSpeed;
            horizontalIntensity = verticalIntensity = idleSwingIntensity;
        }
        else if (_isRunning)
        {
            swingIntensity = runSwingIntensity;
            swingSpeed = runSwingSpeed;
            horizontalIntensity = verticalIntensity = runSwingIntensity;
        }
        else
        {
            horizontalIntensity = walkSwingIntensityHorizontal;
            verticalIntensity = walkSwingIntensityVertical;
            swingSpeed = walkSwingSpeed;
            swingIntensity = Mathf.Max(horizontalIntensity, verticalIntensity);
        }

        // Вычисляем новое положение рук на основе текущего времени и заданных параметров
        Vector3 targetPosition = initialHandPosition + new Vector3(
            Mathf.Sin(Time.time * swingSpeed) * horizontalIntensity,
            Mathf.Sin(Time.time * swingSpeed * 1.5f) * verticalIntensity,
            Mathf.Cos(Time.time * swingSpeed) * swingIntensity
        );

        // Плавно переходим к целевой позиции рук, используя последнюю сохраненную позицию
        hands.localPosition = Vector3.Lerp(lastHandPosition, targetPosition, Time.deltaTime * swingSpeed);

        // Обновляем последнюю позицию для следующего кадра
        lastHandPosition = hands.localPosition;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded)
        {
            _isJumping = true;
        }
    }
}