using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] Object[] characters;
    [SerializeField] player character;
    GameObject[] SpawnPoints;
    Vector3 spawnPos;
    PlayerStat playerStat;
    void Start()
    {
        characters = Resources.LoadAll("Characters", typeof(player));
        character = (player)characters[PlayerStat.SELECTEDPLAYER];
        spawnPos = new Vector3(Random.Range(-40, 40), 10, Random.Range(-40, 40));
        Instantiate(character, spawnPos, character.transform.rotation);

    }
}
