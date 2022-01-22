using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiraganaDictionaryManager : MonoBehaviour
{
    public GameObject hiraganaDictionaryEntryPrefab;
    public GameObject hiraganaDictionaryEntryParent;

    public Button closeMenuButton;
    public GameObject thisMenu;

    /// <summary>
    /// Start function, used to call the AddHiragana function to add all of the Hiragana to the menu
    /// </summary>
    private void Start()
    {
        closeMenuButton.onClick.AddListener(delegate { CloseMenu(); });

        AddHiragana("\u3042", "a");
        AddHiragana("\u3044", "i");
        AddHiragana("\u3046", "u");
        AddHiragana("\u3048", "e");
        AddHiragana("\u304A", "o");
        AddHiragana("\u304B", "ka");
        AddHiragana("\u30AD", "ki");
        AddHiragana("\u30AF", "ku");
        AddHiragana("\u30B1", "ke");
        AddHiragana("\u30B3", "ko");
        AddHiragana("\u30B5", "sa");
        AddHiragana("\u3057", "shi");
        AddHiragana("\u3059", "su");
        AddHiragana("\u305B", "se");
        AddHiragana("\u305D", "so");
        AddHiragana("\u305F", "ta");
        AddHiragana("\u3061", "chi");
        AddHiragana("\u3064", "tsu");
        AddHiragana("\u3066", "te");
        AddHiragana("\u3068", "to");
        AddHiragana("\u306A", "na");
        AddHiragana("\u306B", "ni");
        AddHiragana("\u306C", "nu");
        AddHiragana("\u306D", "ne");
        AddHiragana("\u306E", "no");
        AddHiragana("\u306F", "ha");
        AddHiragana("\u3072", "hi");
        AddHiragana("\u3075", "fu");
        AddHiragana("\u3078", "he");
        AddHiragana("\u307B", "ho");
        AddHiragana("\u307E", "ma");
        AddHiragana("\u307F", "mi");
        AddHiragana("\u3080", "mu");
        AddHiragana("\u3081", "me");
        AddHiragana("\u3082", "mo");
        AddHiragana("\u3084", "ya");
        AddHiragana("\u3086", "yu");
        AddHiragana("\u3088", "yo");
        AddHiragana("\u3089", "ra");
        AddHiragana("\u308A", "ri");
        AddHiragana("\u308B", "ru");
        AddHiragana("\u308C", "re");
        AddHiragana("\u308D", "ro");
        AddHiragana("\u308F", "wa");
        AddHiragana("\u3092", "wo");
        AddHiragana("\u3093", "n");
    }

    void CloseMenu()
    {
        thisMenu.SetActive(false);
    }

    /// <summary>
    /// Adds the Hiragana cards to the menu, sets the information and adds the event listener on clicking the button
    /// </summary>
    /// <param name="character">The Japanese Hiragana character</param>
    /// <param name="sound">The sounded pronunciation of the character</param>
    void AddHiragana(string character, string sound)
    {
        GameObject entry = (GameObject)Instantiate(hiraganaDictionaryEntryPrefab);

        entry.transform.SetParent(hiraganaDictionaryEntryParent.transform);

        entry.transform.localScale = new Vector3(1, 1, 1);

        entry.transform.GetChild(0).GetComponent<Text>().text = character;
        entry.transform.GetChild(1).GetComponent<Text>().text = sound;

        Button button = entry.transform.GetChild(2).GetComponent<Button>();

        button.onClick.AddListener(delegate { PlaySound(button); });
    }

    /// <summary>
    /// Plays the audible pronunication of the character
    /// </summary>
    /// <param name="button">The button that was clicked, used to determine which file to play</param>
    void PlaySound(Button button)
    {
        int playSoundAmount = PlayerPrefs.GetInt("PlaySoundAmount");
        playSoundAmount += 1;
        PlayerPrefs.SetInt("PlaySoundAmount", playSoundAmount);
        PlayerPrefs.Save();

        string sound = button.transform.parent.transform.GetChild(1).GetComponent<Text>().text;
        string path = "Audio/" + sound;

        AudioSource audioSource = button.gameObject.AddComponent<AudioSource>();
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        audioSource.PlayOneShot(audioClip);
    }
}
