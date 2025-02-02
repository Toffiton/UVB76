using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    public bool isTakedPaper;
    public bool isTakedPhone;
    public bool isFirstLoadGame;
    
    public bool isPhoneCallStarted = false;
    public bool isPhoneCallEnded = false;

    public bool isTakedPaperOnFax = false;

    public bool isDayCompleted = false;

    public bool isQuestStarted = false;

    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private GameObject informationTopText;
    [SerializeField] private GameObject informationHelperText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private AudioSource phoneSound;

    private int currentTextStep = 0;
    
    public static event Action<int> OnDayChanged;
    
    private Controls controls;
    

    public void ExecuteSetIsTakedPaperWithDelay(bool isTakedItem)
    {
        StartCoroutine(SetIsTakedPaperWithDelay(isTakedItem));
    }

    public IEnumerator SetIsTakedPaperWithDelay(bool isTakedItem)
    {
        yield return new WaitForSeconds(0.1f);
        isTakedPaper = isTakedItem;
    }

    public void ExecuteSetIsTakedPhoneWithDelay(bool isTakedItem)
    {
        StartCoroutine(SetIsTakedPhoneWithDelay(isTakedItem));
    }

    public IEnumerator SetIsTakedPhoneWithDelay(bool isTakedItem)
    {
        yield return new WaitForSeconds(0.1f);
        isTakedPhone = isTakedItem;
    }

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Jump.performed += SwitchTextExecute;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Jump.performed -= SwitchTextExecute;
    }

    private void Start()
    {
        if (PhonePrefs.GetPhoneCallIsListenById(GetCurrentDay()))
        {
            isPhoneCallEnded = true;
        }

        Debug.Log("day" + GetCurrentDay());
        if (GetIsFirstLoadGame() == 1)
        {
            SwitchText();
        }
        else
        {
            playerSpawner.SpawnPlayerOnDefaultPosition();
            player.ResumeMovementAndLooking();
            StartCoroutine(Fade.FadeScreen(fadeImage, fadeDuration,1F, 0F));
            informationTopText.SetActive(false);
            informationHelperText.SetActive(false);
        }
    }

    public void PlayPhoneSound()
    {
        phoneSound.Play();
    }

    public void StopPhoneSound()
    {
        if (!PhonePrefs.GetPhoneCallIsListenById(GetCurrentDay()))
        {
            isQuestStarted = false;
            phoneSound.Stop();
        }
    }

    private void SwitchTextExecute(InputAction.CallbackContext obj)
    {
        if (GetIsFirstLoadGame() == 0)
        {
            return;
        }
        SwitchText();
    }

    private void SwitchText()
    {
        switch (currentTextStep)
        {
            case 0:
                informationText.text = "12.09.1980";

                break;
            case 1:
                informationText.text = "BKVA-67";

                break;
            case 2:
                informationText.text = "You";

                break;
            case 3:
                informationTopText.SetActive(false);
                informationHelperText.SetActive(false);
                StartCoroutine(Fade.FadeScreen(fadeImage, fadeDuration,1F, 0F));
                playerSpawner.SpawnPlayerOnStartPosition();
                player.ResumeMovementAndLooking();
                SetIsFirstLoadGameFalse();

                break;
        }
        
        currentTextStep += 1;
    }

    public void NotifyAboutNextDay()
    {
        isDayCompleted = false;
        isPhoneCallStarted = false;
        isPhoneCallEnded = false;

        int currentDay = GetCurrentDay();
        int nextDay = currentDay + 1;

        SetCurrentDay(nextDay);

        OnDayChanged?.Invoke(nextDay);
    }

    public int GetCurrentDay()
    {
        return PlayerPrefs.GetInt("CurrentDay", 1);
    }

    public void SetCurrentDay(int day)
    {
        PlayerPrefs.SetInt("CurrentDay", day);
    }

    private int GetIsFirstLoadGame()
    {
        return PlayerPrefs.GetInt("IsFirstLoadGame", 1);
    }

    private void SetIsFirstLoadGameFalse()
    {
        PlayerPrefs.SetInt("IsFirstLoadGame", 0);
    }
}