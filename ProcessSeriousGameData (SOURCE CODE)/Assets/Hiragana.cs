using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiragana
{
    private string character; //The Hiragana character
    private string sound; //The sound of the Hiragana character
    private string column; //The column the Hiragana character belongs to
    private int level; //The level, related to the column, greater is more dificult

    /// <summary>
    /// Empty Hiragana constructor
    /// </summary>
    public Hiragana()
    {

    }

    /// <summary>
    /// Hiragana constructor
    /// </summary>
    /// <param name="character">The Hiragana character</param>
    /// <param name="sound">The sound the Hiragana character represents</param>
    /// <param name="column">The column value the Hiragana character belongs to</param>
    /// <param name="level">The level, translated from column position</param>
    public Hiragana(string character, string sound, string column, int level)
    {
        this.character = character;
        this.sound = sound;
        this.column = column;
        this.level = level;
    }

    /// <summary>
    /// Gets the Hiragana's character
    /// </summary>
    /// <returns>Returns the Hiragana's character</returns>
    public string GetCharacter()
    {
        return character;
    }

    /// <summary>
    /// Gets the Hiragana's sound
    /// </summary>
    /// <returns>Returns the Hiragana's sound</returns>
    public string GetSound()
    {
        return sound;
    }

    /// <summary>
    /// Gets the Hiragana's column
    /// </summary>
    /// <returns>Returns the Hiragana's column</returns>
    public string GetColumn()
    {
        return column;
    }

    /// <summary>
    /// Gets the Hiragana's level
    /// </summary>
    /// <returns>Returns the Hiragana's level</returns>
    public int GetLevel()
    {
        return level;
    }
}
