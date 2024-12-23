using System.Collections;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public bool isTakedItem;

    public IEnumerator SetIsTakedItemWithDelay(bool isTakedItem)
    {
        yield return new WaitForSeconds(0.1f);
        this.isTakedItem = isTakedItem;
    }

    public int GetCurrentDay()
    {
        return PlayerPrefs.GetInt("CurrentDay", 0);
    }

    public void SetCurrentDay(int day)
    {
        PlayerPrefs.SetInt("CurrentDay", day);
    }
}