using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    public Text nameText; //The enemie's UI name
    public Text levelText; //The enemie's UI level
    public Slider hpSlider; //The enemie's hp slider

    /// <summary>
    /// Configures the UI values to reflect the encountered enemy
    /// </summary>
    /// <param name="enemy">The EnemyData of the enemy that has been encountered</param>
    public void SetHUD(EnemyData enemy)
    {
        nameText.text = enemy.enemyName;
        levelText.text = "Lvl " + enemy.enemyLevel;
        hpSlider.maxValue = enemy.enemyMaxHP;
        hpSlider.value = enemy.enemyCurrentHP;
    }

    /// <summary>
    /// Sets the UI slider value to be that of the hp given
    /// </summary>
    /// <param name="hp">The number the UI slider should be set to</param>
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
