using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RadioSignalEmulator : MonoBehaviour
{
    [SerializeField] private RawImage signalDisplay; // UI элемент для отображения
    [SerializeField] private TransceiverController transceiver; // Контроллер частоты
    [SerializeField] private int width = 512;       // Ширина текстуры
    [SerializeField] private int height = 256;      // Высота текстуры
    [SerializeField] private float noiseLevel = 0.3f; // Уровень шума
    [SerializeField] private float updateInterval = 0.05f; // Скорость обновления
    [SerializeField] private int spectrumMinFrequency = 4500; // Минимальная частота спектра
    [SerializeField] private int spectrumMaxFrequency = 4750; // Максимальная частота спектра
    [SerializeField] private float signalWidth = 10f; // Ширина сигнала в пикселях

    private Texture2D signalTexture;
    private Color[] pixels;
    private float speechTimer = 0f; // Таймер для модуляции сигнала

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
        ClearTexture();
    }

    private void Start()
    {
        // Создаем текстуру
        signalTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        signalTexture.filterMode = FilterMode.Point;
        signalTexture.wrapMode = TextureWrapMode.Clamp;

        // Инициализируем массив пикселей
        pixels = new Color[width * height];
        ClearTexture();

        // Присваиваем текстуру RawImage
        signalDisplay.texture = signalTexture;

        // Запускаем обновление сигнала
        InvokeRepeating(nameof(UpdateSignal), 0f, updateInterval);
    }

    private void ClearTexture()
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black; // Чёрный фон
        }
    }

    private void UpdateSignal()
    {
        int frequency = transceiver.frequency; // Получаем текущую частоту

        // Сдвигаем строки вверх
        for (int y = height - 1; y > 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[y * width + x] = pixels[(y - 1) * width + x];
            }
        }

        // Генерируем новый ряд сигналов
        for (int x = 0; x < width; x++)
        {
            float intensity = Random.Range(0f, noiseLevel); // Шум

            // Вычисляем соответствующую частоту для текущей позиции X
            float mappedFrequency = Mathf.Lerp(spectrumMinFrequency, spectrumMaxFrequency, (float)x / width);

            // Если частота находится рядом с текущей частотой трансивера, усиливаем сигнал
            float distanceToSignal = Mathf.Abs(mappedFrequency - frequency);
            if (distanceToSignal < signalWidth)
            {
                float signalStrength = Mathf.Lerp(1f, 0f, distanceToSignal / signalWidth);
                intensity = Mathf.Lerp(intensity, signalStrength, 0.8f);

                // Добавляем синусоидальную модуляцию для "речи" в центре сигнала
                if (distanceToSignal < signalWidth / 3f)
                {
                    speechTimer += Time.deltaTime * 5;
                    float speechIntensity = Mathf.Sin(speechTimer + x * 0.1f) * 0.5f + 0.5f;
                    intensity = Mathf.Lerp(intensity, speechIntensity, 0.8f);
                }
            }

            pixels[x] = new Color(intensity, intensity, intensity);
        }

        // Обновляем текстуру
        signalTexture.SetPixels(pixels);
        signalTexture.Apply();
    }
}