using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform doorHinge;
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip closeDoorSound;

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
        MainGame.OnDayChanged += HandleDayChanged;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= HandleInteract;
        controls.Main.LKM.performed -= HandleInteract;
        MainGame.OnDayChanged -= HandleDayChanged;
    }

    private void HandleDayChanged(int newDay)
    {
        isDoorOpen = false;
        doorHinge.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        StopAllCoroutines();
        float targetAngle = isDoorOpen ? 0f : openAngle;

        if (isDoorOpen)
        {
            sound.clip = openDoorSound;
            sound.Stop();
            sound.Play();
        }
        else
        {
            sound.clip = closeDoorSound;
            sound.Stop();
            sound.Play();
        }

        StartCoroutine(RotateDoor(targetAngle));
        isDoorOpen = !isDoorOpen;
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        float currentAngle = doorHinge.localEulerAngles.y;
        if (currentAngle > 180f) currentAngle -= 360f;

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, speed * Time.deltaTime);
            doorHinge.localEulerAngles = new Vector3(0f, currentAngle, 0f);
            yield return null;
        }

        doorHinge.localEulerAngles = new Vector3(0f, targetAngle, 0f);
    }
}