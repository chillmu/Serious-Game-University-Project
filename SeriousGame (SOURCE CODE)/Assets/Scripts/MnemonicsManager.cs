using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MnemonicsManager : MonoBehaviour
{
    public GameObject mnemonicsEntryPrefab;
    public GameObject mnemonicsEntryParent;

    public Button closeMenuButton;
    public GameObject thisMenu;

    public static MnemonicsManager Instance;

    public List<GameObject> mnemonicsList;

    private void Awake()
    {
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
    /// Start function, used to call the AddHiragana function to add all of the Hiragana to the menu
    /// </summary>
    private void Start()
    {
        mnemonicsList = new List<GameObject>();
        closeMenuButton.onClick.AddListener(delegate { CloseMenu(); });

        //     red = #FF0000
        //  yellow = #FFFF00
        //   green = #00FF00
        //    blue = #0000FF
        //burgundy = #800020

        //   <color=><b><i>
        //   </i></b></color>

        AddHiragana("\u3042", "a", "An <color=#FF0000><b><i>antenna</i></b></color> is on top of the roof. As in '<color=#FF0000><b><i>antenna</i></b></color>'.");
        AddHiragana("\u3044", "i", "There are two <color=#FFFF00><b><i>eels</i></b></color>. As in '<color=#FFFF00><b><i>eels</i></b></color>'."); 
        AddHiragana("\u3046", "u", "<color=#00FF00><b><i>Ooo!</i></b></color> This is heavy. As in '<color=#00FF00><b><i>Ooo!</i></b></color>'.");
        AddHiragana("\u3048", "e", "I have to <color=#0000FF><b><i>exercise</i></b></color>. As in '<color=#0000FF><b><i>exercise</i></b></color>'.");
        AddHiragana("\u304A", "o", "A gold ball is <color=#800020><b><i>on</i></b></color> the green. As in '<color=#800020><b><i>on</i></b></color>'.");
        AddHiragana("\u304B", "ka", "A <color=#FF0000><b><i>kite</i></b></color> is flying in the sky. As in '<color=#FF0000><b><i>kite</i></b></color>'.");
        AddHiragana("\u30AD", "ki", "I have a <color=#FFFF00><b><i>key</i></b></color>. As in '<color=#FFFF00><b><i>key</i></b></color>'.");
        AddHiragana("\u30AF", "ku", "Here comes a <color=#00FF00><b><i>cuckoo</i></b></color> bird. As in '<color=#00FF00><b><i>cuckoo</i></b></color>'.");
        AddHiragana("\u30B1", "ke", "There's a <color=#0000FF><b><i>keg</i></b></color> of beer. As in '<color=#0000FF><b><i>keg</i></b></color>'.");
        AddHiragana("\u30B3", "ko", "A <color=#800020><b><i>core</i></b></color> of an apple is hard to eat. As in '<color=#800020><b><i>core</i></b></color>'.");
        AddHiragana("\u30B5", "sa", "I love <color=#FF0000><b><i>sake!</i></b></color>. As in '<color=#FF0000><b><i>sake</i></b></color>'.");
        AddHiragana("\u3057", "shi", "<color=#FFFF00><b><i>She</i></b></color> has a ponytail. As in '<color=#FFFF00><b><i>she</i></b></color>'.");
        AddHiragana("\u3059", "su", "<color=#00FF00><b><i>Sooey</i></b></color>, sooey! As in '<color=#00FF00><b><i>sooey</i></b></color>'.");
        AddHiragana("\u305B", "se", "This is <color=#0000FF><b><i>Senor</i></b></color> Lopez. As in '<color=#0000FF><b><i>senor</i></b></color>'.");
        AddHiragana("\u305D", "so", "Zig-zag <color=#800020><b><i>sewing</i></b></color>. As in '<color=#800020><b><i>sewing</i></b></color>'.");
        AddHiragana("\u305F", "ta", "'<color=#FF0000><b><i>t</i></b></color>' and '<color=#FF0000><b><i>a</i></b></color>' make ta. As in '<color=#FF0000><b><i>ta</i></b></color>'.");
        AddHiragana("\u3061", "chi", "She is a <color=#FFFF00><b><i>cheerleader</i></b></color>. As in '<color=#FFFF00><b><i>cheerleader</i></b></color>'.");
        AddHiragana("\u3064", "tsu", "<color=#00FF00><b><i>Tsunami</i></b></color> is a tidal wave. As in '<color=#00FF00><b><i>tsunami</i></b></color>'.");
        AddHiragana("\u3066", "te", "The dog has a wagging <color=#0000FF><b><i>tail</i></b></color>. As in '<color=#0000FF><b><i>tail</i></b></color>'.");
        AddHiragana("\u3068", "to", "Ouch! A nail is in my <color=#800020><b><i>toe</i></b></color>. As in '<color=#800020><b><i>toe</i></b></color>'.");
        AddHiragana("\u306A", "na", "A <color=#FF0000><b><i>nun</i></b></color> is kneeling in front of a cross. As in '<color=#FF0000><b><i>nun</i></b></color>'.");
        AddHiragana("\u306B", "ni", "I have a <color=#FFFF00><b><i>needle</i></b></color> and thread. As in '<color=#FFFF00><b><i>needle</i></b></color>'.");
        AddHiragana("\u306C", "nu", "<color=#00FF00><b><i>Noodles</i></b></color> and chopsticks. As in '<color=#00FF00><b><i>noodles</i></b></color>'.");
        AddHiragana("\u306D", "ne", "I caught a big fish in a <color=#0000FF><b><i>net</i></b></color>. As in '<color=#0000FF><b><i>net</i></b></color>'.");
        AddHiragana("\u306E", "no", "This means <color=#800020><b><i>NO</i></b></color>! As in '<color=#800020><b><i>no</i></b></color>'.");
        AddHiragana("\u306F", "ha", "I live in a <color=#FF0000><b><i>house</i></b></color>. As in '<color=#FF0000><b><i>house</i></b></color>'.");
        AddHiragana("\u3072", "hi", "<color=#FFFF00><b><i>He</i></b></color> is on the wall. As in '<color=#FFFF00><b><i>he</i></b></color>'.");
        AddHiragana("\u3075", "fu", "I climbed Mt.<color=#00FF00><b><i>Fuji</i></b></color>. As in '<color=#00FF00><b><i>Fuji</i></b></color>'.");
        AddHiragana("\u3078", "he", "There is a <color=#0000FF><b><i>haystack</i></b></color>. As in '<color=#0000FF><b><i>haystack</i></b></color>'.");
        AddHiragana("\u307B", "ho", "A house becomes a <color=#800020><b><i>home</i></b></color> with a satellite. As in '<color=#800020><b><i>home</i></b></color>'.");
        AddHiragana("\u307E", "ma", "<color=#FF0000><b><i>Mama</i></b></color> loves music. As in '<color=#FF0000><b><i>mama</i></b></color>'.");
        AddHiragana("\u307F", "mi", "Who is 21? <color=#FFFF00><b><i>Me</i></b></color>!. As in '<color=#FFFF00><b><i>me</i></b></color>'.");
        AddHiragana("\u3080", "mu", "<color=#00FF00><b><i>Moo-moo</i></b></color> more milk? As in '<color=#00FF00><b><i>moo</i></b></color>'.");
        AddHiragana("\u3081", "me", "Chopsticks and noodles without a <color=#0000FF><b><i>mess</i></b></color>. As in '<color=#0000FF><b><i>mess</i></b></color>'.");
        AddHiragana("\u3082", "mo", "The <color=#800020><b><i>more</i></b></color> worms, the more fish. As in '<color=#800020><b><i>more</i></b></color>'.");
        AddHiragana("\u3084", "ya", "I am sailing on a <color=#FF0000><b><i>yacht</i></b></color>. As in '<color=#FF0000><b><i>yacht</i></b></color>'.");
        AddHiragana("\u3086", "yu", "Make a <color=#00FF00><b><i>U-turn</i></b></color> as quickly as you can. As in '<color=#00FF00><b><i>U-turn</i></b></color>'.");
        AddHiragana("\u3088", "yo", "<color=#800020><b><i>Yoga</i></b></color> is hard! As in '<color=#800020><b><i>yoga</i></b></color>'.");
        AddHiragana("\u3089", "ra", "I love steamy <color=#FF0000><b><i>ramen</i></b></color> noodles. As in '<color=#FF0000><b><i>ramen</i></b></color>'.");
        AddHiragana("\u308A", "ri", "I'll give you a <color=#FFFF00><b><i>ribbon</i></b></color>. As in '<color=#FFFF00><b><i>ribbon</i></b></color>'.");
        AddHiragana("\u308B", "ru", "Look at my <color=#00FF00><b><i>loop</i></b></color>! As in '<color=#00FF00><b><i>loop</i></b></color>'.");
        AddHiragana("\u308C", "re", "Let's <color=#0000FF><b><i>race</i></b></color>! As in '<color=#0000FF><b><i>race</i></b></color>'.");
        AddHiragana("\u308D", "ro", "I'm a <color=#800020><b><i>roper</i></b></color>! As in '<color=#800020><b><i>roper</i></b></color>'.");
        AddHiragana("\u308F", "wa", "Wow! A magic <color=#FF0000><b><i>wand</i></b></color>! As in '<color=#FF0000><b><i>wand</i></b></color>'.");
        AddHiragana("\u3092", "wo", "<color=#800020><b><i>Woah</i></b></color>! A cheerleader is on my toe! As in '<color=#800020><b><i>woah</i></b></color>'.");
        AddHiragana("\u3093", "n", "This is in the <color=#00FF00><b><i>end</i></b></color>. As in '<color=#00FF00><b><i>end</i></b></color>'.");
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
    void AddHiragana(string character, string sound, string textMnemonic)
    {
        GameObject entry = (GameObject)Instantiate(mnemonicsEntryPrefab);

        entry.transform.SetParent(mnemonicsEntryParent.transform);

        entry.transform.localScale = new Vector3(1, 1, 1);

        entry.transform.GetChild(0).GetComponent<Text>().text = character;
        entry.transform.GetChild(1).GetComponent<Text>().text = sound;

        string path1 = "Mnemonics/" + sound + "1";
        string path2 = "Mnemonics/" + sound + "2";

        entry.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(path1);
        entry.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>(path2);

        entry.transform.GetChild(4).GetComponent<Text>().text = textMnemonic;

        mnemonicsList.Add(entry);
    }
}
