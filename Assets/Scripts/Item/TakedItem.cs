using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(SphereCollider))]
public class TakedItem : MonoBehaviour
{
    /** Находится ли персонаж в области взаимодействия с предметом */
    private bool playerInRange = false;
    /** Наведен ли курсор на предмет */
    private bool itemIsSelected = false;

    public bool GetPlayerInRange()
    {
        return playerInRange;
    }

    public void SetItemIsSelected(bool itemIsSelected)
    {
        this.itemIsSelected = itemIsSelected;
    }

    public bool GetItemIsSelected()
    {
        return itemIsSelected;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
