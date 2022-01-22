using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterGenerator : MonoBehaviour
{
    int chance;
    PlayerMovement playerMovement; //Reference to PlayerMovement script attahced to this gameObject
    PlayerData playerData;

    public GameObject roomManagerGameObject;

    /// <summary>
    /// Start method, called after Awake, initialises this script
    /// </summary>
    private void Start()
    {
        chance = UnityEngine.Random.Range(64, 255);
        //print("chance = " + chance);
        playerMovement = gameObject.GetComponent<PlayerMovement>(); //Get the PlayerMovement script attached to this gameObject
        playerData = gameObject.GetComponent<PlayerData>();
    }

    /// <summary>
    /// Update method, called every frame update, checks if the player is moving or not, if they are then invoke a repeating function to attempt an encouter
    /// </summary>
    private void Update()
    {
        if(playerMovement.isMoving == true)
        {
            InvokeRepeating("EncounterTick", 0f, 2f);
        }
        else
        {
            CancelInvoke("EncounterTick");
        }
    }

    /// <summary>
    /// Function to progress the chance of an encounter, each 'tick' increases the chance
    /// </summary>
    public void EncounterTick()
    {
        if(chance <= 0)
        {
            chance = UnityEngine.Random.Range(64, 255);
            playerData.playerReturnPosition = this.gameObject.transform.position;
            playerData.playerReturnSceneName = SceneManager.GetActiveScene().name;
            playerData.SaveData();

            DateTime roomEntryTime = roomManagerGameObject.GetComponent<RoomManager>().roomEntryTime;
            DateTime currentTime = DateTime.Now;

            TimeSpan time = currentTime - roomEntryTime;
            string seconds = time.Seconds.ToString();

            RoomTimes.Instance.AddRoomTime(new RoomTime(seconds, playerData.playerReturnSceneName));

            SceneManager.LoadScene("EncounterScene", LoadSceneMode.Single); //Load the encounter scene
            playerMovement.isMoving = false;
        }
        else
        {
            chance -= 2;
            //print("new chance = " + chance);
            playerMovement.isMoving = false;
        } 
    }
}


