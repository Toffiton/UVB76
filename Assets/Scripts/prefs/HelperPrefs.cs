using UnityEngine;

/** Удалить */
public class HelperPrefs
{
    public static void SetHelperStep()
    {
        PlayerPrefs.SetInt("HelperStep", 0);
    }

    public static int GetHelperStep()
    {
        return PlayerPrefs.GetInt("HelperStep", 0);
    }
}