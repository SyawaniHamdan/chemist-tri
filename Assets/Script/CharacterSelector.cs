using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] charcters;

    public int currentCharIndex = 0;
    

    void Start()
    {

        currentCharIndex = PlayerPrefs.GetInt("SelecetedCharacter", 0); 
        foreach(GameObject character in charcters)
        {
            character.SetActive(false);
        }

        //enable current car
        charcters[currentCharIndex].SetActive(true);
    }

}
