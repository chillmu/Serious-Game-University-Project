using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{
    private string name; //Achievement name
    private string description; //Achievement description
    private bool unlocked; //Achievement bool unlocked status
    private int points; //Achievement points
    private string icon; //Achievement icon
    private GameObject achievementRef; //Achievement UI GameObject reference
    private int currentProgression; //Achievement current progression
    private int maxProgression; //Achievement max progression

    /// <summary>
    /// Constructor for an Achievement object
    /// </summary>
    /// <param name="name">The name of the Achievement</param>
    /// <param name="description">The description of the Achievement</param>
    /// <param name="icon">The Hiragana character icon of the Achievement</param>
    /// <param name="unlocked">The bool value of whether or not the Achievement is unlocked or not</param>
    /// <param name="points">The number of points the Achievement is worth</param>
    /// <param name="achievementRef">The reference to the UI Achievement gameObject this Achievement represents</param>
    /// <param name="maxProgression">The progression value of the Achievement, how many times you have to compelete the Achievement's task before the Achievement is awarded/unlocked</param>
    public Achievement(string name, string description, string icon, bool unlocked, int points, GameObject achievementRef, int maxProgression)
    {
        this.Name = name;
        this.Description = description;
        this.Icon = icon;
        this.Unlocked = false;
        this.Points = points;
        this.achievementRef = achievementRef;
        this.maxProgression = maxProgression;
        LoadAchievement();
    }

    //Property for the name parameter of this Achievement
    public string Name { get => name; set => name = value; }

    //Property for the description of this Achievement
    public string Description { get => description; set => description = value; }
    
    //Property for the unlocked bool of this Achievement
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    
    //Property for the points of this Achievement
    public int Points { get => points; set => points = value; }
    
    //Property for the icon of this Achievement
    public string Icon { get => icon; set => icon = value; }

    /// <summary>
    /// Tries to earn the Achievement
    /// </summary>
    /// <returns>Returns true if the achievement is unlocked or false if not</returns>
    public bool EarnAchievement()
    {
        if(!Unlocked && CheckProgress())
        {
            achievementRef.GetComponent<Image>().color = new Color32(0, 255, 21, 255);
            SaveAchievement(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Saves the Achievement state into PlayerPrefs
    /// </summary>
    /// <param name="value">Should the Achievement be unlocked or not</param>
    public void SaveAchievement(bool value)
    {
        unlocked = value;

        int tempPoints = PlayerPrefs.GetInt("Points");

        if(unlocked)
        {
            //Stores the amount of points
            PlayerPrefs.SetInt("Points", tempPoints += points);
        }

        PlayerPrefs.SetInt("Progression" + name, currentProgression);

        if(value == true)
        {
            PlayerPrefs.SetInt(name, 1);
        }
        else
        {
            PlayerPrefs.SetInt(name, 0);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load the Achievement from its state in PlayerPrefs
    /// </summary>
    public void LoadAchievement()
    {
        if(PlayerPrefs.GetInt(name) == 1)
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }

        if(unlocked)
        {
            AchievementManager.Instance.textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            currentProgression = PlayerPrefs.GetInt("Progression" + name);
            achievementRef.GetComponent<Image>().color = new Color32(0, 255, 21, 255);
        }
    }

    /// <summary>
    /// Checks the progress of the progression of the Achievement
    /// </summary>
    /// <returns>Returns true if the Achievement is fully progressed, false if not</returns>
    public bool CheckProgress()
    {
        currentProgression++;

        if(maxProgression != 0)
        {
            achievementRef.transform.GetChild(0).GetComponent<Text>().text = name + " " + currentProgression + "/" + maxProgression;
        }
        
        SaveAchievement(false);

        if(maxProgression == 0)
        {
            return true;
        }
        
        if(currentProgression >= maxProgression)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Function to reset the current progression of the Achievement, in the case of Achievements whos progress can be reset
    /// </summary>
    public void ResetProgress()
    {
        currentProgression = 0;
    }
}
