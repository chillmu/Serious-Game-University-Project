using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiraganaList : MonoBehaviour
{
    private List<Hiragana> hiraganaList; //The list of Hiragana

    /// <summary>
    /// Populates the list with all of the Hiragana characters
    /// </summary>
    public void GenerateList()
    {
        hiraganaList = new List<Hiragana>();

        Hiragana hira1 = new Hiragana("\u3042", "a", "v", 1);
        hiraganaList.Add(hira1);
        Hiragana hira2 = new Hiragana("\u3044", "i", "v", 1);
        hiraganaList.Add(hira2);
        Hiragana hira3 = new Hiragana("\u3046", "u", "v", 1);
        hiraganaList.Add(hira3);
        Hiragana hira4 = new Hiragana("\u3048", "e", "v", 1);
        hiraganaList.Add(hira4);
        Hiragana hira5 = new Hiragana("\u304A", "o", "v", 1);
        hiraganaList.Add(hira5);
        Hiragana hira6 = new Hiragana("\u304B", "ka", "k", 2);
        hiraganaList.Add(hira6);
        Hiragana hira7 = new Hiragana("\u30AD", "ki", "k", 2);
        hiraganaList.Add(hira7);
        Hiragana hira8 = new Hiragana("\u30AF", "ku", "k", 2);
        hiraganaList.Add(hira8);
        Hiragana hira9 = new Hiragana("\u30B1", "ke", "k", 2);
        hiraganaList.Add(hira9);
        Hiragana hira10 = new Hiragana("\u30B3", "ko", "k", 2);
        hiraganaList.Add(hira10);
        Hiragana hira11 = new Hiragana("\u30B5", "sa", "s", 3);
        hiraganaList.Add(hira11);
        Hiragana hira12 = new Hiragana("\u3057", "shi", "s", 3);
        hiraganaList.Add(hira12);
        Hiragana hira13 = new Hiragana("\u3059", "su", "s", 3);
        hiraganaList.Add(hira13);
        Hiragana hira14 = new Hiragana("\u305B", "se", "s", 3);
        hiraganaList.Add(hira14);
        Hiragana hira15 = new Hiragana("\u305D", "so", "s", 3);
        hiraganaList.Add(hira15);
        Hiragana hira16 = new Hiragana("\u305F", "ta", "t/ch/ts", 4);
        hiraganaList.Add(hira16);
        Hiragana hira17 = new Hiragana("\u3061", "chi", "t/ch/ts", 4);
        hiraganaList.Add(hira17);
        Hiragana hira18 = new Hiragana("\u3064", "tsu", "t/ch/ts", 4);
        hiraganaList.Add(hira18);
        Hiragana hira19 = new Hiragana("\u3066", "te", "t/ch/ts", 4);
        hiraganaList.Add(hira19);
        Hiragana hira20 = new Hiragana("\u3068", "to", "t/ch/ts", 4);
        hiraganaList.Add(hira20);
        Hiragana hira21 = new Hiragana("\u306A", "na", "n", 5);
        hiraganaList.Add(hira21);
        Hiragana hira22 = new Hiragana("\u306B", "ni", "n", 5);
        hiraganaList.Add(hira22);
        Hiragana hira23 = new Hiragana("\u306C", "nu", "n", 5);
        hiraganaList.Add(hira23);
        Hiragana hira24 = new Hiragana("\u306D", "ne", "n", 5);
        hiraganaList.Add(hira24);
        Hiragana hira25 = new Hiragana("\u306E", "no", "n", 5);
        hiraganaList.Add(hira25);
        Hiragana hira26 = new Hiragana("\u306F", "ha", "h/f", 6);
        hiraganaList.Add(hira26);
        Hiragana hira27 = new Hiragana("\u3072", "hi", "h/f", 6);
        hiraganaList.Add(hira27);
        Hiragana hira28 = new Hiragana("\u3075", "fu", "h/f", 6);
        hiraganaList.Add(hira28);
        Hiragana hira29 = new Hiragana("\u3078", "he", "h/f", 6);
        hiraganaList.Add(hira29);
        Hiragana hira30 = new Hiragana("\u307B", "ho", "h/f", 6);
        hiraganaList.Add(hira30);
        Hiragana hira31 = new Hiragana("\u307E", "ma", "m", 7);
        hiraganaList.Add(hira31);
        Hiragana hira32 = new Hiragana("\u307F", "mi", "m", 7);
        hiraganaList.Add(hira32);
        Hiragana hira33 = new Hiragana("\u3080", "mu", "m", 7);
        hiraganaList.Add(hira33);
        Hiragana hira34 = new Hiragana("\u3081", "me", "m", 7);
        hiraganaList.Add(hira34);
        Hiragana hira35 = new Hiragana("\u3082", "mo", "m", 7);
        hiraganaList.Add(hira35);
        Hiragana hira36 = new Hiragana("\u3084", "ya", "y", 8);
        hiraganaList.Add(hira36);
        Hiragana hira37 = new Hiragana("\u3086", "yu", "y", 8);
        hiraganaList.Add(hira37);
        Hiragana hira38 = new Hiragana("\u3088", "yo", "y", 8);
        hiraganaList.Add(hira38);
        Hiragana hira39 = new Hiragana("\u3089", "ra", "r", 9);
        hiraganaList.Add(hira39);
        Hiragana hira40 = new Hiragana("\u308A", "ri", "r", 9);
        hiraganaList.Add(hira40);
        Hiragana hira41 = new Hiragana("\u308B", "ru", "r", 9);
        hiraganaList.Add(hira41);
        Hiragana hira42 = new Hiragana("\u308C", "re", "r", 9);
        hiraganaList.Add(hira42);
        Hiragana hira43 = new Hiragana("\u308D", "ro", "r", 9);
        hiraganaList.Add(hira43);
        Hiragana hira44 = new Hiragana("\u308F", "wa", "w", 10);
        hiraganaList.Add(hira44);
        Hiragana hira45 = new Hiragana("\u3092", "wo", "w", 10);
        hiraganaList.Add(hira45);
        Hiragana hira46 = new Hiragana("\u3093", "n", "n", 10);
        hiraganaList.Add(hira46);
    }

    /// <summary>
    /// Prints the list of hiragana to the unity console, used for testing purposes
    /// </summary>
    public void PrintList()
    {
        //Get every Hiragana in the list and log it to console by calling their methods
        for(int i = 0; i < hiraganaList.Count; i++)
        {
            Debug.Log("character = " + hiraganaList[i].GetCharacter() + " " + "sound = " + hiraganaList[i].GetSound() + " " + "column = " + hiraganaList[i].GetColumn());
        }
    }

    /// <summary>
    /// Gets the list of Hiragana
    /// </summary>
    /// <returns>Returns the list of Hiragana</returns>
    public List<Hiragana> GetList()
    {
        return hiraganaList;
    }

    /// <summary>
    /// Gets the total number of Hiragana in the list
    /// </summary>
    /// <returns>Returns the total number of Hiragana in the list</returns>
    public int GetSize()
    {
        return hiraganaList.Count;
    }

    /// <summary>
    /// Gets the Hiragana from the list based on its index in that list
    /// </summary>
    /// <param name="index">The index value in the list of Hiragana</param>
    /// <returns>Returns the Hiragana character based the index position in the list</returns>
    public Hiragana GetIndex(int index)
    {
        return hiraganaList[index];
    }
}
