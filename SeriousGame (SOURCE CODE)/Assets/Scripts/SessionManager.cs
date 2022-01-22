using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    string saveFile;

    [SerializeField] public List<string> sessionTimes;
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

        saveFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "sessiontimes.dat";
    }

    private void Start()
    {
        ReadFile();
    }

    void ReadFile()
    {
        if(File.Exists(saveFile))
        {
            Stream stream = File.Open(saveFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            sessionTimes = (List<string>)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
    }

    void WriteFile()
    {
        FileStream fileStream = new FileStream(saveFile, FileMode.OpenOrCreate);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, sessionTimes);
        fileStream.Close();
    }

    //We can get the time elapsed since the game started and save it
    private void OnApplicationQuit()
    {
        ReadFile();
        float timeSinceSessionStart = Time.realtimeSinceStartup;
        string time = FormatFloatSeconds(timeSinceSessionStart);
        sessionTimes.Add(time);
        WriteFile();
    }

    public string FormatFloatSeconds(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString();
    }
}
