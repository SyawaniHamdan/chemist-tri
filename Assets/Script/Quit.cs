using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quit : MonoBehaviour
{
    private Button quit;

    void Start()
    {
        quit = GetComponent<Button>();
        quit.onClick.AddListener(QuitGame);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
