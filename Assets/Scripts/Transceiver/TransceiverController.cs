using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
    [SerializeField] private int targetFrequency = 4625; // Целевая частота

    [SerializeField] private AudioSource speechSource;

    [SerializeField] private AudioLowPassFilter lowPassFilter;

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

        UpdateAudioEffects();
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
        frequency = (int)Mathf.Clamp(frequency + change, minFrequency, maxFrequency);
        frequency = Mathf.RoundToInt(frequency / (float)roundingStep) * roundingStep;

        UpdateFrequencyText();
    }

    private void ChangeFrequency(float change)
    {
        frequency = (int)Mathf.Clamp(frequency + change, minFrequency, maxFrequency);

        UpdateFrequencyText();
    }

    private void UpdateFrequencyText()
    {
        frequencyText.text = frequency.ToString();
    }

    private void UpdateAudioEffects()
    {
        // Расстояние до целевой частоты
        float distanceToTarget = Mathf.Abs(frequency - targetFrequency);

        // Нормализация расстояния для диапазона от 0 до 1
        float normalizedDistance = Mathf.Clamp01(distanceToTarget / (maxFrequency - minFrequency));

        // Настройка помех (низкочастотный фильтр для имитации шумов)
        lowPassFilter.cutoffFrequency = Mathf.Lerp(500, 22000, 1 - normalizedDistance);

        // Если частота в пределах заданного диапазона, воспроизводим речь
        if (distanceToTarget <= 100) // Задайте диапазон, например ±100 от цели
        {
            if (!speechSource.isPlaying)
            {
                speechSource.UnPause(); // Продолжаем воспроизведение, если оно было на паузе
            }
            speechSource.volume = Mathf.Lerp(0.5f, 1f, 1 - normalizedDistance); // Увеличиваем громкость речи
        }
        else
        {
            if (speechSource.isPlaying)
            {
                speechSource.Pause(); // Ставим на паузу, если за пределами диапазона
            }
        }
    }
}