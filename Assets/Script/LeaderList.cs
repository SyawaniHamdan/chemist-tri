using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LeaderList : MonoBehaviour
{
    public Text scorelist1;
    public Text scorelist2;
    public Text scorelist3;
    public Text scorelist4;
    public Text scorelist5;

    const string URI = "http://api.tenenet.net";
    const string token = "c34f737806f0bf1ba600969153063139";
    const string scoreID = "chemist_score";
    const string leadID = "chemist_leaderboard";

    private string lboardJson;
    private string rankJson;
    private int limitplayer;
    private int count = 1;
    
    private List<string> arrayName = new List<string>();
    private List<string> arrayScore = new List<string>();

    SortedList<int, string> listName = new SortedList<int, string>();
    public void load()
    {
        start();
    }

    private void start()
    {
        StartCoroutine(getleaderboard());
    }

    public IEnumerator getleaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/getLeaderboard" + "?token=" + token + "&id=" + leadID);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
           Debug.Log(www.downloadHandler.text);
           lboardJson = www.downloadHandler.text;
        }

        var leaderboardJSON = JsonConvert.DeserializeObject<leaderBoard>(lboardJson);
        limitplayer = leaderboardJSON.message.data.Count;
        Debug.Log(limitplayer);

        for(int i=0; i<limitplayer; i++)
        {
            string aliasName = leaderboardJSON.message.data[i].alias.ToString();
            int rankName = leaderboardJSON.message.data[i].rank;
            StartCoroutine(getPlayer(aliasName, rankName));
        }
    }

    private IEnumerator getPlayer(string alias, int rank)
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/getPlayer" + "?token=" + token + "&alias=" + alias);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
           rankJson = www.downloadHandler.text;
        }

        var scoreJson = JsonConvert.DeserializeObject<gpRoot>(rankJson);
        var scores = scoreJson.message.score[0].value;
        var name = scoreJson.message.alias;
        int sco = int.Parse(scores);
        listName.Add(sco, name);

        if(count == 4)
        {
            scorelist1.text = string.Format("{0,-20} {1,-25} {2,-20} \n", "\t1.",listName.Values[3],listName.Keys[3]);
            scorelist2.text = string.Format("{0,-20} {1,-25} {2,-20} \n", "\t2.",listName.Values[2],listName.Keys[2]);
            scorelist3.text = string.Format("{0,-20} {1,-25} {2,-20} \n", "\t3.",listName.Values[1],listName.Keys[1]);
            scorelist4.text = string.Format("{0,-20} {1,-25} {2,-20} \n", "\t4.",listName.Values[0],listName.Keys[0]);
        }
        
        count++;

    }

    public class gpScore
    {
        public string metric_id { get; set;}
        public string metric_name { get; set;}
        public string metric_type { get; set;}
        public string value { get; set;}
    }
    public class gpMessage
    {
        public string first_name { get; set;}
        public string last_name { get; set;}
        public string alias { get; set;}
        public string id { get; set;}
        public string created { get; set;}
        public List <gpScore> score { get; set;}
    }

    public class gpRoot
    {
        public gpMessage message { get; set;}
        public string status { get; set;}
    }

    public class lbDatum
    {
        public string alias { get; set;}
        public int rank { get; set;}
    }

    public class lbMessage
    {
        public List<lbDatum> data { get; set;}
    }
    public class leaderBoard
    {
        public lbMessage message{ get; set;}
        public string status{ get; set;}
    }
}
