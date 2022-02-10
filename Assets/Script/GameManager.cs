using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Description
{
    public string first_name;
    public string last_name;
    public string alias;
    public string id;
    public string created;
    public List<metricClass> score;
    public string rank;
}

[System.Serializable]
public class Description2
{
    public Description message;
    public string status;
}

[System.Serializable]
public class Description3
{
    public List<Description> data;
}

[System.Serializable]
public class metricClass
{
    public string metric_id;
    public string metric_name;
    public string metric_type;
    public string value;
}

[System.Serializable]
public class Description4
{
    public Description3 message;
    public string status;
}

public class GameManager : MonoBehaviour
{
    const string URI = "http://api.tenenet.net";
    const string token = "50d8e554f009be15093d2c535f3adad3";
    const string scoreID = "chemist_score";
    const string leadID = "chemist_leaderboard";

    //declare instance
    public static GameManager Instance;

    //private string alias;
    //private string id;
    //private string fname = " ";
    //private string lname = " ";

    // input for login
    public InputField aliasName;

    //input for register
    public InputField aliasReg;
    public InputField idreg;
    public InputField fnamereg;
    public InputField lnamereg;


    // error message and page jump for logIn
    public GameObject holdMenu;
    public GameObject holdLogIn;
    public GameObject errorMessage;

    // error message and jump page for Register
    public GameObject holdRegister;

    //public GameObject textPre;
    //public GameObject holdContent;

    

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(getLeaderboard());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //fucntion call for tenenet features
    public void Login()
    {
        string holdAlias = aliasName.text;
        //hold for other scene
        AccessingScore.Instance.MainName = holdAlias;
        StartCoroutine(GetPlayer(holdAlias));
    }

    public void Register()
    {
        string regAlias = aliasReg.text;
        string regId = idreg.text;
        string regfname = fnamereg.text;
        string regLname = lnamereg.text;
        StartCoroutine(registerNewPlayer(regAlias, regId, regfname, regLname));
    }


    public void PlayerActivity(string name)
    {
        //string name = aliasName.text;
        string op = "add";
        float val = AccessingScore.Instance.score;
        StartCoroutine(insertPlayerActivity(name, op, val));
    }

    public void Leaderboard()
    {
        StartCoroutine(getLeaderboard());
    }

    
    //instance
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
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

        if(regPlayer.status == "0")
        {
            holdRegister.SetActive(true);
        }
        else if(regPlayer.status == "1")
        {
            holdLogIn.SetActive(true);
            holdRegister.SetActive(false);
        }

    }

    private IEnumerator GetPlayer(string name)
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
        if(callPlayer.status == "0")
        {
            errorMessage.SetActive(true);
            holdLogIn.SetActive(true);
        }
        else if(callPlayer.status == "1")
        {
            holdLogIn.SetActive(false);
            SceneManager.LoadScene("Home");
        }
    }

    private IEnumerator insertPlayerActivity(string name, string op, float val)
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/insertPlayerActivity" + "?token=" + token + "&alias=" + name + "&id=" + scoreID + "&operator=" + op + "&value=+" + val) ;

        yield return www.SendWebRequest();
        

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);       
        }

    }

    private IEnumerator getLeaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/getLeaderboard" + "?token=" + token + "&id=" + leadID);

        yield return www.SendWebRequest();
        Description4 getLead = new Description4();
        
        List<metricClass> score = new List<metricClass> ();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            getLead = JsonUtility.FromJson<Description4>(www.downloadHandler.text);

           // Debug.Log(getLead.message.data[0].alias);
            //Debug.Log(getLead.message.data[2].rank);

           /* for(int x = 0; x < getLead.message.data.Count; x++)
            {
                GameObject inst1 = Instantiate(textPre, holdContent.transform);
                inst1.GetComponent<Text>().text = getLead.message.data[x].alias;
                inst1 = Instantiate(textPre, holdContent.transform);
                inst1.GetComponent<Text>().text = getLead.message.data[x].rank;
                
            }*/

        }

    }
}
