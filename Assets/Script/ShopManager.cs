using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ShopManager : MonoBehaviour
{
    public int currentCharIndex = 0;
    public GameObject[] charcterModel;

    public CharacterBlueprint[] charcs;
    public Button buyBtn;

    // Start is called before the first frame update
    void Start()
    {
        //plyref to store variable eventhough close and open back still can get the value
        //if there is key with selected char in the 
        //memory loc and if exist it will return stored 
        //value and then enable car model
        //if not exist, create key with defautl value 0
        
        foreach(CharacterBlueprint charc in charcs)
        {
            if(charc.price == 0)
            {
                charc.isUnlocked = true;
            }
            else
            {
                charc.isUnlocked = PlayerPrefs.GetInt(charc.name, 0) == 0 ? false: true;
            }
        }
        currentCharIndex = PlayerPrefs.GetInt("SelecetedCharacter", 0); 
        foreach(GameObject character in charcterModel)
        {
            character.SetActive(false);
        }

        //enable current car
        charcterModel[currentCharIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void ChangeNext()
    {
        charcterModel[currentCharIndex].SetActive(false);

        currentCharIndex++;
        if(currentCharIndex == charcterModel.Length)
        {
            currentCharIndex = 0;
        }

        charcterModel[currentCharIndex].SetActive(true);
        CharacterBlueprint c = charcs[currentCharIndex];
        if(!c.isUnlocked)
        {
            return;
        }
        PlayerPrefs.SetInt("SelecetedCharacter", currentCharIndex); 
    }

    public void ChangePrevious()
    {
        charcterModel[currentCharIndex].SetActive(false);

        //decrement kan
        currentCharIndex--;
        if(currentCharIndex == -1 )
        {
            currentCharIndex = charcterModel.Length - 1;
        }

        charcterModel[currentCharIndex].SetActive(true);
        CharacterBlueprint c = charcs[currentCharIndex];
        if(!c.isUnlocked)
        {
            return;
        }
        PlayerPrefs.SetInt("SelecetedCharacter", currentCharIndex); 
    }

    public void UnlockCar()
    {
        CharacterBlueprint c = charcs[currentCharIndex];
        PlayerPrefs.SetInt(c.name, 1);
        PlayerPrefs.SetInt("SelectedCar", currentCharIndex);
        c.isUnlocked = true;
        PlayerPrefs.SetInt("NumberOfCoins", PlayerPrefs.GetInt("NumberOfCoins", 0) - c.price);
    }

    public void StartGame()
    {
        PlayerStat.SELECTEDPLAYER = currentCharIndex;
        ExitGames.Client.Photon.Hashtable selectedPlayer = new ExitGames.Client.Photon.Hashtable
        {{Multiplayer.PLAYER_SELECTION_NUM, currentCharIndex}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(selectedPlayer);
        PhotonNetwork.LoadLevel("Search");
    }

    private void UpdateUI()
    {
        CharacterBlueprint c = charcs[currentCharIndex];

        if(c.isUnlocked)
        {
            buyBtn.gameObject.SetActive(false);
        }
        else
        {
            buyBtn.gameObject.SetActive(true);
            buyBtn.GetComponentInChildren<Text>().text = "BUY - " + c.price;
            if(c.price < PlayerPrefs.GetInt("NumberOfCoins", 0))
            {
                buyBtn.interactable = true;
            }
            else
            {
                buyBtn.interactable = false;
            }
        }
    }
}
