using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum EncounterState { START, PLAYERTURN, ENEMYTURN, WON, LOST, END }

public class EncounterManager : MonoBehaviour
{
    public GameObject playerPrefab; //The player
    public GameObject enemyPrefab; //The enemy

    public Transform playerBattleStation; //The position the player will be instantiated at
    public Transform enemyBattleStation; //The position the enemy will be instantiated at

    public GameObject mnemonicsPanel;
    GameObject mnemonicsImage2;
    GameObject mnemonicsText;

    string roomToReturnTo;

    public Button nextRoundButton;

    EnemyData enemyData; //The stats of the enemy
    PlayerData playerData; //The stats of the player

    //public PlayerHUD playerHUD; //Used to set the player's HUD
    //public EnemyHUD enemyHUD; //Used to set the enemie's HUD

    public Text questionText; //The question field

    public GameObject answerButtonPrefab; //The answer button prefab
    public GameObject answerButtonParent; //The parent game object the buttons will be children of

    Color defaultButtonColor; //The default color of the answer buttons

    private List<Button> answerButtons; //the list of answer buttons

    private HiraganaList hiraganaList; //The list of Hiragana
    public Hiragana currentHiragana; //The current Hiragana that is in the encounter
    public Hiragana previousHiragana; //The previous current Hiragana

    public EncounterState encounterState; //The current state of the encounter

    public List<Hiragana> workingHiraganaList; //Current list of Hiragana based on the player's level

    DateTime dateTimeStart;
    DateTime dateTimeEnd;

    /// <summary>
    /// Start function, called after Awake, this starts the initialization process of the encounter
    /// </summary>
    private void Start()
    {
        mnemonicsImage2 = mnemonicsPanel.transform.GetChild(0).gameObject;
        mnemonicsText = mnemonicsPanel.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;

        nextRoundButton.onClick.AddListener(delegate { NextRound(); });

        /* use this to reset the correct and incorrect total
        PlayerPrefs.SetInt("CorrectTotal", 0);
        PlayerPrefs.SetInt("IncorrectTotal", 0);
        PlayerPrefs.Save();
        */

        currentHiragana = null;
        //Get the list of Hiragana from the script that generates the list attached to this gameObject
        hiraganaList = gameObject.GetComponent<HiraganaList>();
        hiraganaList.GenerateList();
        //Set the state of the encounter to start
        encounterState = EncounterState.START;
        //Call the StartEncounter() function
        StartEncounter();
    }

    /// <summary>
    /// The rest of the initalization process, needed to be split into seprate methods because certain things have to be initialized before others
    /// </summary>
    void StartEncounter()
    {
        //Instance the player at the player's encounter spawn position 
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        //Need to disable the encounter generator and player movement script on the player during the encounter scene
        playerGO.GetComponent<EncounterGenerator>().enabled = false;
        playerGO.GetComponent<PlayerMovement>().enabled = false;
        playerGO.GetComponent<PlayerCamera>().enabled = false;
        //Player scale is automatically set to the parent's scale, so we need to rescale the player based on the parent scale
        //playerGO.transform.localScale = new Vector3(1 / playerBattleStation.transform.localScale.x, 1 / playerBattleStation.transform.localScale.y, 1 / playerBattleStation.transform.localScale.z); //set the scale of the child to be 1:1 based on the scale of the parent (1/parent)
        //Get the Unit script from the player prefab that contains the player's stats/info
        playerData = playerGO.GetComponent<PlayerData>();
        playerData.LoadData();

        roomToReturnTo = playerData.playerReturnSceneName;
        
        //same as above but with the enemy
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyGO.transform.localScale = new Vector3(1 / enemyBattleStation.transform.localScale.x, 1 / enemyBattleStation.transform.localScale.y, 1 / enemyBattleStation.transform.localScale.z);
        enemyData = enemyGO.GetComponent<EnemyData>();

        //Set the UI values to reflect the information of each Unit
        //playerHUD.SetHUD(playerData);
        //enemyHUD.SetHUD(enemyData);

        //previousHiragana = playerData.previousHiragana;
        string previousHiraganaCharacter = PlayerPrefs.GetString("PreviousHiraganaCharacter");
        string previousHiraganaSound = PlayerPrefs.GetString("PreviousHiraganaSound");
        string previousHiraganaColumn = PlayerPrefs.GetString("PreviousHiraganaColumn");
        int previousHiraganaLevel = PlayerPrefs.GetInt("PreviousHiraganaLevel");

        //If the Hiragana is related to this level's Hiragana, instance it and set it to the previous Hiragana so that you can't come across it multiple times in a row, otherwise set it to null
        if(playerData.playerLevel.Contains(previousHiraganaLevel))
        {
            previousHiragana = new Hiragana(previousHiraganaCharacter, previousHiraganaSound, previousHiraganaColumn, previousHiraganaLevel);
        }
        else
        {
            previousHiragana = null;
        }

        //Start the Hiragana setup
        SetUpHiragana();
    }

