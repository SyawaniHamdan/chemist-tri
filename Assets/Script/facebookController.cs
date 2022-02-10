using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class facebookController : MonoBehaviour
{
    const string URI = "http://api.tenenet.net";
    const string token = "50d8e554f009be15093d2c535f3adad3";
    private void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Logged in Successfully");
        }
        else
        {
             Debug.Log("FB is not log in");
        }
    }
    void OnHideUnity(bool isGameShown)
    {
        if (isGameShown)
        {
            Time.timeScale = 1;
        }  
        else
        {
            Time.timeScale = 0;
        }
            
    }
    public void FacebookLogin()
    {
        List<string> permission = new List<string>();
        permission.Add("public_profile");
        FB.LogInWithReadPermissions(permission, AuthCallResult);
    }
    void AuthCallResult(ILoginResult result)
    {
        if (result.Error != null)
        {   
            Debug.Log(result.Error);            
        }
        else
        {
           if(FB.IsLoggedIn)
           {
               Debug.Log("FB logged in");
               FB.API("/me?fields=first_name,last_name,name,id",HttpMethod.GET, callbackData); 
               FB.API("/me?fields=first_name,last_name,name,id",HttpMethod.GET, callbackDataR);                 
           }
           else
           {
               Debug.Log("Login Failed");
           }
        }
    }

    void callbackData(IResult res)
    {
        if(res.Error != null)
        {
            Debug.Log("Error getting data");
        }
        else
        {
            string regAlias = res.ResultDictionary["name"].ToString();
            StartCoroutine(GetPlayer(regAlias, res));
            //StartCoroutine(registerNewPlayer(regAlias, regId, regfname, regLname));
            //Debug.Log(res.ResultDictionary["id"]);

        }
    }
    void callbackDataR(IResult res)
    {
            string regAlias = res.ResultDictionary["name"].ToString();
            string regId = res.ResultDictionary["id"].ToString();
            string regfname = res.ResultDictionary["first_name"].ToString();
            string regLname = res.ResultDictionary["last_name"].ToString();
            //StartCoroutine(GetPlayer(regAlias, res));
            StartCoroutine(registerNewPlayer(regAlias, regId, regfname, regLname));
            //Debug.Log(res.ResultDictionary["id"]);
    }
    
    private IEnumerator GetPlayer(string name, IResult res)
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/getPlayer" + "?token=" + token + "&alias=" + name);

        yield return www.SendWebRequest();
        Description2 callPlayer = new Description2();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            //decripting Json
            callPlayer = JsonUtility.FromJson<Description2>(www.downloadHandler.text);

            // just to check in console
            Debug.Log(callPlayer.message.alias);
        }

        // to check if existed or not
        if(callPlayer.status == "0")  //not
        {
            Debug.Log("Do not exist account");
            callbackDataR(res);
        }
        else if(callPlayer.status == "1")  //exist
        {
            SceneManager.LoadScene("Home");
        }
    }
    private IEnumerator registerNewPlayer(string regname, string regD, string regFn, string regLn)
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/createPlayer" + "?token=" + token + "&alias=" + regname + "&id=" + regD + "&fname=" + regFn + "&lname=" + regLn);

        yield return www.SendWebRequest();
        Description2 regPlayer = new Description2();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            //decripting Json
            regPlayer = JsonUtility.FromJson<Description2>(www.downloadHandler.text);

            // just to check in console
            Debug.Log(regname);
            Debug.Log(regD);
            Debug.Log(regFn);
            Debug.Log(regLn);
        }

        if(regPlayer.status == "0") //false
        {
            Debug.Log("Error getting data");
        }
        else if(regPlayer.status == "1")  //true
        {
            Debug.Log("Success getting data");
            SceneManager.LoadScene("Home");
        }

    }    
}
