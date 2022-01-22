using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class RoomTimes : MonoBehaviour
{
    public static RoomTimes Instance;

    string saveFile;

    [SerializeField] public List<RoomTime> roomTimes;
    List<string> tempList;

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

        saveFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "roomtimes.dat";
    }

    private void Start()
    {
        ReadFile();
    }

    void ReadFile()
    {
        if (File.Exists(saveFile))
        {
            Stream stream = File.Open(saveFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            roomTimes = (List<RoomTime>)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
    }

    void WriteFile()
    {
        FileStream fileStream = new FileStream(saveFile, FileMode.OpenOrCreate);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, roomTimes);
        fileStream.Close();
    }

    public void AddRoomTime(RoomTime roomTime)
    {
        ReadFile();
        roomTimes.Add(roomTime);
        WriteFile();
    }
}
