using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EncounterTime
{
    [SerializeField] public string seconds;
    [SerializeField] public string room;
    [SerializeField] public string encounterOutcome;

    public EncounterTime(string seconds, string room, string encounterOutcome)
    {
        this.seconds = seconds;
        this.room = room;
        this.encounterOutcome = encounterOutcome;
    }

    public string GetSeconds()
    {
        return seconds;
    }

    public string GetRoom()
    {
        return room;
    }

    public string GetEncounterOutcome()
    {
        return encounterOutcome;
    }
}
