using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainGame : MonoBehaviour
{
    public bool isTakedItem;
    public bool isFirstLoadGame;
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private GameObject informationBlock;
    [SerializeField] private TextMeshProUGUI informationText;

    private int currentTextStep = 0;
    
    private Controls controls;

    public IEnumerator SetIsTakedItemWithDelay(bool isTakedItem)
    {
        yield return new WaitForSeconds(0.1f);
        this.isTakedItem = isTakedItem;
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