using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject findMatch;
    [SerializeField]
    GameObject searchingPanel;

    void Start()
    {
        searchingPanel.SetActive(false);
        findMatch.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED " + PhotonNetwork.CloudRegion + " SERVER");
        PhotonNetwork.AutomaticallySyncScene = true;
        findMatch.SetActive(true);
    }

    public void FindMatch()
    {
        searchingPanel.SetActive(true);
        findMatch.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Searching for a Game");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could Find Room - Creating a Room");
        MakeRoom();
    }
    void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom("RoomName_ "+ randomRoomName, roomOptions);
        Debug.Log("Room Created, Waiting For Another Player");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
       {
           Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + "/2 Starting Game");
           PhotonNetwork.LoadLevel("LabScene");
       }
    }
    public void StopSearch()
    {
        searchingPanel.SetActive(false);
        findMatch.SetActive(true);
        PhotonNetwork.LeaveRoom();
        Debug.Log("Stopped! Back to Menu");
 
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
    }
}
