using UnityEngine;
using UnityEngine.InputSystem;

public class ChairController : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    [SerializeField] private TakedItem takedItem;
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Quaternion playerRotation;

    private bool isSiting = false;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Main.Enable();
        controls.Main.Interact.performed += SitOnChair;
        controls.Main.LKM.performed += SitOnChair;
        controls.Main.Exit.performed += GetUpFromChair;
        controls.Main.PKM.performed += GetUpFromChair;
    }

    private void OnDisable()
    {
        controls.Main.Disable();
        controls.Main.Interact.performed -= SitOnChair;
        controls.Main.LKM.performed -= SitOnChair;
        controls.Main.Exit.performed -= GetUpFromChair;
        controls.Main.PKM.performed -= GetUpFromChair;
    }

    private void SitOnChair(InputAction.CallbackContext obj)
    {
        if (takedItem.GetPlayerInRange() && takedItem.GetItemIsSelected())
        {
            mainGame.PlayPhoneSound();
            isSiting = true;
            player.SitOnChair(playerPosition, playerRotation);
        }
    }

    private void GetUpFromChair(InputAction.CallbackContext obj)
    {
        if (isSiting && !mainGame.isTakedItem)
        {
            player.GetUpFromChair();
            isSiting = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Проверяем, установлены ли позиции и ротация
        if (playerPosition != Vector3.zero && playerRotation != Quaternion.identity)
        {
            Gizmos.color = Color.red; // Цвет стрелки

            // Рисуем саму стрелку
            Vector3 arrowDirection = playerRotation * Vector3.forward; // Направление стрелки из ротации игрока
            Gizmos.DrawLine(playerPosition, playerPosition + arrowDirection * 2f); // Делаем длину стрелки 2 единицы

            // Рисуем наконечник стрелки
            Gizmos.DrawRay(playerPosition + arrowDirection * 2f, Quaternion.Euler(0, 45, 0) * arrowDirection * 0.5f);
            Gizmos.DrawRay(playerPosition + arrowDirection * 2f, Quaternion.Euler(0, -45, 0) * arrowDirection * 0.5f);
        }
    }
}
