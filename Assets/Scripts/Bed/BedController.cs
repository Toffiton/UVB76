using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BedController : MonoBehaviour
{
    [Header("Камеры")]
    [SerializeField] private Transform playerCamera; // Камера игрока
    [SerializeField] private Transform bedCamera; // Камера, направленная на кровать

    [Header("Эффект затемнения")]
    [SerializeField] private Image fadeImage; // Чёрный экран для затемнения
    [SerializeField] private float fadeDuration = 1.5f; // Длительность затемнения

    [Header("Настройки камеры")]
    [SerializeField] private float transitionDuration = 2f; // Длительность перемещения камеры

    [SerializeField] private MainGame mainGame;
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Quaternion playerRotation;
    [SerializeField] private TextMeshProUGUI infoText;

    private bool isSleeping = false;

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

            StartSleepTransition();
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

    public void StartSleepTransition()
    {
        if (!isSleeping)
        {
            StartCoroutine(SleepCoroutine());
        }
    }

    private IEnumerator SleepCoroutine()
    {
        isSleeping = true;
        player.isPlayerStopLooking = true;

        Vector3 initialCameraPosition = playerCamera.position;
        Quaternion initialCameraRotation = playerCamera.rotation;

        yield return SmoothCameraTransition(playerCamera, bedCamera);

        yield return FadeScreen(0f, 1f);

        yield return new WaitForSeconds(2f);

        yield return FadeScreen(1f, 0f);

        yield return SmoothCameraTransition(bedCamera, playerCamera, initialCameraPosition, initialCameraRotation);

        isSleeping = false;
        player.isPlayerStopLooking = false;
        mainGame.NotifyAboutNextDay();
    }

    private IEnumerator FadeScreen(float startAlpha, float endAlpha)
    {
        Color fadeColor = fadeImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeColor.a = alpha;
            fadeImage.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        fadeImage.color = fadeColor;
    }

    private IEnumerator SmoothCameraTransition(
        Transform fromCamera,
        Transform toCamera,
        Vector3? overrideTargetPosition = null,
        Quaternion? overrideTargetRotation = null
    )
    {
        Vector3 targetPosition = overrideTargetPosition ?? toCamera.position;
        Quaternion targetRotation = overrideTargetRotation ?? toCamera.rotation;

        Vector3 startPosition = fromCamera.position;
        Quaternion startRotation = fromCamera.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            playerCamera.position = Vector3.Lerp(startPosition, targetPosition, t);
            playerCamera.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        playerCamera.position = targetPosition;
        playerCamera.rotation = targetRotation;
    }
}