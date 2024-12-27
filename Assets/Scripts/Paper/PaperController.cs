using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaperController : MonoBehaviour
{
    [SerializeField] public PaperTypes paperType;
    /**
     * Заспавнен объект на доске или на столе
     *
     * 1) true - На столе, дестроим объект после дропа
     * 2) false - На доске, прячем объект при взятии, показываем при дропе
     */
    [SerializeField] public bool isSpawnOnTable = false;
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private GameObject defaultHand;
    [SerializeField] private GameObject handWithPaper;
    [SerializeField] private TextMeshProUGUI paperText;
    [SerializeField] private TextMeshProUGUI paperTextInHead;
    
    private Vector3 defaultPosition;

    private bool isTaked = false;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Interact.performed += TakePaper;
        controls.Main.LKM.performed += TakePaper;
        controls.Main.Exit.performed += DropPaper;
        controls.Main.PKM.performed += DropPaper;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= TakePaper;
        controls.Main.LKM.performed -= TakePaper;
        controls.Main.Exit.performed -= DropPaper;
        controls.Main.PKM.performed -= DropPaper;
    }

    private void Start()
    {
        defaultPosition = transform.position;

        if (!isSpawnOnTable)
        {
            if (mainGame.GetCurrentDay() >= (int)paperType)
            {
                ShowPaper();
            }
            else
            {
                HidePaper();
            }
        }

        SetTextInPaper();
    }

    private void TakePaper(InputAction.CallbackContext obj)
    {
        if (mainGame.isTakedItem)
        {
            return;
        }

        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            SetTextInPaperHand();
            mainGame.isTakedItem = true;
            isTaked = true;
            defaultHand.SetActive(false);
            handWithPaper.SetActive(true);
            transform.transform.position = new Vector3(0, 0, 0);
        }
    }

    private void DropPaper(InputAction.CallbackContext obj)
    {
        if (isTaked)
        {
            StartCoroutine(mainGame.SetIsTakedItemWithDelay(false));
            isTaked = false;
            handWithPaper.SetActive(false);
            defaultHand.SetActive(true);
            if (isSpawnOnTable)
            {
                mainGame.isTakedItem = false;
                Destroy(gameObject);
            }
            else
            {
                transform.position = defaultPosition;
            }
        }
    }

    private void ShowPaper()
    {
        transform.position = defaultPosition;
    }

    private void HidePaper()
    {
        transform.transform.position = new Vector3(0, 0, 0);
    }

    private void SetTextInPaper()
    {
        switch (mainGame.GetCurrentDay())
        {
            case (int)PaperTypes.FirstDay:
                paperText.text = "<size=0.015><b>День 1</b></size>\n<line-height=0.01><size=0.01>Странное пробуждение... И почему Бобер не курва? Задай этот вопрос пустому небу.</size></line-height>";
                break;
            case (int)PaperTypes.SecondDay:
                paperText.text = "<size=0.015><b>День 2</b></size>\n<line-height=0.01><size=0.01>В лесу шепчут древние существа. Это Бобер или Бобер — это всего лишь его маска?</size></line-height>";
                break;
            case (int)PaperTypes.ThirdDay:
                paperText.text = "<size=0.015><b>День 3</b></size>\n<line-height=0.01><size=0.01>Темные воды бурлят. И снова этот вопрос: Бобер курва? Но кто ты, чтобы задавать такие вопросы?</size></line-height>";
                break;
            case (int)PaperTypes.FourthDay:
                paperText.text = "<size=0.015><b>День 4</b></size>\n<line-height=0.01><size=0.01>Горы пылают, и в их тени Бобер стоит. Не курва ли он, или же он больше, чем мы можем понять?</size></line-height>";
                break;
            case (int)PaperTypes.FifthDay:
                paperText.text = "<size=0.015><b>День 5</b></size>\n<line-height=0.01><size=0.01>Ветер принесёт ответы, или его унесет вон тот Бобер? Его лицо исчезает в тумане, оставляя лишь его след.</size></line-height>";
                break;
            case (int)PaperTypes.SixthDay:
                paperText.text = "<size=0.015><b>День 6</b></size>\n<line-height=0.01><size=0.01>Кажется, мы не одни. Вдалеке — громкий смех. Или это просто Бобер, и он снова курва?</size></line-height>";
                break;
        }
    }

    private void SetTextInPaperHand()
    {
        switch (paperType)
        {
            case PaperTypes.FirstDay:
                paperTextInHead.text = "<size=0.015><b>День 1</b></size>\n<line-height=0.01><size=0.01>Странное пробуждение... И почему Бобер не курва? Задай этот вопрос пустому небу.</size></line-height>";
                break;
            case PaperTypes.SecondDay:
                paperTextInHead.text = "<size=0.015><b>День 2</b></size>\n<line-height=0.01><size=0.01>В лесу шепчут древние существа. Это Бобер или Бобер — это всего лишь его маска?</size></line-height>";
                break;
            case PaperTypes.ThirdDay:
                paperTextInHead.text = "<size=0.015><b>День 3</b></size>\n<line-height=0.01><size=0.01>Темные воды бурлят. И снова этот вопрос: Бобер курва? Но кто ты, чтобы задавать такие вопросы?</size></line-height>";
                break;
            case PaperTypes.FourthDay:
                paperTextInHead.text = "<size=0.015><b>День 4</b></size>\n<line-height=0.01><size=0.01>Горы пылают, и в их тени Бобер стоит. Не курва ли он, или же он больше, чем мы можем понять?</size></line-height>";
                break;
            case PaperTypes.FifthDay:
                paperTextInHead.text = "<size=0.015><b>День 5</b></size>\n<line-height=0.01><size=0.01>Ветер принесёт ответы, или его унесет вон тот Бобер? Его лицо исчезает в тумане, оставляя лишь его след.</size></line-height>";
                break;
            case PaperTypes.SixthDay:
                paperTextInHead.text = "<size=0.015><b>День 6</b></size>\n<line-height=0.01><size=0.01>Кажется, мы не одни. Вдалеке — громкий смех. Или это просто Бобер, и он снова курва?</size></line-height>";
                break;
        }
    }
}
