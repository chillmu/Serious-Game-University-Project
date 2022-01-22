using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayRoomHiraganaManager : MonoBehaviour
{
    public string room;
    public List<int> level;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("DisplayHiragana" + room) == "true")
        {
            PlayerPrefs.SetString("GoBackToThisRoom", room);

            if (level.Count > 1)
            {
                PlayerPrefs.SetInt("DisplayThisSet1", level[0]);
                PlayerPrefs.SetInt("DisplayThisSet2", level[1]);

                PlayerPrefs.Save();
                ChangeScene();

            }
            else
            {
                PlayerPrefs.SetInt("DisplayThisSet1", level[0]);
                PlayerPrefs.SetInt("DisplayThisSet2", 0);

                PlayerPrefs.Save();
                ChangeScene();
            }
        }
        else
        {
            return;
        }


        /*
        if(PlayerPrefs.GetInt("DontDisplay" + room) == 1) 
        {
            PlayerPrefs.SetInt("DontDisplay" + room, 0);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetString("GoBackToThisRoom", room);

            if(level.Count > 1)
            {
                PlayerPrefs.SetInt("DisplayThisSet1", level[0]);
                PlayerPrefs.SetInt("DisplayThisSet2", level[1]);

            }
            else
            {
                PlayerPrefs.SetInt("DisplayThisSet1", level[0]);
                PlayerPrefs.SetInt("DisplayThisSet2", 0);
            }

            PlayerPrefs.SetInt("DontDisplay" + room, 1);
            PlayerPrefs.Save();
            ChangeScene();
        }
        */
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("DisplayHiraganaRoom", LoadSceneMode.Single);
    }
}
