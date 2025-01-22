using System.Collections;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI infoText;

    private bool isSiting = false;

    private bool textPlayed = false;

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
            if (!mainGame.isDayCompleted)
            {
                if (!textPlayed)
                {
                    StartCoroutine(ShowInfoText());
                }
                return;
            }
            
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

    private IEnumerator ShowInfoText()
    {
        textPlayed = true;
        string message1 = "Я пока не хочу спать";
        string message2 = "Нужно поработать";

        yield return StartCoroutine(TypeText(message1, 0.05f));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText(message2, 0.05f));
        yield return new WaitForSeconds(1f);

        textPlayed = false;
        infoText.text = "";
    }

    private IEnumerator TypeText(string message, float delay)
    {
        infoText.text = "";

        foreach (char letter in message)
        {
            infoText.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }
}