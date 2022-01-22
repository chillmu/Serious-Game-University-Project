using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TutorialManager : MonoBehaviour
{
    public GameObject playerPrefab; //The player
    public GameObject enemyPrefab; //The enemy

    public Transform playerBattleStation; //The position the player will be instantiated at
    public Transform enemyBattleStation; //The position the enemy will be instantiated at

    public GameObject mnemonicsPanel;
    GameObject mnemonicsImage2;
    GameObject mnemonicsText;

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

    bool firstPass = true;

    /// <summary>
    /// Start function, called after Awake, this starts the initialization process of the encounter
    /// </summary>
    private void Start()
    {
        answerButtonParent.transform.GetChild(0).gameObject.SetActive(false);
        answerButtonParent.transform.GetChild(1).gameObject.SetActive(false);
        answerButtonParent.transform.GetChild(2).gameObject.SetActive(false);
        answerButtonParent.transform.GetChild(3).gameObject.SetActive(false);

        StartCoroutine(Tutorial());  
    }

    IEnumerator Tutorial()
    {
        questionText.text = "This is the Encounter Tutorial...";
        yield return new WaitForSeconds(4f);

        questionText.text = "When you encounter a Hiragana character you will enter an encounter scene that looks like this...";
        yield return new WaitForSeconds(8f);

        mnemonicsImage2 = mnemonicsPanel.transform.GetChild(0).gameObject;
        mnemonicsText = mnemonicsPanel.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;

        nextRoundButton.onClick.AddListener(delegate { NextRound(); });
        currentHiragana = null;
        hiraganaList = gameObject.GetComponent<HiraganaList>();
        hiraganaList.GenerateList();
        encounterState = EncounterState.START;
        StartCoroutine(StartEncounter());
    }

    /// <summary>
    /// The rest of the initalization process, needed to be split into seprate methods because certain things have to be initialized before others
    /// </summary>
    IEnumerator StartEncounter()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerGO.GetComponent<EncounterGenerator>().enabled = false;
        playerGO.GetComponent<PlayerMovement>().enabled = false;
        playerGO.GetComponent<PlayerCamera>().enabled = false;
        playerData = playerGO.GetComponent<PlayerData>();
        playerData.LoadData();
        playerData.playerLevel = new List<int>() { 1 };
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyGO.transform.localScale = new Vector3(1 / enemyBattleStation.transform.localScale.x, 1 / enemyBattleStation.transform.localScale.y, 1 / enemyBattleStation.transform.localScale.z);
        enemyData = enemyGO.GetComponent<EnemyData>();
        string previousHiraganaCharacter = PlayerPrefs.GetString("PreviousHiraganaCharacter");
        string previousHiraganaSound = PlayerPrefs.GetString("PreviousHiraganaSound");
        string previousHiraganaColumn = PlayerPrefs.GetString("PreviousHiraganaColumn");
        int previousHiraganaLevel = PlayerPrefs.GetInt("PreviousHiraganaLevel");

        if(playerData.playerLevel.Contains(previousHiraganaLevel))
        {
            previousHiragana = new Hiragana(previousHiraganaCharacter, previousHiraganaSound, previousHiraganaColumn, previousHiraganaLevel);
        }
        else
        {
            previousHiragana = null;
        }

        yield return new WaitForSeconds(0f);

        StartCoroutine(SetUpHiragana());
    }

    void NextRound()
    {
        //nextRoundButton.gameObject.transform.parent.gameObject.SetActive(false);
        StartCoroutine(SetUpHiragana());
    }

    /// <summary>
    /// Determines an appropriate Hiragana character, based on the previous Hiragana if there was one and the Room level the player entered
    /// </summary>
    /// <returns>Returns nothing, only needed for IEnumerators</returns>
    IEnumerator SetUpHiragana()
    {
        mnemonicsImage2.GetComponent<Image>().sprite = null;
        mnemonicsText.GetComponent<Text>().text = null;
        mnemonicsPanel.SetActive(true);

        if (firstPass == true)
        {
            questionText.text = "Depending on the room you were in before the encounter, will dictate which Hiragana characters that will show up...";
            yield return new WaitForSeconds(8f);
        }

        workingHiraganaList = new List<Hiragana>(); //The list of hiragana where the hiragana's level is less than or equal to Players Unit level, based on the Room's level they enter
        List<Hiragana> completeHiraganaList = new List<Hiragana>(); //The entire complete list of Hiragana
        completeHiraganaList = hiraganaList.GetList(); //Get the entire list of Hiragana

        //Add to the working list, the hiragana that are appropriate to the room difficulty the player entered
        foreach(Hiragana hiragana in completeHiraganaList)
        {
            if(hiragana.GetLevel() == 1)
            {
                workingHiraganaList.Add(hiragana);
            }
        }

        workingHiraganaList = ConfigureAdaptiveList(workingHiraganaList);

        //Keep going until currentHiragana has been chosen
        while (currentHiragana == null)
        {
            //select a random Hiragana from the working list within its range
            int index = UnityEngine.Random.Range(0, workingHiraganaList.Count);

            if (previousHiragana == null)
            {
                currentHiragana = workingHiraganaList[index]; //Set the current Hiragana to a random Hiragana from the working list
            }
            else
            if (previousHiragana != null)
            {
                if (workingHiraganaList[index].GetCharacter() != previousHiragana.GetCharacter())
                {
                    currentHiragana = workingHiraganaList[index];
                }
            }
        }

        //this is actually where we need to configure the mnemonics
        yield return StartCoroutine(ConfigureMnemonics());

        int correctTotal = 0;
        int incorrectTotal = 0;

        List<Response> responses = ResponseManager.Instance.responses;

        //loop through and determine correct and incorrect total related to current hiragana to configure how many buttons are needed
        foreach (Response response in responses)
        {
            if (response.GetResponseSound() == currentHiragana.GetSound())
            {
                if (response.GetResponseOutcome() == "Correct")
                {
                    correctTotal++;
                }
                else
                {
                    incorrectTotal++;
                }
            }
        }

        if (firstPass == true)
        {
            questionText.text = "Just like the mnemonics, depending on your history of responses to the current Hirgana character";
            yield return new WaitForSeconds(8f);

            questionText.text = "You will either have access to 2";
            answerButtonParent.transform.GetChild(0).gameObject.SetActive(true);
            answerButtonParent.transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            questionText.text = "3";
            answerButtonParent.transform.GetChild(2).gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            questionText.text = "Or 4 Buttons which are clicked to answer the question";
            answerButtonParent.transform.GetChild(3).gameObject.SetActive(true);
            yield return new WaitForSeconds(8f);
        }

        answerButtons = new List<Button>();

        foreach (Transform child in answerButtonParent.transform)
        {
            if (child.gameObject.activeSelf)
            {
                answerButtons.Add(child.GetComponent<Button>());
            }
        }

        defaultButtonColor = answerButtons[0].GetComponent<Image>().color; //The default button color

        //Set the encounterState to playerturn
        encounterState = EncounterState.PLAYERTURN;

        //Start the player's turn
        StartCoroutine(PlayerTurn());
    }

    IEnumerator ConfigureMnemonics()
    {
        Sprite image2 = null;
        string text = null;

        List<GameObject> mnemonicsList = MnemonicsManager.Instance.mnemonicsList;
        List<Response> responses = ResponseManager.Instance.responses;
        List<Response> workingResponses = new List<Response>();

        foreach(GameObject o in mnemonicsList)
        {
            string character = o.transform.GetChild(1).GetComponent<Text>().text;

            if(character == currentHiragana.GetSound())
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

        if(firstPass == true)
        {
            questionText.text = "Depending on your history of responses to the current Hirgana character";
            yield return new WaitForSeconds(8f);

            questionText.text = "You will either have access to 1";
            mnemonicsImage2.GetComponent<Image>().sprite = image2;
            yield return new WaitForSeconds(5f);
            questionText.text = "Or 2 mnemonics";
            mnemonicsText.GetComponent<Text>().text = text;
            yield return new WaitForSeconds(5f);
            questionText.text = "These will aid you in answereing questions and will apaptively increase or decrease";
            yield return new WaitForSeconds(8f);
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

        foreach (Hiragana hiragana in list)
        {
            string hiraganaSound = hiragana.GetSound();

            foreach (Response response in responses)
            {
                string responseSound = response.GetResponseSound();
                DateTime responseDate = DateTime.Parse(response.GetResponseDate());
                int days = (int)(currentDate - responseDate).TotalDays;
                days = Mathf.Abs(days); //Absolute days between the current date and the response date

                //if the current response is relevant to the current list of hiragana and it is within 28 days of the current date
                if (hiraganaSound == responseSound && days <= 28)
                {
                    workingResponses.Add(response);
                }
            }
        }

        foreach (Hiragana hiragana in list)
        {
            string hiraganaSound = hiragana.GetSound();
            int correct = 0;
            int incorrect = 0;

            foreach (Response response in workingResponses)
            {
                string responseSound = response.GetResponseSound();

                if (hiraganaSound == responseSound)
                {
                    if (response.GetResponseOutcome() == "Correct")
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

            if (difference > 0)
            {
                negative = false;
            }
            else
            {
                negative = true;
            }

            if (negative)
            {
                int amount = Mathf.Abs(difference);

                for (int i = 0; i <= amount; i++)
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
    private IEnumerator PlayerTurn()
    {
        //Set the Enemy TextMesh component text value to be that of the current Hiragana's Japanese character
        enemyData.gameObject.GetComponent<TextMesh>().text = currentHiragana.GetCharacter();

        //Shuffle the buttons, select the correct answer and select false answers and store the answers as the text value of the answer buttons
        InitialiseAnswerButtonList();

        if(firstPass == true)
        {
            questionText.text = "At this point, you will be able to choose an answer";
            yield return new WaitForSeconds(8f);
        }

        /*
        foreach(Button b in answerButtons)
        {
            b.interactable = true;
        }
        */

        if(firstPass == true)
        {
            questionText.text = "For now we will select the wrong answer and see what happens";
            yield return new WaitForSeconds(8f);

            Button theButton = null;
            bool gotButton = false;

            foreach (Button b in answerButtons)
            {
                if(b.gameObject.GetComponentInChildren<Text>().text != currentHiragana.GetSound() && gotButton == false)
                {
                    firstPass = false;
                    gotButton = true;
                    theButton = b;
                }
            }

            OnClickAnswerButton(theButton);
        }
        else
        {
            questionText.text = "This time we will select the correct answer and see what happens";
            yield return new WaitForSeconds(8f);

            Button theButton = null;
            bool gotButton = false;

            foreach(Button b in answerButtons)
            {
                if(b.gameObject.GetComponentInChildren<Text>().text == currentHiragana.GetSound() && gotButton == false)
                {
                    gotButton = true;
                    theButton = b;
                }
            }

            OnClickAnswerButton(theButton);
        }
    }

    /// <summary>
    /// Called when a player clicks one of the answer buttons, goes on to check if the answer that was selected is the correct answer or not
    /// </summary>
    /// <param name="button">The UI button that was clicked</param>
    public void OnClickAnswerButton(Button button)
    {
        /*
        foreach (Button b in answerButtons)
        {
            b.interactable = false;
        }
        */

        //if it is not the player's turn, then this should not be happening,
        //so return from this function at that point
        if (encounterState != EncounterState.PLAYERTURN)
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

        if (chosenAnswer == currentHiragana.GetSound())
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

            //Set the state to the enemy turn
            encounterState = EncounterState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
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
        while (falseAnswers.Count != answerButtons.Count - 1)
        {
            //Define a temporary list based on the current working list
            List<Hiragana> tempList = new List<Hiragana>();
            tempList = workingHiraganaList;

            //Select a random index within the range of the list
            int r = UnityEngine.Random.Range(0, tempList.Count);

            //If it is not equal to the true answer, add it to the false answers list,
            //and remove it from the temporary list so it cannot be randomly selected
            //for the other false answers
            if (tempList[r].GetSound() != trueAnswer && tempList[r].GetSound() != lastFalseAnswer && !falseAnswers.Contains(tempList[r].GetSound()))
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

        for (int i = 0; i <= answerButtons.Count - 1; i++)
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
        while (list.Count > 0)
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
        if (isDead)
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
    public IEnumerator EnemyTurn()
    {
        //Deal damage to the player and check if they are dead
        bool isDead = playerData.TakeDamage(enemyData.enemyDamage);
        //Update the player hud with the new health values
        //playerHUD.SetHP(playerData.playerCurrentHP);

        //if the player is dead or not
        if (isDead)
        {
            PlayerPrefs.SetString("PreviousHiraganaCharacter", currentHiragana.GetCharacter());
            PlayerPrefs.SetString("PreviousHiraganaSound", currentHiragana.GetSound());
            PlayerPrefs.SetString("PreviousHiraganaColumn", currentHiragana.GetColumn());
            PlayerPrefs.SetInt("PreviousHiraganaLevel", currentHiragana.GetLevel());

            //set the encounter state to lost and end the encounter
            encounterState = EncounterState.LOST;
            StartCoroutine(EndEncounter());
        }
        else
        {
            //set the current Hiragana to null, set the encounter state to the player turn
            //and start a new round
            previousHiragana = currentHiragana;
            currentHiragana = null;
            encounterState = EncounterState.PLAYERTURN;
            //SetUpHiragana();

            //display the next button
            //nextRoundButton.gameObject.transform.parent.gameObject.SetActive(true);

            //questionText.text = "At this point the next round button can be clicked, don't worry though, I will click it for you";
            yield return new WaitForSeconds(0f);
            //nextRoundButton.onClick.Invoke();
            NextRound();
        }
    }

    /// <summary>
    /// Function that ends an encounter and determines if it was won or lost
    /// </summary>
    /// <returns>Returns nothing, needed for IEnumerators</returns>
    public IEnumerator EndEncounter()
    {
        //the encounter has been won or lost
        if (encounterState == EncounterState.WON)
        {
            //let the player know they have won the encounter
            questionText.text = "Encounter has been won";

            //Wait for a few seconds
            yield return new WaitForSeconds(2f);

            playerData.SaveData();

            questionText.text = "Now that you know how an encounter works, enjoy the game";
            yield return new WaitForSeconds(5f);

            PlayerPrefs.SetString("TutorialViewed", "true");
            PlayerPrefs.Save();

            SceneManager.LoadScene("OpeningScene", LoadSceneMode.Single);

            //Send the player back to the previous scene
            //For now we will just return to the overworld scene
            //SceneManager.LoadScene(playerData.playerReturnSceneName, LoadSceneMode.Single);
        }
        else
        if (encounterState == EncounterState.LOST)
        {
            //Let the player know they have lost the encounter
            questionText.text = "Encounter has been lost";

            playerData.SaveData();

            //wait for a few seconds
            yield return new WaitForSeconds(2f);

            //Restart the game with the player waking up in the starting area
        }
    }
}
