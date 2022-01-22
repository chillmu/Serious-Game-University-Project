using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Text nameText; //The UI text for the player name
    public Text levelText; //The UI text for the player level
    public Slider hpSlider; //The UI slider for the player health points

    /// <summary>
    /// Function to set the UI element's values to reflect the player
    /// </summary>
    /// <param name="player">The PlayerData of the player</param>
    public void SetHUD(PlayerData player)
    {
        nameText.text = player.playerName;
        levelText.text = "Lvl " + player.playerLevel;
        hpSlider.maxValue = player.playerMaxHP;
        hpSlider.value = player.playerCurrentHP;
    }

    /// <summary>
    /// Function to set the UI slider value
    /// </summary>
    /// <param name="hp">The number the UI slider value should be set to</param>
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
