using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade
{
    public static IEnumerator FadeScreen(Image fadeImage, float fadeDuration, float startAlpha, float endAlpha)
    {
        if (startAlpha == 0F)
        {
            fadeImage.gameObject.SetActive(true);
        }

        Color fadeColor = fadeImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeColor.a = alpha;
            fadeImage.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        fadeImage.color = fadeColor;

        if (endAlpha == 0F)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }
}
