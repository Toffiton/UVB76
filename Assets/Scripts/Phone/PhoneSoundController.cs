using UnityEngine;
using System.Collections;

public class PhoneSoundController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TransceiverController transceiverController;
    [Space]
    [Header("День 1")]
    [SerializeField] private AudioClip step1;
    [SerializeField] private AudioClip step2;
    [SerializeField] private AudioClip step3;

    [SerializeField] private AudioSource speechSource;

    private int currentStep = 0;

    private void Start()
    {
        StartCoroutine(StepSequence());
    }

    private void Update()
    {
        UpdateAudioEffects();
    }

    private IEnumerator StepSequence()
    {
        // Шаг 1: Воспроизводим step1
        yield return PlayClip(step1);

        // Шаг 2: Воспроизводим step2
        yield return PlayClip(step2);

        // Шаг 3: Ждём, пока частота будет в рамках и удерживается 5 секунд
        yield return WaitForFrequencyInRange(5f);

        // Воспроизводим step3
        yield return PlayClip(step3);

        // Далее можно добавлять другие шаги
    }

    private IEnumerator PlayClip(AudioClip clip)
    {
        speechSource.clip = clip;
        speechSource.Play();

        while (speechSource.isPlaying)
        {
            yield return null; // Ждём завершения клипа
        }
    }

    private IEnumerator WaitForFrequencyInRange(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            float frequency = transceiverController.frequency;
            if (frequency >= transceiverController.minFrequency && frequency <= transceiverController.maxFrequency)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0; // Сброс таймера, если вышли из диапазона
            }

            yield return null; // Ждём следующий кадр
        }
    }

    private void UpdateAudioEffects()
    {
        float distanceToTarget = Mathf.Abs(transceiverController.frequency - transceiverController.targetFrequency);
        float normalizedDistance = Mathf.Clamp01(distanceToTarget / (transceiverController.maxFrequency - transceiverController.minFrequency));
        lowPassFilter.cutoffFrequency = Mathf.Lerp(500, 22000, 1 - normalizedDistance);

        if (distanceToTarget <= 100)
        {
            if (!speechSource.isPlaying)
            {
                speechSource.UnPause();
            }
            speechSource.volume = Mathf.Lerp(0.5f, 1f, 1 - normalizedDistance);
        }
        else
        {
            if (speechSource.isPlaying)
            {
                speechSource.Pause();
            }
        }
    }
}
