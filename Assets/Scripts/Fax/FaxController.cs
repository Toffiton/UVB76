using UnityEngine;
using System.Collections;

public class FaxController : MonoBehaviour
{
    [SerializeField] private PaperController paper;
    [SerializeField] private Transform startPoint; // Начальная точка
    [SerializeField] private Transform endPoint;   // Конечная точка
    [SerializeField] private float moveDuration = 2f; // Время движения в секундах

    private bool isPrintable = false;

    public void PrintPaper()
    {
        if (!isPrintable)
        {
            isPrintable = true;
            paper.canTakedPaper = false;
            paper.ShowPaper();
            StartCoroutine(MovePaper());
        }
    }

    private IEnumerator MovePaper()
    {
        float elapsedTime = 0f; // Время, прошедшее с начала движения

        // Устанавливаем начальную позицию бумаги
        paper.transform.position = startPoint.position;

        while (elapsedTime < moveDuration)
        {
            // Считаем интерполяцию от 0 до 1
            float t = elapsedTime / moveDuration;

            // Плавно перемещаем объект
            paper.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);

            // Увеличиваем прошедшее время
            elapsedTime += Time.deltaTime;

            yield return null; // Ждём следующий кадр
        }

        // Устанавливаем конечную позицию точно, чтобы избежать погрешностей
        paper.transform.position = endPoint.position;
        isPrintable = false;
        
        yield return new WaitForSeconds(1f);
        paper.canTakedPaper = true;
    }
}