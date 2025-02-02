using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TakedItem takedItem;

    [SerializeField] private GameObject defaultHand;
    [SerializeField] private GameObject handWithPhone;
    [SerializeField] private PhoneSoundController phoneSoundController;
    [SerializeField] private ChairController chairController;

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

    private void Update()
    {
        if (mainGame.isPhoneCallEnded)
        {
            DropCall(new InputAction.CallbackContext());
        }
    }

    private void TakeCall(InputAction.CallbackContext obj)
    {
        if (mainGame.isTakedPhone)
        {
            return;
        }

        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            mainGame.StopPhoneSound();
            mainGame.isTakedPhone = true;
            isTaked = true;
            defaultHand.SetActive(false);
            handWithPhone.SetActive(true);
            if (!PhonePrefs.GetPhoneCallIsListenById(mainGame.GetCurrentDay()) && chairController.isSiting)
            {
                StopAllCoroutines();
                phoneSoundController.StartCall();
                mainGame.isQuestStarted = false;
            }
            HidePhone();
        }
    }

    private void DropCall(InputAction.CallbackContext obj)
    {
        if (isTaked && !mainGame.isTakedPaper)
        {
            StartCoroutine(mainGame.SetIsTakedPaperWithDelay(false));
            isTaked = false;
            mainGame.ExecuteSetIsTakedPhoneWithDelay(false);
            phoneSoundController.CancelCall();
            handWithPhone.SetActive(false);
            defaultHand.SetActive(true);

            ShowPhone();
            
            StopAllCoroutines();
            StartCoroutine(CheckPhoneCallIsEnd());
        }
    }

    private IEnumerator CheckPhoneCallIsEnd()
    {
        yield return new WaitForSeconds(4F);
        if (!chairController.isSiting)
        {
            yield return null;
        }
        if (mainGame.isPhoneCallStarted)
        {
            mainGame.PlayPhoneSound();
            mainGame.isQuestStarted = true;
        }
        yield return null;
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
