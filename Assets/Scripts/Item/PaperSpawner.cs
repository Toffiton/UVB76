using UnityEngine;

public class PaperSpawner : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private MainGame mainGame;

    void Start()
    {
        var paper = Instantiate(paperPrefab, transform.position, paperPrefab.transform.rotation);
        paper.GetComponent<PaperController>().isSpawnOnTable = true;
        switch (mainGame.GetCurrentDay())
        {
            case 0:
                paper.GetComponent<PaperController>().paperType = PaperTypes.FifthDay;
                break;
            case 1:
                paper.GetComponent<PaperController>().paperType = PaperTypes.SecondDay;
                break;
            case 2:
                paper.GetComponent<PaperController>().paperType = PaperTypes.ThirdDay;
                break;
            case 3:
                paper.GetComponent<PaperController>().paperType = PaperTypes.FourthDay;
                break;
            case 4:
                paper.GetComponent<PaperController>().paperType = PaperTypes.FifthDay;
                break;
        }
        
        paper.SetActive(true);
    }
}