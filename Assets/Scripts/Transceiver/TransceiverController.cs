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

    void Update()
    {
        // Обновляем состояние кнопок
        leftButton.isCanButtonPressed = !rightButton.isButtonPressed;
        rightButton.isCanButtonPressed = !leftButton.isButtonPressed;

        // Обработка левой кнопки
        HandleButtonPress(-1, leftButton);

        // Обработка правой кнопки
        HandleButtonPress(1, rightButton);
    }

    private void HandleButtonPress(int direction, ButtonPressController button)
    {
        if (button.isButtonPressed)
        {
            ChangeFrequency(direction * frequencyStep, transform);
        }
    }

    private void ChangeFrequency(float change, bool isRound)
    {
        // Меняем частоту с учётом границ
        frequency = (int)Mathf.Clamp(frequency + change, minFrequency, maxFrequency);
        // Обновляем текст
        UpdateFrequencyText();
    }

    private void UpdateFrequencyText()
    {
        // Преобразуем частоту в строку формата "XXX.XXX"
        frequencyText.text = frequency.ToString();
    }
}
