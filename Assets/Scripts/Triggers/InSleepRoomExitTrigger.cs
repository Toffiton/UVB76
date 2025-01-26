using UnityEngine;

public class InSleepRoomExitTrigger : MonoBehaviour
{
    [SerializeField] private BedController bedController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(bedController.ExitSleepLevelTransition());
        }
    }
}