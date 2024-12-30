using TMPro;
using UnityEngine;

public class TransceiverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI frequencyText;

    [SerializeField] private ButtonPressController leftButton;
    [SerializeField] private ButtonPressController rightButton;

    [SerializeField] private int frequencyStep = 125; // Шаг изменения частоты
    [SerializeField] private float holdChangeSpeed = 0.5f; // Скорость изменения частоты при удержании
    [SerializeField] private int minFrequency = 3000; // Минимальная частота
    [SerializeField] private int maxFrequency = 5000; // Максимальная частота

    public int frequency = 4000;

    private float holdTimerLeft = 0f; // Таймер удержания левой кнопки
    private float holdTimerRight = 0f; // Таймер удержания правой кнопки

    private const float holdThreshold = 0.2f; // Порог времени для определения удержания

    void Update()
    {
        // Обновляем состояние кнопок
        leftButton.isCanButtonPressed = !rightButton.isButtonPressed;
        rightButton.isCanButtonPressed = !leftButton.isButtonPressed;

        // Обработка левой кнопки
        HandleButtonPress(-1, leftButton, ref holdTimerLeft);

        // Обработка правой кнопки
        HandleButtonPress(1, rightButton, ref holdTimerRight);
    }

    private void HandleButtonPress(int direction, ButtonPressController button, ref float holdTimer)
    {
        if (button.isButtonPressed)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer < holdThreshold) // Одиночное нажатие
            {
                if (Mathf.Approximately(holdTimer, Time.deltaTime)) // Выполняем только один раз
                {
                    ChangeFrequency(direction * frequencyStep, transform);
                }
            }
            else // Удержание: плавное изменение
            {
                ChangeFrequency(direction * frequencyStep, transform);
            }
        }
        else
        {
            holdTimer = 0f; // Сброс таймера при отпускании кнопки
        }
    }

    private void ChangeFrequency(float change, bool isRound)
    {
        // Меняем частоту с учётом границ
        frequency = (int)Mathf.Clamp(frequency + change, minFrequency, maxFrequency);
        if (isRound)
        {
            // Округляем частоту к кратному шагу
            frequency = (int)Mathf.Round(frequency / frequencyStep) * frequencyStep;
        }
        // Обновляем текст
        UpdateFrequencyText();
    }

    private void UpdateFrequencyText()
    {
        // Преобразуем частоту в строку формата "XXX.XXX"
        frequencyText.text = frequency.ToString();
    }
}
