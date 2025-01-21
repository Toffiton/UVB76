using System.Collections;
using TMPro;
using UnityEngine;

public class NumericKeypadController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Кнопки")]
    [SerializeField] private ButtonPressController oneButton;
    [SerializeField] private ButtonPressController twoButton;
    [SerializeField] private ButtonPressController threeButton;
    [SerializeField] private ButtonPressController fourStepButton;
    [SerializeField] private ButtonPressController fiveStepButton;
    [SerializeField] private ButtonPressController sixStepButton;
    [SerializeField] private ButtonPressController sevenStepButton;
    [SerializeField] private ButtonPressController eightStepButton;
    [SerializeField] private ButtonPressController nineStepButton;
    [SerializeField] private ButtonPressController zeroStepButton;
    [SerializeField] private ButtonPressController acceptStepButton;
    [SerializeField] private ButtonPressController cancelStepButton;

    private bool[] buttonHandled = new bool[12];
    private bool isAnimating = true; // Флаг для анимации текста

    public string selectedCode = "";

    private const int MAX_CHARS = 5;

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    void Update()
    {
        // Проверяем все кнопки независимо от состояния анимации
        HandleButtonPress(oneButton, 0);
        HandleButtonPress(twoButton, 1);
        HandleButtonPress(threeButton, 2);
        HandleButtonPress(fourStepButton, 3);
        HandleButtonPress(fiveStepButton, 4);
        HandleButtonPress(sixStepButton, 5);
        HandleButtonPress(sevenStepButton, 6);
        HandleButtonPress(eightStepButton, 7);
        HandleButtonPress(nineStepButton, 8);
        HandleButtonPress(zeroStepButton, 9);
        
        
        HandleAcceptButtonPress(acceptStepButton, 10);
        HandleCancelButtonPress(cancelStepButton, 11);
    }

    private void HandleButtonPress(ButtonPressController button, int buttonIndex)
    {
        if (button.isButtonPressed && !buttonHandled[buttonIndex])
        {
            buttonHandled[buttonIndex] = true;

            if (isAnimating)
            {
                // Останавливаем анимацию и очищаем текст при первом нажатии
                isAnimating = false;
                StopCoroutine(AnimateText());
                displayText.text = ""; // Очищаем текстовое поле
            }

            if (displayText.text.Length < MAX_CHARS)
            {
                displayText.text += button.buttonNumber;
            }
        }
        else if (!button.isButtonPressed)
        {
            buttonHandled[buttonIndex] = false;
        }
    }

    private IEnumerator AnimateText()
    {
        string baseText = "Enter code";
        int dotCount = 0;

        while (isAnimating)
        {
            displayText.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4; // Цикл от 0 до 3
            yield return new WaitForSeconds(0.5f); // Задержка между изменениями текста
        }
    }
    
    private void HandleAcceptButtonPress(ButtonPressController button, int buttonIndex)
    {
        if (button.isButtonPressed && !buttonHandled[buttonIndex])
        {
            buttonHandled[buttonIndex] = true;

            selectedCode = displayText.text;

            if (isAnimating)
            {
                StartCoroutine(ErrorMessage("Error"));
                return;
            }

            if (displayText.text == "1462")
            {
                StartCoroutine(SuccessMessage("Success"));
                return;
            }

            if (displayText.text.Length < MAX_CHARS)
            {
                StartCoroutine(ErrorMessage("Error"));
                return;
            }

            if (!isAnimating)
            {
                switch (displayText.text)
                {
                    case "11111":
                        StartCoroutine(SuccessMessage("Success"));
                        mainGame.isDayCompleted = true;
                        StartCoroutine(ShowInfoText());
                        break;
                    case "22222":
                        StartCoroutine(SuccessMessage("Success"));
                        mainGame.isDayCompleted = true;
                        StartCoroutine(ShowInfoText());
                        break;
                    default:
                        StartCoroutine(ErrorMessage("Error"));
                        break;
                }
            }
        }
        else if (!button.isButtonPressed)
        {
            buttonHandled[buttonIndex] = false;
        }
    }

    private IEnumerator SuccessMessage(string message)
    {
        isAnimating = false;
        StopCoroutine(AnimateText());

        displayText.text = "Loading...";

        yield return new WaitForSeconds(1f);
        
        displayText.text = "";

        yield return new WaitForSeconds(0.6f);
        
        displayText.text = message;

        yield return new WaitForSeconds(1f);
        
        displayText.text = "";

        yield return new WaitForSeconds(0.5f);

        displayText.text = message;

        yield return new WaitForSeconds(2f);

        isAnimating = true;
        StartCoroutine(AnimateText());
    }

    private IEnumerator ErrorMessage(string message)
    {
        isAnimating = false;
        StopCoroutine(AnimateText());

        displayText.text = message;

        yield return new WaitForSeconds(1f);
        
        displayText.text = "";

        yield return new WaitForSeconds(0.5f);
        
        displayText.text = message;

        yield return new WaitForSeconds(1f);
        
        displayText.text = "";

        isAnimating = true;
        StartCoroutine(AnimateText());
    }

    private void HandleCancelButtonPress(ButtonPressController button, int buttonIndex)
    {
        if (button.isButtonPressed && !buttonHandled[buttonIndex])
        {
            buttonHandled[buttonIndex] = true;

            if (!isAnimating)
            {
                // Останавливаем анимацию и очищаем текст при первом нажатии
                isAnimating = true;
                StartCoroutine(AnimateText());
                displayText.text = ""; // Очищаем текстовое поле
            }
        }
        else if (!button.isButtonPressed)
        {
            buttonHandled[buttonIndex] = false;
        }
    }

    private IEnumerator ShowInfoText()
    {
        string message1 = "Debug: На сегодня всё";
        string message2 = "Debug: Пора спать";
        string message3 = "Debug: Нужно переделать на звонок в релизе";

        yield return StartCoroutine(TypeText(message1, 0.05f));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText(message2, 0.05f));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText(message3, 0.05f));
        yield return new WaitForSeconds(2f);

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