    void NextRound()
    {
        nextRoundButton.gameObject.transform.parent.gameObject.SetActive(false);
        SetUpHiragana();
    }

    /// <summary>
    /// Determines an appropriate Hiragana character, based on the previous Hiragana if there was one and the Room level the player entered
    /// </summary>
    /// <returns>Returns nothing, only needed for IEnumerators</returns>
    void SetUpHiragana()
    {
        mnemonicsImage2.GetComponent<Image>().sprite = null;
        mnemonicsText.GetComponent<Text>().text = null;

        mnemonicsPanel.SetActive(true);

        //Let the user know that they have encountered an enemy
        questionText.text = "An enemy has appeared...";

        workingHiraganaList = new List<Hiragana>(); //The list of hiragana where the hiragana's level is less than or equal to Players Unit level, based on the Room's level they enter
        List<Hiragana> completeHiraganaList = new List<Hiragana>(); //The entire complete list of Hiragana

        completeHiraganaList = hiraganaList.GetList(); //Get the entire list of Hiragana

        //Add to the working list, the hiragana that are appropriate to the room difficulty the player entered
        foreach(Hiragana hiragana in completeHiraganaList)
        {
            if(playerData.playerLevel.Contains(hiragana.GetLevel()))
            {
                workingHiraganaList.Add(hiragana);
            }
        }

        /*
        if(roomToReturnTo == "Room8" || roomToReturnTo == "Room10")
        {
            workingHiraganaList.Add(workingHiraganaList[0]);
        }
        */

        workingHiraganaList = ConfigureAdaptiveList(workingHiraganaList);

        //Keep going until currentHiragana has been chosen
        while(currentHiragana == null)
        {
            //select a random Hiragana from the working list within its range
            int index = UnityEngine.Random.Range(0, workingHiraganaList.Count);

            if(previousHiragana == null)
            {
                currentHiragana = workingHiraganaList[index]; //Set the current Hiragana to a random Hiragana from the working list
            }
            else
            if(previousHiragana != null)
            {
                if(workingHiraganaList[index].GetCharacter() != previousHiragana.GetCharacter())
                {
                    currentHiragana = workingHiraganaList[index];
                }
            }
        }

        //this is actually where we need to configure the mnemonics
        ConfigureMnemonics();

        int correctTotal = 0;
        int incorrectTotal = 0;

        List<Response> responses = ResponseManager.Instance.responses;

        //loop through and determine correct and incorrect total related to current hiragana to configure how many buttons are needed
        foreach(Response response in responses)
        {
            if(response.GetResponseSound() == currentHiragana.GetSound())
            {
                if(response.GetResponseOutcome() == "Correct")
                {
                    correctTotal++;
                }
                else
                {
                    incorrectTotal++;
                }
            }
        }

        int difference = correctTotal - incorrectTotal;
        int numberOfButtons;

        if(difference <= -2)
        {
            numberOfButtons = 2;
        }
        else
        if (difference < 0 && difference > -2)
        {
            numberOfButtons = 3;
        }
        else
        {
            numberOfButtons = 4;
        }

        if(roomToReturnTo == "Room8" || roomToReturnTo == "Room10")
        {
            if(numberOfButtons == 4)
            {
                Debug.Log("Number of buttons has been altered");
                numberOfButtons = 3;
            } 
        }

        if(numberOfButtons == 4)
        {
            answerButtonParent.transform.GetChild(0).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(1).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(2).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        if (numberOfButtons == 3)
        {
            answerButtonParent.transform.GetChild(0).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(1).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(2).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        if (numberOfButtons == 2)
        {
            answerButtonParent.transform.GetChild(0).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(1).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(2).gameObject.SetActive(false);
            answerButtonParent.transform.GetChild(3).gameObject.SetActive(false);
        }

        answerButtons = new List<Button>();

        foreach(Transform child in answerButtonParent.transform)
        {
            if(child.gameObject.activeSelf)
            {
                answerButtons.Add(child.GetComponent<Button>());
            }
        }

        defaultButtonColor = answerButtons[0].GetComponent<Image>().color; //The default button color

        //Set the encounterState to playerturn
        encounterState = EncounterState.PLAYERTURN;

        //Start the player's turn
        PlayerTurn();
    }

    void ConfigureMnemonics()
    {
        Sprite image2 = null;
        string text = null;

        List<GameObject> mnemonicsList = MnemonicsManager.Instance.mnemonicsList;
        List<Response> responses = ResponseManager.Instance.responses;
        List<Response> workingResponses = new List<Response>();

        foreach (GameObject o in mnemonicsList)
        {
            string character = o.transform.GetChild(1).GetComponent<Text>().text;

            if (character == currentHiragana.GetSound())
            {
                image2 = o.transform.GetChild(3).GetComponent<Image>().sprite;
                text = o.transform.GetChild(4).GetComponent<Text>().text;
            }
        }

        DateTime currentDate = DateTime.Now;

        foreach (Response response in responses)
        {
            string responseSound = response.GetResponseSound();
            DateTime responseDate = DateTime.Parse(response.GetResponseDate());
            int days = (int)(currentDate - responseDate).TotalDays;
            days = Mathf.Abs(days); //Absolute days between the current date and the response date

            //if the current response is relevant to the current list of hiragana and it is within 28 days of the current date
            if (currentHiragana.GetSound() == responseSound && days <= 28)
            {
                workingResponses.Add(response);
            }
        }

        int correct = 0;
        int incorrect = 0;

        foreach(Response response in workingResponses)
        {
            if(response.GetResponseOutcome() == "Correct")
            {
                correct++;
            }
            else
            {
                incorrect++;
            }
        }

        int difference = correct - incorrect;
        bool negative;

        if(difference > 0)
        {
            negative = false;
        }
        else
        {
            negative = true;
        }

        if(negative) //The amount is negative, therefore not doing so well
        {
            int amount = Mathf.Abs(difference);

            if (amount <= 1) //not doing that bad
            {
                mnemonicsImage2.GetComponent<Image>().sprite = null;
                mnemonicsText.GetComponent<Text>().text = null;
            }
            else
            if (amount > 1 && amount <= 2)
            {
                mnemonicsImage2.GetComponent<Image>().sprite = image2;
                mnemonicsText.GetComponent<Text>().text = null;
            }
            else
            if (amount > 2)
            {
                mnemonicsImage2.GetComponent<Image>().sprite = image2;
                mnemonicsText.GetComponent<Text>().text = text;
            }

            mnemonicsPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Alter the working Hiragana list by adding more instances of incorrectly answered questions
    /// </summary>
    /// <param name="list">the supplied list</param>
    /// <returns>the adapted altered list</returns>
    List<Hiragana> ConfigureAdaptiveList(List<Hiragana> list)
    {
        List<Hiragana> adaptiveList = new List<Hiragana>();
        List<Response> responses = ResponseManager.Instance.responses;
        List<Response> workingResponses = new List<Response>();

        DateTime currentDate = DateTime.Now;

        foreach(Hiragana hiragana in list)
        {
            string hiraganaSound = hiragana.GetSound();

            foreach(Response response in responses)
            {
                string responseSound = response.GetResponseSound();
                DateTime responseDate = DateTime.Parse(response.GetResponseDate());
                int days = (int)(currentDate - responseDate).TotalDays;
                days = Mathf.Abs(days); //Absolute days between the current date and the response date
                
                //if the current response is relevant to the current list of hiragana and it is within 28 days of the current date
                if(hiraganaSound == responseSound && days <= 28)
                {
                    workingResponses.Add(response);
                }
            }
        }

        foreach(Hiragana hiragana in list)
        {
            string hiraganaSound = hiragana.GetSound();
            int correct = 0;
            int incorrect = 0;

            foreach(Response response in workingResponses)
            {
                string responseSound = response.GetResponseSound();

                if(hiraganaSound == responseSound)
                {
                    if(response.GetResponseOutcome() == "Correct")
                    {
                        correct++;
                    }
                    else
                    {
                        incorrect++;
                    }
                }
            }

            int difference = correct - incorrect;
            bool negative;

            if(difference > 0)
            {
                negative = false;
            }
            else
            {
                negative = true;
            }

            if(negative)
            {
                int amount = Mathf.Abs(difference);

                for(int i = 0; i <= amount; i++)
                {
                    adaptiveList.Add(hiragana);
                }
            }
            else
            {
                adaptiveList.Add(hiragana);
            }
        }

        return adaptiveList;
    }

    /// <summary>
    /// The player's turn, allows the clicking of answer buttons
    /// </summary>
    private void PlayerTurn()
    {
        //Set the Enemy TextMesh component text value to be that of the current Hiragana's Japanese character
        enemyData.gameObject.GetComponent<TextMesh>().text = currentHiragana.GetCharacter();

        //Shuffle the buttons, select the correct answer and select false answers and store the answers as the text value of the answer buttons
        InitialiseAnswerButtonList();

        questionText.text = "What sound is this character";
        
        foreach(Button b in answerButtons)
        {
            b.interactable = true;
        }

        dateTimeStart = DateTime.Now;
    }

    /// <summary>
    /// Called when a player clicks one of the answer buttons, goes on to check if the answer that was selected is the correct answer or not
    /// </summary>
    /// <param name="button">The UI button that was clicked</param>
    public void OnClickAnswerButton(Button button)
    {
        dateTimeEnd = DateTime.Now;

        foreach(Button b in answerButtons)
        {
            b.interactable = false;
        }

        //if it is not the player's turn, then this should not be happening,
        //so return from this function at that point
        if(encounterState != EncounterState.PLAYERTURN)
        {
            return;
        }
        else
        {
            //Check to see if the answer the player selected is the correct answer for the current Hiragana that is displayed
            StartCoroutine(CheckAnswer(button));
        }
    }

    /// <summary>
    /// Function to determine if the chosen answer is the true answer or not
    /// </summary>
    /// <param name="button">The UI button that was selected</param>
    /// <returns>Returns nothing, needed for IEnumerators</returns>
    public IEnumerator CheckAnswer(Button button)
    {
        string chosenAnswer = button.GetComponentInChildren<Text>().text;

        if(chosenAnswer == currentHiragana.GetSound())
        {
            //The chosen answer is correct, need to damage the enemy and then the player can take another turn
            questionText.text = "Correct answer!";
            button.GetComponent<Image>().color = Color.green;

            string sound = currentHiragana.GetSound().ToUpper();
            string achievementName = sound + " Mastery";

            AchievementManager.Instance.EarnAchievement(achievementName);

            //Wait for a few seconds before the rest of the corountine is executed
            yield return new WaitForSeconds(2f);

            //Set the button back to its original color
            button.GetComponent<Image>().color = defaultButtonColor;

            questionText.text = "The pronunciation is...";

            yield return new WaitForSeconds(1f);

            //Play the audio clip that pronounces the sound of the character
            AudioSource audioSource = button.gameObject.AddComponent<AudioSource>();
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + currentHiragana.GetSound());
            audioSource.PlayOneShot(audioClip);

            yield return new WaitForSeconds(2f);

            TimeSpan responseTime = dateTimeEnd - dateTimeStart;
            string secondsToRespond = responseTime.Seconds.ToString();

            //Access ResponseManager to add a new response
            ResponseManager.Instance.AddResponse(new Response(currentHiragana.GetSound(), System.DateTime.Now.ToString(), "Correct", secondsToRespond.ToString(), roomToReturnTo));

            //Update the number of correct times a question has been answered
            int correctTotal = PlayerPrefs.GetInt("CorrectTotal");
            correctTotal += 1;
            PlayerPrefs.SetInt("CorrectTotal", correctTotal);
            PlayerPrefs.Save();

            //Damage the enemy for an amount equal to the playerUnit's damage
            DamageEnemy();
        }
        else
        {
            //the chosen answer is false, it's now the enemies turn
            questionText.text = "Wrong answer";
            button.GetComponent<Image>().color = Color.red;

            //Wait for a few seconds before the rest of the corountine is executed
            yield return new WaitForSeconds(2f);

            questionText.text = "The correct answer was '" + currentHiragana.GetSound() + "'";

            //Set the button back to its original color
            button.GetComponent<Image>().color = defaultButtonColor;

            //Wait for a few seconds before the rest of the corountine is executed
            yield return new WaitForSeconds(2f);

            questionText.text = "The pronunciation is...";

            yield return new WaitForSeconds(1f);

            //Play the audio clip that pronounces the sound of the character
            AudioSource audioSource = button.gameObject.AddComponent<AudioSource>();
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + currentHiragana.GetSound());
            audioSource.PlayOneShot(audioClip);

            //Wait for a few seconds before the rest of the corountine is executed
            yield return new WaitForSeconds(2f);

            TimeSpan responseTime = dateTimeEnd - dateTimeStart;
            string secondsToRespond = responseTime.Seconds.ToString();

            //Access ResponseManager to add a new response
            ResponseManager.Instance.AddResponse(new Response(currentHiragana.GetSound(), System.DateTime.Now.ToString(), "Incorrect", secondsToRespond.ToString(), roomToReturnTo));

            //Update the number of incorrect times a question has been answered
            int incorrectTotal = PlayerPrefs.GetInt("IncorrectTotal");
            incorrectTotal += 1;
            PlayerPrefs.SetInt("IncorrectTotal", incorrectTotal);
            PlayerPrefs.Save();

            //Set the state to the enemy turn
            encounterState = EncounterState.ENEMYTURN;
            EnemyTurn();
        }
    }

    /// <summary>
    /// Function to initialize the answer buttons, shuffes the buttons, selects a correct answer and 3 false answers and then places them as the text value of the buttons,
    /// so it is completely random each time a Hiragana character is encountered
    /// </summary>
    private void InitialiseAnswerButtonList()
    {
        //need to randomise the order of the List so that the correct answer is never in the same button
        answerButtons = ShuffleButtons(answerButtons);

        string trueAnswer = currentHiragana.GetSound(); //The true value of the current Hiragana

        List<string> falseAnswers = new List<string>(); //List of false answers

        string lastFalseAnswer = null;

        //Add false answers to the false answers list until we have as many buttons - 1
        while(falseAnswers.Count != answerButtons.Count - 1)
        {
            //Define a temporary list based on the current working list
            List<Hiragana> tempList = new List<Hiragana>();
            tempList = workingHiraganaList;

            //Select a random index within the range of the list
            int r = UnityEngine.Random.Range(0, tempList.Count);

            //If it is not equal to the true answer, add it to the false answers list,
            //and remove it from the temporary list so it cannot be randomly selected
            //for the other false answers
            if(tempList[r].GetSound() != trueAnswer && tempList[r].GetSound() != lastFalseAnswer && !falseAnswers.Contains(tempList[r].GetSound()))
            {
                falseAnswers.Add(tempList[r].GetSound());
                lastFalseAnswer = tempList[r].GetSound();
                tempList.RemoveAt(r);
            }
        }

        //Store the true answer and false answers in a list
        List<string> allAnswers = new List<string>();
        allAnswers = falseAnswers;
        allAnswers.Add(trueAnswer);

        for(int i = 0; i <= answerButtons.Count - 1; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = allAnswers[i];
        }
    }

    /// <summary>
    /// Shuffles the list of buttons
    /// </summary>
    /// <param name="list">The list to be shuffled</param>
    /// <returns>Returns the shuffled list</returns>
    public List<Button> ShuffleButtons(List<Button> list)
    {
        //a temporary list to be returned by this function
        List<Button> shuffledList = new List<Button>();

        //While the provided list still has indexes, randomly remove them
        //and store them in the list to be returned
        while(list.Count > 0)
        {
            int r = UnityEngine.Random.Range(0, list.Count);
            shuffledList.Add(list[r]);
            list.RemoveAt(r);
        }

        //The fully shuffled list
        return shuffledList;
    }

    /// <summary>
    /// Function to damage the enemy
    /// </summary>
    /// <returns>Returns nothing, needed for IEnumerators</returns>
    public void DamageEnemy()
    {
        //Deal damage to the enemy and determine if they are dead
        bool isDead = enemyData.TakeDamage(playerData.playerDamage);
        //Update the enemy hud with the new health values
        //enemyHUD.SetHP(enemyData.enemyCurrentHP);

        //if the enemy is dead or not
        if(isDead)
        {
            PlayerPrefs.SetString("PreviousHiraganaCharacter", currentHiragana.GetCharacter());
            PlayerPrefs.SetString("PreviousHiraganaSound", currentHiragana.GetSound());
            PlayerPrefs.SetString("PreviousHiraganaColumn", currentHiragana.GetColumn());
            PlayerPrefs.SetInt("PreviousHiraganaLevel", currentHiragana.GetLevel());

            //set the encounter state to won and end the encounter
            encounterState = EncounterState.WON;
            StartCoroutine(EndEncounter());
        }
        else
        {
            //set the current Hiragana to null and restart the round
            previousHiragana = currentHiragana;
            currentHiragana = null;
            //SetUpHiragana();

            //display the next button
            nextRoundButton.gameObject.transform.parent.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Function which completes the enemie's turn
    /// </summary>
    /// <returns>Returns nothing, needed for IEnumerators</returns>
    public void EnemyTurn()
    {
        //Deal damage to the player and check if they are dead
        bool isDead = playerData.TakeDamage(enemyData.enemyDamage);
        //Update the player hud with the new health values
        //playerHUD.SetHP(playerData.playerCurrentHP);

        int number = PlayerPrefs.GetInt("DamageTaken");
        number += 1;
        PlayerPrefs.SetInt("DamageTaken", number);
        PlayerPrefs.Save();

        //if the player is dead or not
        if(isDead)
        {
            PlayerPrefs.SetString("PreviousHiraganaCharacter", currentHiragana.GetCharacter());
            PlayerPrefs.SetString("PreviousHiraganaSound", currentHiragana.GetSound());
            PlayerPrefs.SetString("PreviousHiraganaColumn", currentHiragana.GetColumn());
            PlayerPrefs.SetInt("PreviousHiraganaLevel", currentHiragana.GetLevel());
            PlayerPrefs.Save();

            //set the encounter state to lost and end the encounter
            encounterState = EncounterState.END;
            StartCoroutine(EndEncounter());
        }
        else
        {
            //set the current Hiragana to null, set the encounter state to the player turn
            //and start a new round
            previousHiragana = currentHiragana;
            currentHiragana = null;
            //SetUpHiragana();

            playerData.SaveData();
            encounterState = EncounterState.LOST;


            StartCoroutine(EndEncounter());

            //display the next button
            //nextRoundButton.gameObject.transform.parent.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Function that ends an encounter and determines if it was won or lost
    /// </summary>
    /// <returns>Returns nothing, needed for IEnumerators</returns>
    public IEnumerator EndEncounter()
    {
        //the encounter has been won or lost
        if(encounterState == EncounterState.WON)
        {
            //let the player know they have won the encounter
            questionText.text = "Encounter has been won";

            //Wait for a few seconds
            yield return new WaitForSeconds(2f);

            playerData.SaveData();

            int number = PlayerPrefs.GetInt("TotalInARow");
            number += 1;
            PlayerPrefs.SetInt("TotalInARow", number);

            if(number > PlayerPrefs.GetInt("TotalInARowHighScore"))
            {
                PlayerPrefs.SetInt("TotalInARowHighScore", number);
            }

            PlayerPrefs.Save();

            float seconds = Time.timeSinceLevelLoad;
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            EncounterTimes.Instance.AddEncounterTime(new EncounterTime(time.Seconds.ToString(), playerData.playerReturnSceneName, "WON"));

            //Send the player back to the previous scene
            //For now we will just return to the overworld scene
            SceneManager.LoadScene(playerData.playerReturnSceneName, LoadSceneMode.Single);
        }
        else
        if(encounterState == EncounterState.LOST)
        {
            //let the player know they have won the encounter
            questionText.text = "Encounter has been lost";

            //Wait for a few seconds
            yield return new WaitForSeconds(2f);

            questionText.text = "You have " + playerData.playerCurrentHP + " exhaustion points left";

            yield return new WaitForSeconds(2f);

            playerData.SaveData();

            PlayerPrefs.SetInt("TotalInARow", 0);
            PlayerPrefs.Save();

            float seconds = Time.timeSinceLevelLoad;
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            EncounterTimes.Instance.AddEncounterTime(new EncounterTime(time.Seconds.ToString(), playerData.playerReturnSceneName, "LOST"));

            //Send the player back to the previous scene
            //For now we will just return to the overworld scene
            SceneManager.LoadScene(playerData.playerReturnSceneName, LoadSceneMode.Single);
        }
        else
        if(encounterState == EncounterState.END)
        {
            //Let the player know they have lost the encounter
            questionText.text = "Encounter has ended";

            yield return new WaitForSeconds(3f);

            questionText.text = "You have become too exhausted";

            yield return new WaitForSeconds(2f);

            playerData.SaveData();

            int number = PlayerPrefs.GetInt(playerData.playerReturnSceneName + "Failed");
            number += 1;
            PlayerPrefs.SetInt(playerData.playerReturnSceneName + "Failed", number);

            PlayerPrefs.SetInt("TotalInARow", 0);
            PlayerPrefs.Save();

            float seconds = Time.timeSinceLevelLoad;
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            EncounterTimes.Instance.AddEncounterTime(new EncounterTime(time.Seconds.ToString(), playerData.playerReturnSceneName, "EXHAUSTED"));

            //Restart the game with the player waking up in the starting area
            SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        }
    }
}