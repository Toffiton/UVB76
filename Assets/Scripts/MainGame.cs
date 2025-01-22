using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MainGame : MonoBehaviour
{
    public bool isTakedPaper;
    public bool isTakedPhone;
    public bool isFirstLoadGame;
    
    public bool isPhoneCallStarted = false;
    public bool isPhoneCallEnded = false;

    public bool isTakedPaperOnFax = false;

    public bool isDayCompleted = false;

    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private GameObject informationBlock;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private AudioSource phoneSound;

    private int currentTextStep = 0;
    
    private Controls controls;
    

    public void ExecuteSetIsTakedPaperWithDelay(bool isTakedItem)
    {
        StartCoroutine(SetIsTakedPaperWithDelay(isTakedItem));
    }

    public IEnumerator SetIsTakedPaperWithDelay(bool isTakedItem)
    {
        yield return new WaitForSeconds(0.1f);
        this.isTakedPaper = isTakedItem;
    }

    public void ExecuteSetIsTakedPhoneWithDelay(bool isTakedItem)
    {
        StartCoroutine(SetIsTakedPhoneWithDelay(isTakedItem));
    }

    public IEnumerator SetIsTakedPhoneWithDelay(bool isTakedItem)
    {
        yield return new WaitForSeconds(0.1f);
        this.isTakedPhone = isTakedItem;
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
        Debug.Log("day" + GetCurrentDay());
        if (GetIsFirstLoadGame() == 1)
        {
            SwitchText();
        }
        else
        {
            playerSpawner.SpawnPlayerOnDefaultPosition();
            player.isPlayerStopMovement = true;
            informationBlock.SetActive(false);
        }
    }

    public void PlayPhoneSound()
    {
        phoneSound.Play();
    }

    public void StopPhoneSound()
    {
        phoneSound.Stop();
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
                informationText.text = "UVB-76";

                break;
            case 2:
                informationText.text = "You";

                break;
            case 3:
                informationBlock.SetActive(false);
                playerSpawner.SpawnPlayerOnStartPosition();
                player.isPlayerStopMovement = true;
                SetIsFirstLoadGameFalse();

                break;
        }
        
        currentTextStep += 1;
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