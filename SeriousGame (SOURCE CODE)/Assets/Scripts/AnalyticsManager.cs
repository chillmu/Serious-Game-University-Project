using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsManager : MonoBehaviour
{
    public GameObject analyticsEntryPrefab;
    public GameObject analyticsEntryParent;

    public Button closeMenuButton;
    public GameObject thisMenu;
    public GameObject mainUI;
    bool isShowing;

    Color defaultColor;

    public Dictionary<string, GameObject> analyticsDictionary = new Dictionary<string, GameObject>();

    List<Response> responses;

    public GameObject analyticsGraph;

    /// <summary>
    /// Start function, used to call the AddHiragana function to add all of the Hiragana to the menu
    /// </summary>
    private void Start()
    {
        closeMenuButton.onClick.AddListener(delegate { CloseMenu(); });

        SortResponses();

        AddEntry("\u3042", "a");
        AddEntry("\u3044", "i");
        AddEntry("\u3046", "u");
        AddEntry("\u3048", "e");
        AddEntry("\u304A", "o");
        AddEntry("\u304B", "ka");
        AddEntry("\u30AD", "ki");
        AddEntry("\u30AF", "ku");
        AddEntry("\u30B1", "ke");
        AddEntry("\u30B3", "ko");
        AddEntry("\u30B5", "sa");
        AddEntry("\u3057", "shi");
        AddEntry("\u3059", "su");
        AddEntry("\u305B", "se");
        AddEntry("\u305D", "so");
        AddEntry("\u305F", "ta");
        AddEntry("\u3061", "chi");
        AddEntry("\u3064", "tsu");
        AddEntry("\u3066", "te");
        AddEntry("\u3068", "to");
        AddEntry("\u306A", "na");
        AddEntry("\u306B", "ni");
        AddEntry("\u306C", "nu");
        AddEntry("\u306D", "ne");
        AddEntry("\u306E", "no");
        AddEntry("\u306F", "ha");
        AddEntry("\u3072", "hi");
        AddEntry("\u3075", "fu");
        AddEntry("\u3078", "he");
        AddEntry("\u307B", "ho");
        AddEntry("\u307E", "ma");
        AddEntry("\u307F", "mi");
        AddEntry("\u3080", "mu");
        AddEntry("\u3081", "me");
        AddEntry("\u3082", "mo");
        AddEntry("\u3084", "ya");
        AddEntry("\u3086", "yu");
        AddEntry("\u3088", "yo");
        AddEntry("\u3089", "ra");
        AddEntry("\u308A", "ri");
        AddEntry("\u308B", "ru");
        AddEntry("\u308C", "re");
        AddEntry("\u308D", "ro");
        AddEntry("\u308F", "wa");
        AddEntry("\u3092", "wo");
        AddEntry("\u3093", "n");
    }

    void SortResponses()
    {
        //Need to process the entire list of Responses from the ResponseManager
        //List of every response
        List<Response> responses = ResponseManager.Instance.ReturnList();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateEntries();
        }
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
    void AddEntry(string character, string sound)
    {
        GameObject entry = (GameObject)Instantiate(analyticsEntryPrefab);

        defaultColor = entry.GetComponent<Image>().color;

        analyticsDictionary.Add(sound, entry);

        entry.transform.SetParent(analyticsEntryParent.transform);

        entry.transform.localScale = new Vector3(1, 1, 1);

        entry.transform.GetChild(0).GetComponent<Text>().text = character;
        entry.transform.GetChild(1).GetComponent<Text>().text = sound;

        Button viewGraphButton = entry.transform.GetChild(2).GetChild(0).GetComponent<Button>();

        viewGraphButton.onClick.AddListener(delegate { ViewGraph(viewGraphButton); });
    }

    void UpdateEntries()
    {
        foreach(KeyValuePair<string, GameObject> entry in analyticsDictionary)
        {
            int correct = PlayerPrefs.GetInt("Correct " + entry.Key);
            int incorrect = PlayerPrefs.GetInt("Incorrect " + entry.Key);
            int total = correct + incorrect;

            int threshold = 2; //currently set to small amount for testing, set it to 5 or something later
            int difference = correct - incorrect;

            if ((correct == 0 && incorrect == 0) || total == 0)
            {
                entry.Value.GetComponent<Image>().color = defaultColor;
            }
            else
            {
                if (difference > threshold) //if the difference is positive, greater than theshold
                {
                    entry.Value.GetComponent<Image>().color = new Color(0, 1, 0, 0.4f);
                }
                else
                if (Mathf.Abs(difference) <= threshold) //difference is within 5
                {
                    entry.Value.GetComponent<Image>().color = new Color(1, 0.5f, 0, 0.4f);

                }
                else
                if (difference < -threshold) //difference is negative, less than -threshold
                {
                    entry.Value.GetComponent<Image>().color = new Color(1, 0, 0, 0.4f);
                }
            }
        }
    }

    void ViewGraph(Button button)
    {
        //mainUI.SetActive(false);
        //thisMenu.SetActive(false);

        int viewGraphAmount = PlayerPrefs.GetInt("ViewGraphAmount");
        viewGraphAmount += 1;
        PlayerPrefs.SetInt("ViewGraphAmount", viewGraphAmount);
        PlayerPrefs.Save();

        string sound = button.transform.parent.parent.GetChild(1).GetComponent<Text>().text;

        List<Response> workingResponses = new List<Response>();

        responses = ResponseManager.Instance.responses;

        //Get this list of responses based on the analytical character that was clicked
        foreach(Response response in responses)
        {
            if(response.GetResponseSound() == sound)
            {
                workingResponses.Add(response);
            }
        }

        //This is where we need to send these responses to the graph script so that it can use it to build that graph
        analyticsGraph.transform.GetChild(0).gameObject.SetActive(true);
        analyticsGraph.GetComponentInChildren<WindowGraph>().ProcessResponses(workingResponses);
    }
}

