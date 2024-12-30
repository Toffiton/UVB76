using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TakedItem takedItem;

    [SerializeField] private GameObject defaultHand;
    [SerializeField] private GameObject handWithPhone;
    [SerializeField] private AudioSource handWithPhoneAudio;
    
    private Vector3 defaultPosition;

    private bool isTaked = false;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Interact.performed += TakeCall;
        controls.Main.LKM.performed += TakeCall;
        controls.Main.Exit.performed += DropCall;
        controls.Main.PKM.performed += DropCall;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= TakeCall;
        controls.Main.LKM.performed -= TakeCall;
        controls.Main.Exit.performed -= DropCall;
        controls.Main.PKM.performed -= DropCall;
    }

    private void Start()
    {
        defaultPosition = transform.position;
    }

    private void TakeCall(InputAction.CallbackContext obj)
    {
        if (mainGame.isTakedItem)
        {
            return;
        }

        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            mainGame.StopPhoneSound();
            mainGame.isTakedItem = true;
            isTaked = true;
            defaultHand.SetActive(false);
            handWithPhone.SetActive(true);
            if (!PhonePrefs.GetPhoneCallIsListenById(mainGame.GetCurrentDay()) && mainGame.isQuestStarted)
            {
                handWithPhoneAudio.Play();
                PhonePrefs.SetPhoneCallIsListenById(mainGame.GetCurrentDay());
            }
            HidePhone();
        }
    }

    private void DropCall(InputAction.CallbackContext obj)
    {
        if (isTaked)
        {
            StartCoroutine(mainGame.SetIsTakedItemWithDelay(false));
            isTaked = false;

            if (handWithPhoneAudio.isPlaying)
            {
                handWithPhoneAudio.Stop();
            }
            handWithPhone.SetActive(false);
            defaultHand.SetActive(true);

            ShowPhone();
        }
    }

    private void ShowPhone()
    {
        transform.position = defaultPosition;
    }

    private void HidePhone()
    {
        transform.transform.position = new Vector3(0, 0, 0);
    }
}
