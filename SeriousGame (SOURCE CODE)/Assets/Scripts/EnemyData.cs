using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public string enemyName; //The name of the enemy
    public int enemyLevel; //The enemie's level
    public int enemyMaxHP; //The enemie's max health points
    public int enemyCurrentHP; //The enemie's current health points
    public int enemyDamage; //The enemie's damage

    /// <summary>
    /// Function to recieve damage from a player, subracts param damage from the current hp and determines if the enemy is dead or not 
    /// </summary>
    /// <param name="damage">The integar value of damage to be taken</param>
    /// <returns>Returns true if the enemy is dead, false if not</returns>
    public bool TakeDamage(int damage)
    {
        enemyCurrentHP -= damage;

        if(enemyCurrentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
