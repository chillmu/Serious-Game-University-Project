using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] public string responseSound;
    [SerializeField] public string responseDate;
    [SerializeField] public string responseOutcome;
    [SerializeField] public string secondsToRespond;
    [SerializeField] public string roomEncounteredIn;

    public Response(string responseSound, string responseDate, string responseOutcome, string secondsToRespond, string roomEncounteredIn)
    {
        this.responseSound = responseSound;
        this.responseDate = responseDate;
        this.responseOutcome = responseOutcome;
        this.secondsToRespond = secondsToRespond;
        this.roomEncounteredIn = roomEncounteredIn;
    }

    public string GetResponseSound()
    {
        return responseSound;
    }

    public string GetResponseDate()
    {
        return responseDate;
    }

    public string GetResponseOutcome()
    {
        return responseOutcome;
    }

    public string GetSecondsToRespond()
    {
        return secondsToRespond;
    }

    public string GetRoomEncounteredIn()
    {
        return roomEncounteredIn;
    }
}
