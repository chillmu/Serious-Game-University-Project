using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public static HPManager Instance;

    string saveFile;

    [SerializeField] public List<int> hpValues;

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

        saveFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "hpvalues.dat";
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
            hpValues = (List<int>)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
    }

    void WriteFile()
    {
        FileStream fileStream = new FileStream(saveFile, FileMode.OpenOrCreate);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, hpValues);
        fileStream.Close();
    }

    public void AddHpValue(int hpValue)
    {
        ReadFile();
        hpValues.Add(hpValue);
        WriteFile();
    }
}
