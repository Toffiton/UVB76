using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 2.5f;

    private bool isClicked = false;
    
    private void Start()
    {
        StartCoroutine(Fade.FadeScreen(fadeImage, fadeDuration,1F, 0F));
        if (PlayerPrefs.GetInt("IsFirstLoadGame", 1) == 1)
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        if (isClicked)
        {
            return;
        }

        isClicked = true;
        StartCoroutine(StartGameExecute());
    }

    public void NewGame()
    {
        if (isClicked)
        {
            return;
        }

        isClicked = true;
        StartCoroutine(NewGameExecute());
    }

    public void QuitGame()
    {
        if (isClicked)
        {
            return;
        }

        isClicked = true;
        Application.Quit();
    }
    
    private IEnumerator StartGameExecute()
    {
        yield return StartCoroutine(Fade.FadeScreen(fadeImage, fadeDuration,0F, 1F));

        SceneManager.LoadScene(1);
        
        yield return null;
    }

    private IEnumerator NewGameExecute()
    {
        yield return StartCoroutine(Fade.FadeScreen(fadeImage, fadeDuration,0F, 1F));

        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
        yield return null;
    }
    //
    // private IEnumerator FadeScreen(float startAlpha, float endAlpha)
    // {
    //     if (startAlpha == 0F)
    //     {
    //         fadeImage.gameObject.SetActive(true);
    //     }
    //
    //     Color fadeColor = fadeImage.color;
    //     float elapsedTime = 0f;
    //
    //     while (elapsedTime < fadeDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
    //         fadeColor.a = alpha;
    //         fadeImage.color = fadeColor;
    //
    //         yield return null;
    //     }
    //
    //     fadeColor.a = endAlpha;
    //     fadeImage.color = fadeColor;
    //
    //     if (endAlpha == 0F)
    //     {
    //         fadeImage.gameObject.SetActive(false);
    //     }
    // }
}
