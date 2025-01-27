using UnityEngine;
using System.Collections;

public class TransceiverSoundController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TransceiverController transceiverController;
    [Space]
    [Header("Фон")]
    [SerializeField] private AudioClip radioBackgroundSound;

    [Space]
    [Header("День 1")]
    [SerializeField] private AudioClip soundFirstDay;

    [SerializeField] private AudioSource speechSource;
    [SerializeField] private AudioLowPassFilter lowPassFilter;

    private void Start()
    {
        speechSource.clip = soundFirstDay;
    }
    
    private void Update()
    {
        if (mainGame.isPhoneCallEnded && !mainGame.isDayCompleted)
        {
            UpdateAudioEffects();
        }
        else
        {
            if (speechSource.isPlaying)
            {
                speechSource.Stop();
            }
        }
    }

    private void UpdateAudioEffects()
    {
        // Расстояние до целевой частоты
        float distanceToTarget = Mathf.Abs(transceiverController.frequency - transceiverController.targetFrequency);

        // Нормализация расстояния для диапазона от 0 до 1
        float normalizedDistance = Mathf.Clamp01(distanceToTarget / (transceiverController.maxFrequency - transceiverController.minFrequency));

        // Настройка помех (низкочастотный фильтр для имитации шумов)
        lowPassFilter.cutoffFrequency = Mathf.Lerp(500, 22000, 1 - normalizedDistance);

        // Если частота в пределах заданного диапазона, воспроизводим речь
        if (distanceToTarget <= 100) // Задайте диапазон, например ±100 от цели
        {
            if (!speechSource.isPlaying)
            {
                speechSource.Play(); // Продолжаем воспроизведение, если оно было на паузе
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
