using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button aboutButton;
    public Button closeButton;
    public Button tutorialButton;
    public Button joinDiscordButton;

    public GameObject warning;
    public GameObject participantIDField;
    public Text idText;
    public GameObject participantGameObject;
    public GameObject aboutPanel;

    private void Start()
    {
        startButton.onClick.AddListener(delegate { StartGame(); });
        aboutButton.onClick.AddListener(delegate { ShowAboutPanel(); });
        closeButton.onClick.AddListener(delegate { CloseAboutPanel(); });
        tutorialButton.onClick.AddListener(delegate { StartTutorial(); });
        joinDiscordButton.onClick.AddListener(delegate { OpenLink(); });

        string theID = PlayerPrefs.GetString("ParticipantID");
        participantGameObject.GetComponent<InputField>().text = theID;

        if (PlayerPrefs.GetString("TutorialViewed") == "true")
        {
            tutorialButton.gameObject.SetActive(false);
        }

        if(PlayerPrefs.GetString("TutorialViewed") != "true")
        {
            startButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (idText.text == string.Empty || idText.text == null)
        {
            startButton.interactable = false;
            tutorialButton.interactable = false;
        }
        else
        {
            startButton.interactable = true;
            tutorialButton.interactable = true;
        }
    }

    void OpenLink()
    {
        string link = "https://discord.com/invite/UUywFTd";

        Application.OpenURL(link);
    }

    void StartGame()
    {
        if(idText.text != string.Empty || idText.text != null)
        {
            PlayerPrefs.SetString("ParticipantID", idText.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        }
        else
        {
            warning.GetComponent<Text>().color = Color.red;
            warning.GetComponent<Text>().text = "Please input your participant ID before attempting to start the game";     
        }
    }
    
    void ShowAboutPanel()
    {
        aboutPanel.gameObject.SetActive(true);
    }

    void CloseAboutPanel()
    {
        aboutPanel.gameObject.SetActive(false);
    }

    void StartTutorial()
    {
        if (idText.text != string.Empty || idText.text != null)
        {
            PlayerPrefs.SetString("ParticipantID", idText.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("UITutorial", LoadSceneMode.Single);
        }
        else
        {
            warning.GetComponent<Text>().color = Color.red;
            warning.GetComponent<Text>().text = "Please input your participant ID before attempting to start the game";
        }
    }
}
