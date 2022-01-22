using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab; //The Achievement prefab
    public GameObject achievements; //The parent that holds Achievements
    public GameObject visualAchievement; //The Achievement prefab variant to be shown on screen when an Achievement is unlocked
    public GameObject visualAchievementParent; //The parent that displays unlocked Achievements
    public Text textPoints; //The total number of points gained from unlocked Achievements
    public Dictionary<string, Achievement> achievementDictionary = new Dictionary<string, Achievement>(); //A dictionary of all Achievements where the key is the Achievement's name and the value is the Achievement itself
    public static AchievementManager Instance; //The static instance of this class, singleton design pattern
    private int fadeTime = 1; //The fade time in seconds for how long an earned Achievement should be displayed fading in and then fading out
    public Button closeMenuButton;
    public GameObject thisMenu;

    /// <summary>
    /// The Awake function, called before every other function, as part of the singleton design pattern, makes sure there is only ever one instance of this class
    /// </summary>
    private void Awake()
    {
        PlayerPrefs.SetInt("OverworldUnlocked", 1);
        PlayerPrefs.SetInt("Room1Unlocked", 1);
        PlayerPrefs.SetInt("Room2Unlocked", 1);
        PlayerPrefs.SetInt("Room3Unlocked", 1);
        PlayerPrefs.SetInt("Room4Unlocked", 1);
        PlayerPrefs.SetInt("Room5Unlocked", 1);
        PlayerPrefs.SetInt("Room6Unlocked", 1);
        PlayerPrefs.SetInt("Room7Unlocked", 1);
        PlayerPrefs.SetInt("Room8Unlocked", 1);
        PlayerPrefs.SetInt("Room9Unlocked", 1);
        PlayerPrefs.SetInt("Room10Unlocked", 1);
        PlayerPrefs.Save();

        if(!PlayerPrefs.HasKey("PreviousHiraganaCharacter"))
        {
            PlayerPrefs.SetString("PreviousHiraganaCharacter", "\u3042");
            PlayerPrefs.SetString("PreviousHiraganaSound", "a");
            PlayerPrefs.SetString("PreviousHiraganaColumn", "v");
            PlayerPrefs.SetInt("PreviousHiraganaLevel", 1);
        }

        //If the instance of this class is null, don't destroy this gameObject and set the instance equal to this instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Otherwise, if the instance is not equal to this, destroy it
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// The Start function, called after the Awake function, sets up all of the Achievements
    /// </summary>
    private void Start()
    {
        if(closeMenuButton)
        {
            closeMenuButton.onClick.AddListener(delegate { CloseMenu(); });
        }
       
        //REMEMBER TO REMOVE THIS
        //PlayerPrefs.DeleteAll(); 

        //Adding all the achievements
        CreateAchievement("Room1 Completed", "Get the Book from Room 1, Room 11 is now unlocked, unless already unlocked", "1", 25, 0);
        CreateAchievement("A Mastery", "Master the Hiragana character \u3042", "\u3042", 5, 3);
        CreateAchievement("I Mastery", "Master the Hiragana character \u3044", "\u3044", 5, 3);
        CreateAchievement("U Mastery", "Master the Hiragana character \u3046", "\u3046", 5, 3);
        CreateAchievement("E Mastery", "Master the Hiragana character \u3048", "\u3048", 5, 3);
        CreateAchievement("O Mastery", "Master the Hiragana character \u304A", "\u304A", 5, 3);

        CreateAchievement("Room2 Completed", "Get the Book from Room 2, Room 11 is now unlocked, unless already unlocked", "2", 25, 0);
        CreateAchievement("KA Mastery", "Master the Hiragana character \u304B", "\u304B", 5, 3);
        CreateAchievement("KI Mastery", "Master the Hiragana character \u30AD", "\u30AD", 5, 3);
        CreateAchievement("KU Mastery", "Master the Hiragana character \u30AF", "\u30AF", 5, 3);
        CreateAchievement("KE Mastery", "Master the Hiragana character \u30B1", "\u30B1", 5, 3);
        CreateAchievement("KO Mastery", "Master the Hiragana character \u30B3", "\u30B3", 5, 3);

        CreateAchievement("Room3 Completed", "Get the Book from Room 3, Room 12 is now unlocked, unless already unlocked", "3", 25, 0);
        CreateAchievement("SA Mastery", "Master the Hiragana character \u30B5", "\u30B5", 5, 3);
        CreateAchievement("SHI Mastery", "Master the Hiragana character \u3057", "\u3057", 5, 3);
        CreateAchievement("SU Mastery", "Master the Hiragana character \u3059", "\u3059", 5, 3);
        CreateAchievement("SE Mastery", "Master the Hiragana character \u305B", "\u305B", 5, 3);
        CreateAchievement("SO Mastery", "Master the Hiragana character \u305D", "\u305D", 5, 3);

        CreateAchievement("20 Percent Books Earned", "Earn 20 percent of the books, or 3 of them", "20%", 20, 0);

        CreateAchievement("Room4 Completed", "Get the Book from Room 4, Room 12 is now unlocked, unless already unlocked", "4", 25, 0);
        CreateAchievement("TA Mastery", "Master the Hiragana character \u305F", "\u305F", 5, 3);
        CreateAchievement("CHI Mastery", "Master the Hiragana character \u3061", "\u3061", 5, 3);
        CreateAchievement("TSU Mastery", "Master the Hiragana character \u3064", "\u3064", 5, 3);
        CreateAchievement("TE Mastery", "Master the Hiragana character \u3066", "\u3066", 5, 3);
        CreateAchievement("TO Mastery", "Master the Hiragana character \u3068", "\u3068", 5, 3);

        CreateAchievement("Room5 Completed", "Get the Book from Room 5, Room 13 is now unlocked, unless already unlocked", "5", 25, 0);
        CreateAchievement("NA Mastery", "Master the Hiragana character \u306A", "\u306A", 5, 3);
        CreateAchievement("NI Mastery", "Master the Hiragana character \u306B", "\u306B", 5, 3);
        CreateAchievement("NU Mastery", "Master the Hiragana character \u306C", "\u306C", 5, 3);
        CreateAchievement("NE Mastery", "Master the Hiragana character \u306D", "\u306D", 5, 3);
        CreateAchievement("NO Mastery", "Master the Hiragana character \u306E", "\u306E", 5, 3);

        CreateAchievement("Room6 Completed", "Get the Book from Room 6, Room 13 is now unlocked, unless already unlocked", "6", 25, 0);
        CreateAchievement("HA Mastery", "Master the Hiragana character \u306F", "\u306F", 5, 3);
        CreateAchievement("HI Mastery", "Master the Hiragana character \u3072", "\u3072", 5, 3);
        CreateAchievement("FU Mastery", "Master the Hiragana character \u3075", "\u3075", 5, 3);
        CreateAchievement("HE Mastery", "Master the Hiragana character \u3078", "\u3078", 5, 3);
        CreateAchievement("HO Mastery", "Master the Hiragana character \u307B", "\u307B", 5, 3);

        CreateAchievement("40 Percent Books Earned", "Earn 40 percent of the books, or 6 of them", "40%", 40, 0);

        CreateAchievement("Room7 Completed", "Get the Book from Room 7, Room 14 is now unlocked, unless already unlocked", "7", 25, 0);
        CreateAchievement("MA Mastery", "Master the Hiragana character \u307E", "\u307E", 5, 3);
        CreateAchievement("MI Mastery", "Master the Hiragana character \u307F", "\u307F", 5, 3);
        CreateAchievement("MU Mastery", "Master the Hiragana character \u3080", "\u3080", 5, 3);
        CreateAchievement("ME Mastery", "Master the Hiragana character \u3081", "\u3081", 5, 3);
        CreateAchievement("MO Mastery", "Master the Hiragana character \u3082", "\u3082", 5, 3);

        CreateAchievement("Room8 Completed", "Get the Book from Room 8, Room 14 is now unlocked, unless already unlocked", "8", 25, 0);
        CreateAchievement("YA Mastery", "Master the Hiragana character \u3084", "\u3084", 5, 3);
        CreateAchievement("YU Mastery", "Master the Hiragana character \u3086", "\u3086", 5, 3);
        CreateAchievement("YO Mastery", "Master the Hiragana character \u3088", "\u3088", 5, 3);

        CreateAchievement("Room9 Completed", "Get the Book from Room 9, Room 15 is now unlocked, unless already unlocked", "9", 25, 0);
        CreateAchievement("RA Mastery", "Master the Hiragana character \u3089", "\u3089", 5, 3);
        CreateAchievement("RI Mastery", "Master the Hiragana character \u308A", "\u308A", 5, 3);
        CreateAchievement("RU Mastery", "Master the Hiragana character \u308B", "\u308B", 5, 3);
        CreateAchievement("RE Mastery", "Master the Hiragana character \u308C", "\u308C", 5, 3);
        CreateAchievement("RO Mastery", "Master the Hiragana character \u308D", "\u308D", 5, 3);

        CreateAchievement("60 Percent Books Earned", "Earn 60 percent of the books, or 9 of them", "60%", 60, 0);

        CreateAchievement("Room10 Completed", "Get the Book from Room 10, Room 15 is now unlocked, unless already unlocked", "10", 25, 0);
        CreateAchievement("WA Mastery", "Master the Hiragana character \u308F", "\u308F", 5, 3);
        CreateAchievement("WI Mastery", "Master the Hiragana character \u3090", "\u3090", 5, 3);
        CreateAchievement("WE Mastery", "Master the Hiragana character \u3091", "\u3091", 5, 3);
        CreateAchievement("WO Mastery", "Master the Hiragana character \u3092", "\u3092", 5, 3);
        CreateAchievement("N Mastery", "Master the Hiragana character \u3093", "\u3093", 5, 3);

        CreateAchievement("Room11 Completed", "Get the Book from Room 11", "11", 50, 0);
        CreateAchievement("Room12 Completed", "Get the Book from Room 12", "12", 50, 0);

        CreateAchievement("80 Percent Books Earned", "Earn 80 percent of the books, or 12 of them", "80%", 80, 0);

        CreateAchievement("Room13 Completed", "Get the Book from Room 13", "13", 50, 0);
        CreateAchievement("Room14 Completed", "Get the Book from Room 14", "14", 50, 0);
        CreateAchievement("Room15 Completed", "Get the Book from Room 15", "15", 50, 0);

        CreateAchievement("100 Percent Books Earned", "Earn 100 percent of the books, or 15 of them", "100%", 100, 0);
    }

    void CloseMenu()
    {
        thisMenu.SetActive(false);
    }

    /// <summary>
    /// Function to attempt to earn an Achievement
    /// </summary>
    /// <param name="title">The name of the Achievemnt to be unlocked</param>
    public void EarnAchievement(string title)
    {
        //If the function EarnAchievement() inside the achievement class returns true, then show the achievement on screen because it is unlocked
        if(achievementDictionary[title].EarnAchievement())
        {
            GameObject achievement = (GameObject)Instantiate(visualAchievement);

            SetAchievementInfo(visualAchievementParent, achievement, title);
            textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            StartCoroutine(FadeAchievement(achievement));
        }
    }

    /// <summary>
    /// Creates the Achievement and stores it in a UI gameObject
    /// </summary>
    /// <param name="title">The name of the Achievement</param>
    /// <param name="description">The description of the Achievement</param>
    /// <param name="icon">The icon that the Achievement is associated with</param>
    /// <param name="points">The number of points this Achievement will award on unlocking</param>
    /// <param name="progress">How many times you have to do the Achievement before it unlocks</param>
    public void CreateAchievement(string title, string description, string icon, int points, int progress)
    {
        //Instantiate the prefab as GameObject achievement
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        Achievement newAchievement = new Achievement(title, description, icon, false, points, achievement, progress);

        achievementDictionary.Add(title, newAchievement);

        SetAchievementInfo(achievements, achievement, title, progress);
    }

    /// <summary>
    /// Function to set the UI Achievement GameObject's information
    /// </summary>
    /// <param name="parent">The parent GameObject the Achievement should be attached to</param>
    /// <param name="achievement">The UI Achievement GameObject</param>
    /// <param name="title">The name of the Achievement</param>
    /// <param name="progression">The progression of the Achievement</param>
    public void SetAchievementInfo(GameObject parent, GameObject achievement, string title, int progression = 0)
    {
        //Set the achievement as a child of the parent achievements
        achievement.transform.SetParent(parent.transform);
        //Rescale the gameObject
        achievement.transform.localScale = new Vector3(1, 1, 1);

        //If the achievement has progression, set it
        string progress = progression > 0 ? " " + PlayerPrefs.GetInt("Progression" + title) + "/" + progression.ToString() : string.Empty;

        achievement.transform.GetChild(0).GetComponent<Text>().text = title + progress; //title
        achievement.transform.GetChild(1).GetComponent<Text>().text = achievementDictionary[title].Description; //description
        achievement.transform.GetChild(2).GetComponent<Text>().text = achievementDictionary[title].Icon; //icon, still need to implement
        achievement.transform.GetChild(3).GetComponent<Text>().text = achievementDictionary[title].Points.ToString(); //points
    }

    /// <summary>
    /// Function responsible for fading an unlocked Achievement onto the screen and then off again
    /// </summary>
    /// <param name="achievement">The unlocked Achievement in question</param>
    /// <returns>Returns nothing, needed for IEnumerators</returns>
    private IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        float fadeRate = 1.0f / fadeTime;

        int startAlpha = 0;
        int endAlpha = 1;

        for(int i = 0; i < 2; i++)
        {
            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                progress += fadeRate * Time.deltaTime;

                yield return null;
            }

            yield return new WaitForSeconds(2);

            startAlpha = 1;
            endAlpha = 0;
        }

        Destroy(achievement);
    }
}
