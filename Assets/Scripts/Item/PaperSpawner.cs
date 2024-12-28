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
            case 1:
                paper.GetComponent<PaperController>().paperType = PaperTypes.FirstDay;
                break;
            case 2:
                paper.GetComponent<PaperController>().paperType = PaperTypes.SecondDay;
                break;
            case 3:
                paper.GetComponent<PaperController>().paperType = PaperTypes.ThirdDay;
                break;
            case 4:
                paper.GetComponent<PaperController>().paperType = PaperTypes.FourthDay;
                break;
            case 5:
                paper.GetComponent<PaperController>().paperType = PaperTypes.FifthDay;
                break;
        }
        
        paper.SetActive(true);
    }
}