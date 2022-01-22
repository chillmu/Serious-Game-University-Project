using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public string playerName; //The name of the player
    public List<int> playerLevel; //The player's level
    public int playerMaxHP; //The player's max health points
    public int playerCurrentHP; //The player's current health points
    public int playerDamage; //The player's damage
    public Vector3 playerReturnPosition; //The position the player will be returned to after an encounter has finished
    public string playerReturnSceneName; //The scene the player will be returned to after an encounter has finished

    /// <summary>
    /// Start function, loads all of the nessesary data
    /// </summary>
    private void Start()
    {
        LoadData();
    }

    /// <summary>
    /// Function to recieve damage from an enemy
    /// </summary>
    /// <param name="damage">The number of points to be subracted from the current health points</param>
    /// <returns>Returns true if the player is dead, otherwise returns false</returns>
    public bool TakeDamage(int damage)
    {
        playerCurrentHP -= damage;

        if(playerCurrentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Sets the player return position
    /// </summary>
    /// <param name="pos">The position to be set as the player return position</param>
    public void SetReturnPosition(Vector3 pos)
    {
        playerReturnPosition = pos;
    }

    /// <summary>
    /// Saves all of the requred data
    /// </summary>
    public void SaveData()
    {
        GameData.Instance.playerName = playerName;
        GameData.Instance.playerLevel = playerLevel;
        GameData.Instance.playerMaxHP = playerMaxHP;
        GameData.Instance.playerCurrentHP = playerCurrentHP;
        GameData.Instance.playerDamage = playerDamage;
        GameData.Instance.playerReturnPosition = playerReturnPosition;
        GameData.Instance.playerReturnSceneName = playerReturnSceneName;
    }

    /// <summary>
    /// Loads all of the required data
    /// </summary>
    public void LoadData()
    {
        playerName = GameData.Instance.playerName;
        playerLevel = GameData.Instance.playerLevel;
        playerMaxHP = GameData.Instance.playerMaxHP;
        playerCurrentHP = GameData.Instance.playerCurrentHP;
        playerDamage = GameData.Instance.playerDamage;
        playerReturnPosition = GameData.Instance.playerReturnPosition;
        playerReturnSceneName = GameData.Instance.playerReturnSceneName;
    }
}
