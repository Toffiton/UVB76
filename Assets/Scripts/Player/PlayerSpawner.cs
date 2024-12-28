using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform defaultPosition;

    public void SpawnPlayerOnStartPosition()
    {
        player.transform.position = startPosition.position;
    }

    public void SpawnPlayerOnDefaultPosition()
    {
        player.transform.position = defaultPosition.position;
    }
}
