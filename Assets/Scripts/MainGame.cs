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
}
