using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour
{
    public static string USERID = "USERID", USERMAIL = "USERMAIL";
    public static int SELECTEDPLAYER = 0;
    public PlayerStat(string useid, string usermail)
    {
        USERID = useid;
        USERMAIL = usermail;
    }
}
