using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance; //Singleton instance of this script

    public string playerName; //The name of the player
    public List<int> playerLevel; //The player's level
    public int playerMaxHP; //The player's max health points
    public int playerCurrentHP; //The player's current health points
    public int playerDamage; //The player's damage
    public Vector3 playerReturnPosition; //The position the player was in before the encounter started
    public string playerReturnSceneName; //The name of the scene that the player was in before the encounter started

    /// <summary>
    /// Awake function, called before everything else, makes sure there is only one instance of this script
    /// </summary>
    void Awake()
    {
        //If the instance of this class is null, don't destroy this gameObject and set the instance equal to this instance
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        //Otherwise, if the instance is not equal to this, destroy it
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
