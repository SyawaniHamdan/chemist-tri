using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessingScore : MonoBehaviour
{
    public static AccessingScore Instance;

    public int score = 0;

    public string MainName;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
