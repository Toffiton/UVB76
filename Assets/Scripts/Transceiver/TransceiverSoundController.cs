using UnityEngine;

public class TransceiverSoundController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TransceiverController transceiverController;

    [Space]
    [Header("День 1")]
    [SerializeField] private AudioSource mainSource;
    [SerializeField] private AudioClip soundFirstDay;
    [SerializeField] private int mainRange = 100;

    [Space]
    [Header("Побочные переговоры радиолюбителей 3000 кгц")]
    [SerializeField] private AudioSource firstSideSource;
    [SerializeField] private AudioClip firstSideAudio;
    [SerializeField] private int firstSideRange = 100;

    [Space]
    [Header("Побочные переговоры радиолюбителей 5000 кгц")]
    [SerializeField] private AudioSource secondSideSource;
    [SerializeField] private AudioClip secondSideAudio;
    [SerializeField] private int secondSideRange = 100;

    [Space]
    [SerializeField] private AudioLowPassFilter lowPassFilter;

    private void Start()
    {
        // Запускаем все треки сразу
        PlayLoopingAudio(mainSource, soundFirstDay);
        PlayLoopingAudio(firstSideSource, firstSideAudio);
        PlayLoopingAudio(secondSideSource, secondSideAudio);
    }

    private void Update()
    {
        if (mainGame.isPhoneCallEnded)
        {
            if (!mainGame.isDayCompleted)
            {
                AdjustAudioVolume(mainSource, transceiverController.targetFrequency, mainRange);
            }

            AdjustAudioVolume(firstSideSource, transceiverController.firstSideTargetFrequency, firstSideRange);
            AdjustAudioVolume(secondSideSource, transceiverController.secondSideTargetFrequency, secondSideRange);
        }
        else
        {
            // Останавливаем звуки, если звонок не завершён
            StopAllAudio();
        }
    }

    private void PlayLoopingAudio(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.loop = true; // Делаем бесконечный цикл
        source.Play();
        source.volume = 0; // Начинаем с нулевой громкости
    }

    private void AdjustAudioVolume(AudioSource source, int targetFrequency, int range)
    {
        float distanceToTarget = Mathf.Abs(transceiverController.frequency - targetFrequency);
        float normalizedDistance = Mathf.Clamp01(distanceToTarget / range);

        if (distanceToTarget <= range)
        {
            source.volume = Mathf.Lerp(0.1f, 1f, 1 - normalizedDistance);
        }
        else
        {
            source.volume = 0; // Заглушаем звук, если далеко от частоты
        }
    }

    private void StopAllAudio()
    {
        mainSource.Stop();
        firstSideSource.Stop();
        secondSideSource.Stop();
    }
}
