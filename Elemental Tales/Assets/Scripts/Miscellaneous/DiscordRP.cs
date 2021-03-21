using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiscordPresence;

public class DiscordRP : MonoBehaviour
{

    public static void setRP(string d)
    {
        Debug.Log("SETTING DISCORD STATE TO: " + d);
        PresenceManager.UpdatePresence(detail: d, state: "By Team12");
    }

}
