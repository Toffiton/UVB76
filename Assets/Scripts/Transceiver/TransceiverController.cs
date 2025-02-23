using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TransceiverController : MonoBehaviour
{
    [SerializeField] [CanBeNull] private MainGame mainGame;
    [SerializeField] private TextMeshProUGUI frequencyText;

    [SerializeField] private ButtonPressController leftButton;
    [SerializeField] private ButtonPressController rightButton;
    [SerializeField] private ButtonPressController leftStepButton;
    [SerializeField] private ButtonPressController rightStepButton;

    [SerializeField] private int roundingStep = 250; // Шаг округления
    [SerializeField] private int frequencyStep = 125; // Базовый шаг изменения частоты
    [SerializeField] private float initialChangeSpeed = 0.5f; // Начальная скорость изменения частоты
    [SerializeField] private float acceleration = 0.1f; // Ускорение изменения частоты при удержании
    [SerializeField] public int minFrequency = 2000; // Минимальная частота
    [SerializeField] public int maxFrequency = 6000; // Максимальная частота
    [SerializeField] public int targetFrequency = 4625; // Целевая частота
    [SerializeField] public int firstSideTargetFrequency = 3000; // Целевая частота
    [SerializeField] public int secondSideTargetFrequency = 5000; // Целевая частота

    public int frequency = 4000;

    private bool leftStepHandled = false;
    private bool rightStepHandled = false;

    private void OnEnable()
    {
        MainGame.OnDayChanged += HandleDayChanged;
    }

    private void OnDisable()
    {
        MainGame.OnDayChanged -= HandleDayChanged;
    }

    private void HandleDayChanged(int newDay)
    {
        frequency = 4000;
        UpdateFrequencyText();
    }


    private void Start()
    {
        if (mainGame == null)
        {
            return;
        }

        switch (mainGame?.GetCurrentDay())
        {
            case 2: frequency = 6000; 
                break;
            case 3: frequency = 2000; 
                break;
        }
    }

    void Update()
    {
        HandleButtonPress(-1, leftButton);
        HandleButtonPress(1, rightButton);

        HandleStepButtonPress(-1, leftStepButton, ref leftStepHandled);
        HandleStepButtonPress(1, rightStepButton, ref rightStepHandled);
    }

    private void HandleButtonPress(int direction, ButtonPressController button)
    {
        if (button.isButtonPressed)
        {
            float changeSpeed = initialChangeSpeed + acceleration * 10;

            ChangeFrequency(direction * frequencyStep * changeSpeed * Time.deltaTime);
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
        frequencyText.text = frequency + " МГц";
    }
}