using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EncounterTimes : MonoBehaviour
{
    public static EncounterTimes Instance;

    string saveFile;

    [SerializeField] public List<EncounterTime> encounterTimes;
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

        saveFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "encountertimes.dat";
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
            encounterTimes = (List<EncounterTime>)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
    }

    void WriteFile()
    {
        FileStream fileStream = new FileStream(saveFile, FileMode.OpenOrCreate);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, encounterTimes);
        fileStream.Close();
    }

    public void AddEncounterTime(EncounterTime encounterTime)
    {
        ReadFile();
        encounterTimes.Add(encounterTime);
        WriteFile();
    }
}
