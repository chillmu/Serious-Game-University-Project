using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public string bookName;
    public string roomName;

    private void Awake()
    {
        if(PlayerPrefs.GetInt(bookName) == 1)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Determines when a GameObject with collision collides with this GameObject
    /// </summary>
    /// <param name="collision">The collision that has occured</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            PlayerPrefs.SetInt(bookName, 1);
            PlayerPrefs.SetString(roomName + "CompletedDate", DateTime.Now.ToString("dd-MM-yyyy\\THH:mm:ss\\Z"));
            PlayerPrefs.Save();
            AchievementManager.Instance.EarnAchievement(roomName + " Completed");

            RoomOrder.Instance.AddRoom(roomName);

            Destroy(gameObject);

            int amount = 0;
            int total = 0;

            for(int i = 1; i <= 15; i++)
            {
                if (PlayerPrefs.GetInt("Book" + i) == 1)
                {
                    amount += 1;
                }

                total += 1;
            }

            float percent = 100 * (amount / total);

            if(AchievementManager.Instance.achievementDictionary.ContainsKey(percent + " Percent Books Earned"))
            {
                AchievementManager.Instance.EarnAchievement(percent + " Percent Books Earned");
            }
        }
    }
}