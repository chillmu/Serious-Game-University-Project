using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button achievementMenuButton;
    public Button hiraganaDictionaryButton;
    public Button analyticsMenuButton;
    public Button mnemonicsMenuButton;

    public GameObject achievementCanvas;
    public GameObject hiraganaDictionaryCanvas;
    public GameObject analyticsCanvas;
    public GameObject mnemonicsCanvas;

    public GameObject thisMenu;

    public Text totalTimePlayed;

    bool isShowing;

    private void Start()
    {
        achievementCanvas = AchievementManager.Instance.transform.GetChild(0).gameObject;
        mnemonicsCanvas = MnemonicsManager.Instance.transform.GetChild(0).gameObject;

        achievementMenuButton.onClick.AddListener(delegate { DisplayAchievementMenu(); });
        hiraganaDictionaryButton.onClick.AddListener(delegate { DisplayHiraganaDictionaryMenu(); });
        analyticsMenuButton.onClick.AddListener(delegate { DisplayAnalyticsMenu(); });
        mnemonicsMenuButton.onClick.AddListener(delegate { DisplayMnemonicsMenu(); });

        StartCoroutine(CountSessions());
    }

    IEnumerator CountSessions()
    {
        yield return new WaitForSeconds(1f);


    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isShowing = !isShowing;
            thisMenu.SetActive(isShowing);
        }

        TimeSpan totalTimeSpan = new TimeSpan();
        List<string> sessionTimesStrings = new List<string>();
        sessionTimesStrings = SessionManager.Instance.sessionTimes;

        foreach (string time in sessionTimesStrings)
        {
            TimeSpan ts = TimeSpan.Parse(time);
            totalTimeSpan += ts;
        }

        totalTimeSpan += TimeSpan.FromSeconds(Time.realtimeSinceStartup);

        totalTimePlayed.text = "Days: " + totalTimeSpan.Days + "\n" +
                               "Hours: " + totalTimeSpan.Hours + "\n" +
                               "Minutes: " + totalTimeSpan.Minutes + "\n" +
                               "Seconds: " + totalTimeSpan.Seconds;
    }

    void DisplayMnemonicsMenu()
    {
        mnemonicsCanvas.SetActive(true);
        thisMenu.SetActive(false);
        int mnemonicsAmount = PlayerPrefs.GetInt("MnemonicsAmount");
        mnemonicsAmount += 1;
        PlayerPrefs.SetInt("MnemonicsAmount", mnemonicsAmount);
        PlayerPrefs.Save();
    }

    void DisplayAchievementMenu()
    {
        achievementCanvas.SetActive(true);
        thisMenu.SetActive(false);
        int achievementAmount = PlayerPrefs.GetInt("AchievementAmount");
        achievementAmount += 1;
        PlayerPrefs.SetInt("AchievementAmount", achievementAmount);
        PlayerPrefs.Save();
    }

    void DisplayHiraganaDictionaryMenu()
    {
        hiraganaDictionaryCanvas.SetActive(true);
        thisMenu.SetActive(false);
        int dictionaryAmount = PlayerPrefs.GetInt("DictionaryAmount");
        dictionaryAmount += 1;
        PlayerPrefs.SetInt("DictionaryAmount", dictionaryAmount);
        PlayerPrefs.Save();
    }

    void DisplayAnalyticsMenu()
    {
        analyticsCanvas.SetActive(true);
        thisMenu.SetActive(false);
        int analyticsAmount = PlayerPrefs.GetInt("AnalyticsAmount");
        analyticsAmount += 1;
        PlayerPrefs.SetInt("AnalyticsAmount", analyticsAmount);
        PlayerPrefs.Save();
    }
}
