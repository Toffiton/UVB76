using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BedController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
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
        controls.Main.Interact.performed += Sleep;
        controls.Main.LKM.performed += Sleep;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= Sleep;
        controls.Main.LKM.performed -= Sleep;
    }

    private void Sleep(InputAction.CallbackContext obj)
    {
        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            if (mainGame.GetCurrentDay() == 6)
            {
                mainGame.SetCurrentDay(1);
                SceneManager.LoadScene(0);
                return;
            }

            isSiting = true;
            player.SitOnChair(playerPosition, playerRotation);
            mainGame.SetCurrentDay(mainGame.GetCurrentDay() + 1);
            SceneManager.LoadScene(0);
        }
    }
}