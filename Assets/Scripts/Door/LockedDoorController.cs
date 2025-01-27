using UnityEngine;using System.Collections;
using UnityEngine.InputSystem;

public class LockedDoorController : MonoBehaviour
{
    [SerializeField] private Transform doorHinge;
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private AudioSource sound;

    private bool isDoorOpen = false;

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
            if (sound.isPlaying)
            {
                sound.Stop();
                sound.Play();
            }
            else
            {
                sound.Play();
            }
        }
    }
}