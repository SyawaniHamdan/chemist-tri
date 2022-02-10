using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogoutController : MonoBehaviour
{
    public GameObject LogoutCanvas;
    public GameObject MainMenuCanvas;

    public void FindLogout()
    {
        LogoutCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);

    }
    public void AccountLogout()
    {
        SceneManager.LoadScene("Main");
    }
}
