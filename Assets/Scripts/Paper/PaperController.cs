using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = System.Numerics.Vector2;

public class PaperController : MonoBehaviour
{
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private GameObject defaultHand;
    [SerializeField] private GameObject paper;

    private Vector3 paperPosition;
    private Quaternion paperRotation;

    private bool isTaked = false;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Interact.performed += TakePaper;
        controls.Main.LKM.performed += TakePaper;
        controls.Main.Exit.performed += DropPaper;
        controls.Main.PKM.performed += DropPaper;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= TakePaper;
        controls.Main.LKM.performed -= TakePaper;
        controls.Main.Exit.performed -= DropPaper;
        controls.Main.PKM.performed -= DropPaper;
    }

    private void TakePaper(InputAction.CallbackContext obj)
    {
        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            isTaked = true;
            paper.SetActive(true);
        }
    }

    private void DropPaper(InputAction.CallbackContext obj)
    {
        if (isTaked)
        {
            isTaked = false;
            paper.SetActive(false);
        }
    }
}
