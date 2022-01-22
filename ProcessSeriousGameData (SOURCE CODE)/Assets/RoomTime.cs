using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomTime
{
    [SerializeField] public string seconds;
    [SerializeField] public string room;

    public RoomTime(string seconds, string room)
    {
        this.seconds = seconds;
        this.room = room;
    }

    public string GetSeconds()
    {
        return seconds;
    }

    public string GetRoom()
    {
        return room;
    }
}
