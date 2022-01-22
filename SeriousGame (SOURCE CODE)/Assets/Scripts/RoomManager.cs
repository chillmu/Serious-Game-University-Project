using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public GameObject player;
    PlayerData playerData;

    public List<int> roomLevel;

    public Transform playerDoorEntryPosition; //Door position

    public DateTime roomEntryTime;

    private void Start()
    {
        roomEntryTime = DateTime.Now;

        playerData = player.GetComponent<PlayerData>(); //Get the PlayerData script from the Player GameObject set in the inspector
        playerData.LoadData();

        //If the player entered via door, else
        if (playerData.playerReturnPosition == Vector3.zero)
        {
            //position the player in front of the door
            Vector2 position = playerDoorEntryPosition.position + new Vector3(0f, 1.5f, 0f);
            player.transform.position = position;
        }
        else
        {
            player.transform.position = playerData.playerReturnPosition; //Get the Vector3 playerReturnPosition and set the Player's position to that
        }

        playerData.playerLevel = roomLevel;
        playerData.playerReturnSceneName = SceneManager.GetActiveScene().name;
        playerData.SaveData();
    }
}
