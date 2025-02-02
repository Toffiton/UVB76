using UnityEngine;
using System.Collections;

public class PhoneSoundController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TransceiverController transceiverController;
    [SerializeField] private FaxController faxController;
    [SerializeField] private NumericKeypadController numericKeypadController;
    [Header("Бумага в факсе")]
    [SerializeField] private PaperController paper;
    [Header("Бумага на столе")]
    [SerializeField] private PaperController paperOnTable;

    [Space]
    [Header("День 1")]
    [SerializeField] private AudioClip step1;
    [SerializeField] private AudioClip step2;
    [SerializeField] private AudioClip step3;
    [SerializeField] private AudioClip step4;
    [SerializeField] private AudioClip step5;
    [SerializeField] private AudioClip step6;
    
    [Space]
    [Header("День 2")]
    [SerializeField] private AudioClip step21;

    [SerializeField] private AudioSource speechSource;
    
    private Coroutine waitForFrequencyCoroutine;

    private int currentStep = 0;

    public void StartCall()
    {
        switch (mainGame.GetCurrentDay())
        {
            case 1:
                StartCoroutine(StartTutorialCall());
                break;
            case 2:
                StartCoroutine(StartSecondDayCall());
                break;
        }
    }

    private IEnumerator StartTutorialCall()
    {
        mainGame.isPhoneCallStarted = true;
        yield return PlayClip(step1);

        yield return PlayClip(step2);

        // Шаг 3: Ждём, пока частота будет в рамках и удерживается 3 секунд
        yield return WaitForFrequencyInRange(3f);

        yield return PlayClip(step3);

        yield return new WaitForSeconds(3F);

        faxController.PrintPaper();

        // Шаг 4: Ждём, когда игрок возьмёт бумагу из факса в руку
        yield return WaitForTakePaper(3f);

        yield return PlayClip(step4);

        // Шаг 4: Ждём, когда игрок введёт код 1462
        yield return WaitForInputCode(3f);

        yield return PlayClip(step5);

        yield return PlayClip(step6);

        yield return WaitForInputCode(0.6f);

        PhonePrefs.SetPhoneCallIsListenById(mainGame.GetCurrentDay());
        mainGame.isPhoneCallStarted = false;
        mainGame.isPhoneCallEnded = true;
    }

    private IEnumerator StartSecondDayCall()
    {
        mainGame.isPhoneCallStarted = true;

        yield return PlayClip(step21);

        PhonePrefs.SetPhoneCallIsListenById(mainGame.GetCurrentDay());
        mainGame.isPhoneCallStarted = false;
        mainGame.isPhoneCallEnded = true;
    }

    public void CancelCall()
    {
        if (speechSource.isPlaying)
        {
            speechSource.Stop();
        }
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
            if (frequency >= 4555F && frequency <= 4700F)
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

    private IEnumerator WaitForTakePaper(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            if (paper.isTaked || paperOnTable.isTaked)
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

    private IEnumerator WaitForInputCode(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            if (numericKeypadController.selectedCode == "1462")
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
