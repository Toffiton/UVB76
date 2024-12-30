using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Камера игрока
    [SerializeField] private float maxDistance = 10f; // Максимальная дистанция наведения
    [SerializeField] private LayerMask highlightLayer; // Слой для подсвечиваемых объектов

    private Outline currentOutline; // Текущий подсвечиваемый объект
    private TakedItem takedItem; // Текущий подсвечиваемый объект

    private void Update()
    {
        HandleHighlighting();
    }

    private void HandleHighlighting()
    {
        // Запускаем рейкаст из центра экрана
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Проверяем, попадает ли луч в объект
        if (Physics.Raycast(ray, out hit, maxDistance, highlightLayer, QueryTriggerInteraction.Ignore))
        {
            // Пытаемся получить компонент Outline у объекта
            Outline outline = hit.collider.GetComponent<Outline>();
            TakedItem takedItem = hit.collider.GetComponent<TakedItem>();

            if (outline != null)
            {
                if (takedItem == null)
                {
                    throw new System.NotImplementedException("TakedItem is null, add TakedItem to the gameobject");
                }

                // Если это новый объект, на который мы навелись
                if (currentOutline != outline)
                {
                    if (takedItem.GetPlayerInRange())
                    {
                        takedItem.SetItemIsSelected(true);

                        RemoveHighlight();
                        RemoveTakedItemSelector();

                        ApplyHighlight(outline); // Подсвечиваем новый объект
                        ApplyTakedItem(takedItem);
                    }
                    else
                    {
                        takedItem.SetItemIsSelected(false);
                        RemoveHighlight(); // Убираем подсветку с предыдущего объекта
                        RemoveTakedItemSelector();
                    }
                }

                return; // Выходим, так как объект найден
            }
        }

        // Если луч не попадает в подсвечиваемый объект
        RemoveHighlight();
        RemoveTakedItemSelector();
    }

    private void ApplyHighlight(Outline outline)
    {
        currentOutline = outline;
        currentOutline.ShowOutline(); // Включаем обводку
    }

    private void ApplyTakedItem(TakedItem takedItem)
    {
        this.takedItem = takedItem;
        takedItem.SetItemIsSelected(true);
    }

    private void RemoveHighlight()
    {
        if (currentOutline != null)
        {
            currentOutline.HideOutline(); // Выключаем обводку
            currentOutline = null; // Сбрасываем ссылку
        }
    }

    private void RemoveTakedItemSelector()
    {
        if (takedItem != null)
        {
            takedItem.SetItemIsSelected(false);
            takedItem = null; // Сбрасываем ссылку
        }
    }
}