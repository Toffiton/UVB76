using TMPro;
using UnityEngine;

public class TransceiverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI frequencyText;

    [SerializeField] private ButtonPressController leftButton;
    [SerializeField] private ButtonPressController rightButton;
    [SerializeField] private ButtonPressController leftStepButton;
    [SerializeField] private ButtonPressController rightStepButton;

    [SerializeField] private int roundingStep = 250; // Шаг округления
    [SerializeField] private int frequencyStep = 125; // Базовый шаг изменения частоты
    [SerializeField] private float initialChangeSpeed = 0.5f; // Начальная скорость изменения частоты
    [SerializeField] private float acceleration = 0.1f; // Ускорение изменения частоты при удержании
    [SerializeField] private int minFrequency = 3000; // Минимальная частота
    [SerializeField] private int maxFrequency = 5000; // Максимальная частота

    public int frequency = 4000;

    private float holdTimeLeft = 0;
    private float holdTimeRight = 0;

    private bool leftStepHandled = false;
    private bool rightStepHandled = false;

    
    void Update()
    {
        HandleButtonPress(-1, leftButton, ref holdTimeLeft);
        HandleButtonPress(1, rightButton, ref holdTimeRight);

        HandleStepButtonPress(-1, leftStepButton, ref leftStepHandled);
        HandleStepButtonPress(1, rightStepButton, ref rightStepHandled);
    }

    private void HandleButtonPress(int direction, ButtonPressController button, ref float holdTime)
    {
        if (button.isButtonPressed)
        {
            holdTime += Time.deltaTime;

            float changeSpeed = initialChangeSpeed + acceleration * holdTime;

            ChangeFrequency(direction * frequencyStep * changeSpeed * Time.deltaTime);
        }
        else
        {
            holdTime = 0;
        }
    }

    private void HandleStepButtonPress(int direction, ButtonPressController button, ref bool stepHandled)
    {
        if (button.isButtonPressed && !stepHandled)
        {
            ChangeFrequencyWithRounding(direction * roundingStep);
            stepHandled = true;
        }
        else if (!button.isButtonPressed)
        {
            stepHandled = false;
        }
    }

    private void ChangeFrequencyWithRounding(float change)
    {
        // Меняем частоту и округляем до ближайшего значения, кратного roundingStep
        frequency = (int)Mathf.Clamp(frequency + change, minFrequency, maxFrequency);
        frequency = Mathf.RoundToInt(frequency / (float)roundingStep) * roundingStep;

        // Обновляем текст
        UpdateFrequencyText();
    }

    private void ChangeFrequency(float change)
    {
        // Меняем частоту с учётом границ
        frequency = (int)Mathf.Clamp(frequency + change, minFrequency, maxFrequency);

        // Обновляем текст
        UpdateFrequencyText();
    }

    private void UpdateFrequencyText()
    {
        // Преобразуем частоту в строку формата "XXXX"
        frequencyText.text = frequency.ToString();
    }
}
