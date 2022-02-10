using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject[] SpawnPoints;
    CameraFollow cameraFollow;
    public GameObject dicsonnectPanel;
    [SerializeField] Object[] characters;


    void Awake()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        if(PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }
    // Start is called before the first frame update

    void SpawnPlayer()
    {
        int player = 0;
        object playerNo;
        if(PhotonNetwork.IsMasterClient)
        {
            player = 0;
            
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Multiplayer
            .PLAYER_SELECTION_NUM, out playerNo))
            {
                GameObject Player = PhotonNetwork.Instantiate(characters[(int)playerNo].name, SpawnPoints[player].transform.position, Quaternion.identity);
                cameraFollow.SetCameraTarget(Player.transform);
            }
            
        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            player = 1;
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Multiplayer
            .PLAYER_SELECTION_NUM, out playerNo))
            {
                GameObject Player2 = PhotonNetwork.Instantiate(characters[(int)playerNo].name, SpawnPoints[player].transform.position, Quaternion.identity);
                cameraFollow.SetCameraTarget(Player2.transform);
            }
        }
    }
    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad()
    {
        //PhotonNetwork.Disconnect();
        //while (PhotonNetwork.IsConnected)
        PhotonNetwork.LeaveRoom();
        while(PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel("Search");
    }

    private void OnPhotonPlayerDisconnected(Player otherPlayer)
    {
        dicsonnectPanel.gameObject.SetActive(true);
    }

        /*if(PhotonNetwork.IsMasterClient)
        {
            player = 0;
            characterValue = whichCharacter;
            myCharacter = Instantiate(PlayerInfo.PI.charcters[whichCharacter], SpawnPoints[player].transform.position, Quaternion.identity);
            cameraFollow.SetCameraTarget(myCharacter.transform);

        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            player = 1;
            characterValue = whichCharacter;
            myCharacter = Instantiate(PlayerInfo.PI.charcters[whichCharacter], SpawnPoints[player].transform.position, Quaternion.identity);
            cameraFollow.SetCameraTarget(myCharacter.transform);
        }*/
        
    
}
