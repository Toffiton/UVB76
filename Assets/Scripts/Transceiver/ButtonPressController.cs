using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPressController : MonoBehaviour
{
    [SerializeField] private Vector3 pressedPositionOffset = new Vector3(0, -0.01f, 0);
    [SerializeField] private float pressSpeed = 5f;
    [SerializeField] private TakedItem takedItem;

    private Vector3 initialLocalPosition;
    private Vector3 targetLocalPosition;
    private Controls controls;

    public bool isButtonPressed = false;

    private void Awake()
    {
        // Инициализация управления
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Interact.performed += OnButtonPressed;
        controls.Main.Interact.canceled += OnButtonReleased;
        controls.Main.LKM.performed += OnButtonPressed;
        controls.Main.LKM.canceled += OnButtonReleased;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= OnButtonPressed;
        controls.Main.Interact.canceled -= OnButtonReleased;
        controls.Main.LKM.performed -= OnButtonPressed;
        controls.Main.LKM.canceled -= OnButtonReleased;
    }

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        targetLocalPosition = initialLocalPosition;
    }

    private void Update()
    {
        if (!takedItem.GetPlayerInRange() || !takedItem.GetItemIsSelected())
        {
            isButtonPressed = false;
            targetLocalPosition = initialLocalPosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, pressSpeed * Time.deltaTime);
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            isButtonPressed = true;
            targetLocalPosition = initialLocalPosition + pressedPositionOffset;
        }
    }

    private void OnButtonReleased(InputAction.CallbackContext context)
    {
        isButtonPressed = false;
        targetLocalPosition = initialLocalPosition;
    }
}