using UnityEngine;

public class PhonePrefs
{
    public static void SetPhoneCallIsListenById(int id)
    {
        PlayerPrefs.SetInt("PhoneCall_" + id, 1);
    }

    public static bool GetPhoneCallIsListenById(int id)
    {
        return PlayerPrefs.GetInt("PhoneCall_" + id, 0) == 1;
    }
}