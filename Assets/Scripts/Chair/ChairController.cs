using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = System.Numerics.Vector2;

public class ChairController : MonoBehaviour
{
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Quaternion playerRotation;

    private bool isSiting = false;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Interact.performed += SitOnChair;
        controls.Main.Exit.performed += GetUpFromChair;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= SitOnChair;
        controls.Main.Exit.performed -= GetUpFromChair;
    }

    private void SitOnChair(InputAction.CallbackContext obj)
    {
        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            isSiting = true;
            player.SitOnChair(playerPosition, playerRotation);
        }
    }

    private void GetUpFromChair(InputAction.CallbackContext obj)
    {
        if (isSiting)
        {
            player.GetUpFromChair();
            isSiting = false;
        }
    }
}
