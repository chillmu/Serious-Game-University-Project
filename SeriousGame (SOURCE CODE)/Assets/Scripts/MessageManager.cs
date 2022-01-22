using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public string[] messages; //list of messages to be displayed
    bool playingMessages; //If the messages are playing or not
    public TMPro.TextMeshProUGUI messageText; //The UI TextMeshPro where the messages will be displayed
    public GameObject textContainer; //The UI text container
    string[] characters = new string[] {"あ", "い", "う", "え", "お", "か", "キ", "ク", "ケ", "コ", "サ", "し", "す", "せ", "そ", "た", "ち", "つ", "て", "と", "な", "に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ", "ま", "み", "む", "め", "も", "や", "ゆ", "よ", "ら", "り", "る", "れ", "ろ", "わ", "を", "ん"};

    int index = 0;
    int count = 0;

    private void Start()
    {
        count = messages.Length;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && playingMessages == true)
        {
            index++;
            
            if(index < count)
            {
                UpdateMessages(index);
            }
            else
            {
                StopMessages();
            }
        }
    }

    /// <summary>
    /// If there is a collision with this GameObject and another, start playing the messages
    /// </summary>
    /// <param name="collision">The collision which happened</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(playingMessages == true)
            {
                return;
            }
            else
            {
                StartMessages();
            }
        }
        else
        {
            return;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StopMessages();
        }
    }

    /// <summary>
    /// Plays the list of messages with a few second between each message
    /// </summary>
    /// <returns>Returns nothing, needed for IEnumerators</returns>
    void StartMessages()
    {
        textContainer.SetActive(true);
        playingMessages = true;

        messageText.text = messages[index];

        foreach(string s in characters)
        {
            if(messages[index].Contains(s))
            {
                string sound = messages[index];
                char[] chars = new char[] { ' ', '=', s.ToCharArray()[0] };
                sound = new string(sound.Where(c => !chars.Contains(c)).ToArray());

                PlaySound(sound.ToString());
            }
        }
    }

    void UpdateMessages(int index)
    {
        messageText.text = messages[index];

        foreach(string s in characters)
        {
            if(messages[index].Contains(s))
            {
                string sound = messages[index];
                char[] chars = new char[] { ' ', '=', s.ToCharArray()[0] };
                sound = new string(sound.Where(c => !chars.Contains(c)).ToArray());

                PlaySound(sound.ToString());
            }
        }

        if(index == messages.Length - 1)
        {
            int amount = PlayerPrefs.GetInt("SignPostTimes");
            amount += 1;
            PlayerPrefs.SetInt("SignPostTimes", amount);
            PlayerPrefs.Save();
        }
    }

    void StopMessages()
    {
        index = 0;
        playingMessages = false;
        messageText.text = "";
        textContainer.SetActive(false);
    }

    void PlaySound(string sound)
    {
        string path = "Audio/" + sound;

        if(gameObject.GetComponent<AudioSource>())
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            audioSource.PlayOneShot(audioClip);
        }
    }
}
