using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class SleepBoonkerDoor: MonoBehaviour
{
    
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private BedController bedController;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Interact.performed += HandleInteract;
        controls.Main.LKM.performed += HandleInteract;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= HandleInteract;
        controls.Main.LKM.performed -= HandleInteract;
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            StartCoroutine(bedController.ExitSleepLevelTransition());
        }
    }
}