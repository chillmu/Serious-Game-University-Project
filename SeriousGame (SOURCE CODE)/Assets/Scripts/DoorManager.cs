using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    public string sceneToLoad;

    public Sprite doorOpen;
    public Sprite doorClosed;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Book1") == 1 || PlayerPrefs.GetInt("Book2") == 1)
        {
            PlayerPrefs.SetInt("Room11Unlocked", 1);
        }

        if(PlayerPrefs.GetInt("Book3") == 1 || PlayerPrefs.GetInt("Book4") == 1)
        {
            PlayerPrefs.SetInt("Room12Unlocked", 1);
        }

        if(PlayerPrefs.GetInt("Book5") == 1 || PlayerPrefs.GetInt("Book6") == 1)
        {
            PlayerPrefs.SetInt("Room13Unlocked", 1);
        }

        if (PlayerPrefs.GetInt("Book7") == 1 || PlayerPrefs.GetInt("Book8") == 1)
        {
            PlayerPrefs.SetInt("Room14Unlocked", 1);
        }

        if (PlayerPrefs.GetInt("Book9") == 1 || PlayerPrefs.GetInt("Book10") == 1)
        {
            PlayerPrefs.SetInt("Room15Unlocked", 1);
        }

        PlayerPrefs.Save();

        //if the room is not unlocked then hide it
        if (PlayerPrefs.GetInt(sceneToLoad + "Unlocked") != 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = doorClosed;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
        }
    }

    /// <summary>
    /// Determines when a GameObject with collision collides with this GameObject
    /// </summary>
    /// <param name="collision">The collision that has occured</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /**
         * Player is entering a Room via Door therefore we need to set the return position to null so that the player does
         * not enter the Room at the return position because that is only needed when returning to a Room via an Encounter
         * In the RoomManager script we can check if the player return position is set to Vector3.zero, if so, set the player
         * position in front of the door, otherwise set the position to the player return position because the player is
         * returning from an Encounter.
         */

        if(gameObject.GetComponent<SpriteRenderer>().sprite == doorOpen)
        {
            if (collision.collider.GetComponent<PlayerData>())
            {
                PlayerData playerData = collision.collider.GetComponent<PlayerData>();
                playerData.playerReturnPosition = Vector3.zero;
                playerData.SaveData();

                int amount = PlayerPrefs.GetInt(sceneToLoad + "Entered");
                amount += 1;
                PlayerPrefs.SetInt(sceneToLoad + "Entered", amount);

                PlayerPrefs.SetString("DisplayHiragana" + sceneToLoad, "true");

                PlayerPrefs.Save();

                if(sceneToLoad == "Overworld")
                {
                    int currentHp = playerData.playerCurrentHP;

                    HPManager.Instance.AddHpValue(currentHp);
                }

                SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
}
