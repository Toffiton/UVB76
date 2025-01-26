using System;
using TMPro;
using UnityEngine;

/** Удалить */
public class HelperController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI helperText;
    private int currentStep = 0;
    
    private void Start()
    {
        currentStep = HelperPrefs.GetHelperStep();

        switch (currentStep)
        {
            case 0:
                helperText.text = "Осмотреться";
                break;
        }
    }

    private void Update()
    {
        
    }
}
