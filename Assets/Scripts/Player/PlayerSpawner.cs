using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform defaultPosition;
    [SerializeField] private Transform inSleepPosition;

    public void SpawnPlayerOnStartPosition()
    {
        player.transform.position = startPosition.position;
    }

    public void SpawnPlayerOnDefaultPosition()
    {
        player.transform.position = defaultPosition.position;
    }

    public void SpawnPlayerOnSleepPosition()
    {
        player.transform.position = inSleepPosition.position;
    }

    public void SpawnPlayerToCustomPosition(Vector3 position, Quaternion rotation)
    {
        player.transform.position = position;
        player.transform.rotation = rotation;
    }
}