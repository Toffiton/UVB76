using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaperController : MonoBehaviour
{
    [SerializeField] private bool isFaxPaper;
    [SerializeField] [CanBeNull] private PaperController paperOnTable;
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private GameObject defaultHand;
    [SerializeField] private GameObject handWithPaper;
    [SerializeField] private TextMeshProUGUI paperText;
    [SerializeField] private TextMeshProUGUI paperTextInHead;

    private PaperTypes _paperType;
    private Vector3 defaultPosition;

    public bool isTaked = false;

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
        switch (mainGame.GetCurrentDay())
        {
            case 1:
                _paperType = PaperTypes.FirstDay;
                break;
            case 2:
                _paperType = PaperTypes.SecondDay;
                break;
            case 3:
                _paperType = PaperTypes.ThirdDay;
                break;
            case 4:
                _paperType = PaperTypes.FourthDay;
                break;
            case 5:
                _paperType = PaperTypes.FifthDay;
                break;
        }

        defaultPosition = transform.position;

        if (isFaxPaper)
        {
            ShowPaper();
        }
        else
        {
            HidePaper();
        }

        SetTextInPaper();

        HidePaper();
    }

    private void TakePaper(InputAction.CallbackContext obj)
    {
        if (mainGame.isTakedPaper)
        {
            return;
        }

        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            SetTextInPaperHand();
            mainGame.isTakedPaper = true;
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
            if (isFaxPaper)
            {
                mainGame.isTakedPaperOnFax = true;
            }

            mainGame.ExecuteSetIsTakedPaperWithDelay(false);
            isTaked = false;
            handWithPaper.SetActive(false);
            defaultHand.SetActive(true);
            if (isFaxPaper)
            {
                StartCoroutine(DropItemWithDelay());
            }
            else
            {
                transform.position = defaultPosition;
            }
        }
    }

    private IEnumerator DropItemWithDelay()
    {
        HidePaper();

        yield return new WaitForSeconds(0.4F);
        mainGame.isTakedPaper = false;
        paperOnTable?.ShowPaper();
        Destroy(gameObject);
    }

    public void ShowPaper()
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
                paperText.text = "<size=0.015><b>День 1. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Николай Светлана\n2 — Ольга Анна\n3 — Роман Павел\n4 — Василий Дмитрий\n5 — Сергей Елена\n6 — Иван Мария\n7 — Ольга Тамара\n8 — Григорий Юлия\n9 — Александр Петя\n0 — Артём Анатолий</size></line-height>";
                break;
            case (int)PaperTypes.SecondDay:
                paperText.text = "<size=0.015><b>День 2. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Алексей Ирина\n2 — Борис Наталья\n3 — Виктор Ольга\n4 — Григорий Анна\n5 — Дмитрий Юлия\n6 — Евгений Елена\n7 — Иван Светлана\n8 — Константин Мария\n9 — Лев Анастасия\n0 — Михаил Татьяна</size></line-height>";
                break;
            case (int)PaperTypes.ThirdDay:
                paperText.text = "<size=0.015><b>День 3. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Павел Надежда\n2 — Роман Валерия\n3 — Станислав Наталья\n4 — Тимофей Елена\n5 — Фёдор Ольга\n6 — Юрий Анна\n7 — Артём Светлана\n8 — Владислав Мария\n9 — Георгий Татьяна\n0 — Василий Анастасия</size></line-height>";
                break;
            case (int)PaperTypes.FourthDay:
                paperText.text = "<size=0.015><b>День 4. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Алексей Оксана\n2 — Борис Виктория\n3 — Владимир Елена\n4 — Дмитрий Ирина\n5 — Евгений Татьяна\n6 — Игорь Наталья\n7 — Константин Светлана\n8 — Николай Марина\n9 — Олег Анна\n0 — Пётр Надежда</size></line-height>";
                break;
            case (int)PaperTypes.FifthDay:
                paperText.text = "<size=0.015><b>День 5. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Сергей Валерия\n2 — Тимофей Юлия\n3 — Фёдор Марина\n4 — Юрий Оксана\n5 — Артём Анастасия\n6 — Владислав Ирина\n7 — Георгий Виктория\n8 — Иван Елена\n9 — Константин Ольга\n0 — Лев Светлана</size></line-height>";
                break;
            case (int)PaperTypes.SixthDay:
                paperText.text = "<size=0.015><b>День 6. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Михаил Наталья\n2 — Николай Татьяна\n3 — Олег Анна\n4 — Павел Марина\n5 — Роман Оксана\n6 — Станислав Валерия\n7 — Тимофей Юлия\n8 — Фёдор Светлана\n9 — Юрий Виктория\n0 — Артём Ирина</size></line-height>";
                break;
        }
    }

    private void SetTextInPaperHand()
    {
        switch (_paperType)
        {
            case PaperTypes.FirstDay:
                paperTextInHead.text = "<size=0.015><b>День 1. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Николай Светлана\n2 — Ольга Анна\n3 — Роман Павел\n4 — Василий Дмитрий\n5 — Сергей Елена\n6 — Иван Мария\n7 — Ольга Тамара\n8 — Григорий Юлия\n9 — Александр Петя\n0 — Артём Анатолий</size></line-height>";
                break;
            case PaperTypes.SecondDay:
                paperTextInHead.text = "<size=0.015><b>День 2. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Алексей Ирина\n2 — Борис Наталья\n3 — Виктор Ольга\n4 — Григорий Анна\n5 — Дмитрий Юлия\n6 — Евгений Елена\n7 — Иван Светлана\n8 — Константин Мария\n9 — Лев Анастасия\n0 — Михаил Татьяна</size></line-height>";
                break;
            case PaperTypes.ThirdDay:
                paperTextInHead.text = "<size=0.015><b>День 3. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Павел Надежда\n2 — Роман Валерия\n3 — Станислав Наталья\n4 — Тимофей Елена\n5 — Фёдор Ольга\n6 — Юрий Анна\n7 — Артём Светлана\n8 — Владислав Мария\n9 — Георгий Татьяна\n0 — Василий Анастасия</size></line-height>";
                break;
            case PaperTypes.FourthDay:
                paperTextInHead.text = "<size=0.015><b>День 4. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Алексей Оксана\n2 — Борис Виктория\n3 — Владимир Елена\n4 — Дмитрий Ирина\n5 — Евгений Татьяна\n6 — Игорь Наталья\n7 — Константин Светлана\n8 — Николай Марина\n9 — Олег Анна\n0 — Пётр Надежда</size></line-height>";
                break;
            case PaperTypes.FifthDay:
                paperTextInHead.text = "<size=0.015><b>День 5. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Сергей Валерия\n2 — Тимофей Юлия\n3 — Фёдор Марина\n4 — Юрий Оксана\n5 — Артём Анастасия\n6 — Владислав Ирина\n7 — Георгий Виктория\n8 — Иван Елена\n9 — Константин Ольга\n0 — Лев Светлана</size></line-height>";
                break;
            case PaperTypes.SixthDay:
                paperTextInHead.text = "<size=0.015><b>День 6. Инструкция</b></size>\n<line-height=0.01><size=0.01>1 — Михаил Наталья\n2 — Николай Татьяна\n3 — Олег Анна\n4 — Павел Марина\n5 — Роман Оксана\n6 — Станислав Валерия\n7 — Тимофей Юлия\n8 — Фёдор Светлана\n9 — Юрий Виктория\n0 — Артём Ирина</size></line-height>";
                break;
        }
    }
}
