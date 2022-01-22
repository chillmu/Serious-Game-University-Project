using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITutorialManager : MonoBehaviour
{
    public Text tutorialText;

    public GameObject mainUICanvas;

    public Button achievementButton;
    public bool achievementButtonClicked;
    public Button closeAchievementButton;
    public bool closeAchievementButtonClicked;

    public Button dictionaryButton;
    public bool dictionaryButtonClicked;
    public Button closeDictionaryButton;
    public bool closeDictionaryButtonClicked;

    public Button analyticsButton;
    public bool analyticsButtonClicked;
    public Button closeAnalyticsButton;
    public bool closeAnalyticsButtonClicked;

    public Button mnemonicsButton;
    public bool mnemonicsButtonClicked;
    public Button closeMnemonicsButton;
    public bool closeMnemonicsButtonClicked;

    public Transform playSoundButtonParent;
    public List<Button> playSoundButtonList;
    public bool playSoundButtonClicked;

    public Transform viewGraphButtonParent;
    public List<Button> viewGraphButtonList;
    public bool viewGraphButtonClicked;

    private void Start()
    {
        AchievementManager.Instance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        MnemonicsManager.Instance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        AchievementManager.Instance.transform.GetChild(0).transform.localScale = new Vector3(1f, 1f, 1f);
        MnemonicsManager.Instance.transform.GetChild(0).transform.localScale = new Vector3(1f, 1f, 1f);

        achievementButtonClicked = false;
        closeAchievementButtonClicked = false;

        dictionaryButtonClicked = false;
        closeDictionaryButtonClicked = false;

        analyticsButtonClicked = false;
        closeAnalyticsButtonClicked = false;

        mnemonicsButtonClicked = false;
        closeMnemonicsButtonClicked = false;

        achievementButton.onClick.AddListener(delegate { StartCoroutine(AchievementButtonClicked()); });
        closeAchievementButton.onClick.AddListener(delegate { StartCoroutine(CloseAchievementsButtonClicked()); });

        dictionaryButton.onClick.AddListener(delegate { StartCoroutine(DictionaryButtonClicked()); });
        closeDictionaryButton.onClick.AddListener(delegate { StartCoroutine(CloseDictionaryButtonClicked()); });

        analyticsButton.onClick.AddListener(delegate { StartCoroutine(AnalyticsButtonClicked()); });
        closeAnalyticsButton.onClick.AddListener(delegate { StartCoroutine(CloseAnalyticsButtonClicked()); });

        mnemonicsButton.onClick.AddListener(delegate { StartCoroutine(MnemonicsButtonClicked()); });
        closeMnemonicsButton.onClick.AddListener(delegate { StartCoroutine(CloseMnemonicsButtonClicked()); });

        playSoundButtonList = new List<Button>();
        playSoundButtonClicked = false;

        foreach(Transform child in playSoundButtonParent)
        {
            Button button = child.gameObject.GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate { StartCoroutine(PlaySoundButtonClicked()); });
            playSoundButtonList.Add(button);
            
        }

        foreach(Transform child in viewGraphButtonParent)
        {
            Button button = child.gameObject.GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate { StartCoroutine(ViewGraphButtonClicked()); });
            viewGraphButtonList.Add(button);
        }

        StartCoroutine(UITutorial());
    }

    IEnumerator PlaySoundButtonClicked()
    {
        playSoundButtonClicked = true;

        yield return new WaitForSeconds(1f);

        playSoundButtonClicked = false;
    }

    IEnumerator ViewGraphButtonClicked()
    {
        viewGraphButtonClicked = true;

        yield return new WaitForSeconds(1f);

        viewGraphButtonClicked = false;
    }

    IEnumerator AchievementButtonClicked()
    {
        achievementButtonClicked = true;

        yield return new WaitForSeconds(1f);

        achievementButtonClicked = false;
    }

    IEnumerator CloseAchievementsButtonClicked()
    {
        closeAchievementButtonClicked = true;

        yield return new WaitForSeconds(1f);

        closeAchievementButtonClicked = false;
    }

    IEnumerator DictionaryButtonClicked()
    {
        dictionaryButtonClicked = true;

        yield return new WaitForSeconds(1f);

        dictionaryButtonClicked = false;
    }

    IEnumerator CloseDictionaryButtonClicked()
    {
        closeDictionaryButtonClicked = true;

        yield return new WaitForSeconds(1f);

        closeDictionaryButtonClicked = false;
    }

    IEnumerator AnalyticsButtonClicked()
    {
        analyticsButtonClicked = true;

        yield return new WaitForSeconds(1f);

        analyticsButtonClicked = false;
    }

    IEnumerator CloseAnalyticsButtonClicked()
    {
        closeAnalyticsButtonClicked = true;

        yield return new WaitForSeconds(1f);

        closeAnalyticsButtonClicked = false;
    }

    IEnumerator MnemonicsButtonClicked()
    {
        mnemonicsButtonClicked = true;

        yield return new WaitForSeconds(1f);

        mnemonicsButtonClicked = false;
    }

    IEnumerator CloseMnemonicsButtonClicked()
    {
        closeMnemonicsButtonClicked = true;

        yield return new WaitForSeconds(1f);

        closeMnemonicsButtonClicked = false;
    }

    IEnumerator UITutorial()
    {
        yield return new WaitForSeconds(8f);

        mainUICanvas.SetActive(true);

        tutorialText.text = "We will start by first opening the achievements menu, try clicking the button labeled 'Achievements'. You can open and close this menu by pressing the Escape key on your keyboard";

        achievementButton.interactable = true;
        dictionaryButton.interactable = false;
        analyticsButton.interactable = false;
        mnemonicsButton.interactable = false;

        yield return new WaitUntil(() => achievementButtonClicked);

        tutorialText.text = "This is the achievement menu, it displays all of the achievements and your progress towards them, close the menu when you are done looking around";

        yield return new WaitUntil(() => closeAchievementButtonClicked);

        mainUICanvas.SetActive(true);

        tutorialText.text = "Alright, now we will check and see what the Dictionary does, try clicking the button labeled 'Hiragana Dictionary'";

        achievementButton.interactable = false;
        dictionaryButton.interactable = true;
        analyticsButton.interactable = false;
        mnemonicsButton.interactable = false;

        yield return new WaitUntil(() => dictionaryButtonClicked);

        tutorialText.text = "This is the dictionary menu, it displays all of the Japanese Hiragana, try clicking one of the buttons labeled 'Play Sound'. close the dictionary menu when you are done";

        yield return new WaitUntil(() => closeDictionaryButtonClicked);

        mainUICanvas.SetActive(true);

        tutorialText.text = "Okay, now we will see what the analytics look like, try clicking the button labeled 'Analytics'";

        achievementButton.interactable = false;
        dictionaryButton.interactable = false;
        analyticsButton.interactable = true;
        mnemonicsButton.interactable = false;

        yield return new WaitUntil(() => analyticsButtonClicked);

        tutorialText.text = "This is the analytics menu, here you can view your history of responses to questions that you answer in the game, try checking out a character's analytics by clicking one of the buttons labeled 'View Graph'. Close the menu when you are done";

        yield return new WaitUntil(() => closeAnalyticsButtonClicked);

        mainUICanvas.SetActive(true);

        tutorialText.text = "Lastly, there is the mnemonics menu, 'mnemonics' are tools which help us learn and remember. Try clicking the button labeled 'Mnemonics'";

        achievementButton.interactable = false;
        dictionaryButton.interactable = false;
        analyticsButton.interactable = false;
        mnemonicsButton.interactable = true;

        yield return new WaitUntil(() => mnemonicsButtonClicked);

        tutorialText.text = "Here you will find useful information that will help you learn the sounds of each Hiragana character. Close the menu when you are done";

        yield return new WaitUntil(() => closeMnemonicsButtonClicked);

        tutorialText.text = "There is one more button but you cannot see it yet. This button will email data that is captured during play, only click this button when you are instructed to...";

        yield return new WaitForSeconds(8f);

        tutorialText.text = "Now you know how to use the tutorial, we will see what an encounter looks like";

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("EncounterTutorial", LoadSceneMode.Single);
    }
}
