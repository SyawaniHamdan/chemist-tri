using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changescene : MonoBehaviour
{
    public void btn_change(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
