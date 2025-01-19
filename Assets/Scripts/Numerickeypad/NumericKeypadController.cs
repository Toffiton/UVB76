using System.Collections;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class NumericKeypadController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;

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
                        Debug.Log("value is true: 11111");
                        break;
                    case "22222":
                        StartCoroutine(SuccessMessage("Success"));
                        Debug.Log("value is true: 22222");
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
}