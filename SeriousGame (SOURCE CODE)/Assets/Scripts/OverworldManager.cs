using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    public GameObject player; //The player

    /// <summary>
    /// Start function, sets up the doors which send the player to other scenes
    /// </summary>
    private void Start()
    {
        player.GetComponent<EncounterGenerator>().enabled = false; //Disable the encounter generator
        player.GetComponent<PlayerData>().playerCurrentHP = 10;
        player.GetComponent<PlayerData>().SaveData();

        //Get the player data and make sure it's up to date
        PlayerData playerData = player.GetComponent<PlayerData>();
        playerData.LoadData();

        //Get a list of Door GameObjects
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        //Check each door and see if Door's room name is the same as the room the player just came from
        foreach (GameObject door in doors)
        {
            DoorManager doorManager = door.GetComponent<DoorManager>();

            if(playerData.playerReturnSceneName == doorManager.sceneToLoad)
            {
                player.transform.position = door.transform.position + new Vector3(0f, -1.5f, 0f);
                return;
            }
        }
    }
}