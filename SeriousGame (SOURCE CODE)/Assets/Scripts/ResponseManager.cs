using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ResponseManager : MonoBehaviour
{
    public static ResponseManager Instance; //Singleton instance of this script

    string saveFile;

    [SerializeField] public List<Response> responses;
    List<Response> tempList;

    /// <summary>
    /// Awake function, called before everything else, makes sure there is only one instance of this script
    /// </summary>
    private void Awake()
    {
        //If the instance of this class is null, don't destroy this gameObject and set the instance equal to this instance
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        //Otherwise, if the instance is not equal to this, destroy it
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        saveFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "responses.dat";
    }

    private void Start()
    {
        ReadFile();

        /*
        //this was used to create fake data for the development of the analytical dashboard
        for(int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("a", randomDate.ToString(), "Correct"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("a", randomDate.ToString(), "Incorrect"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("i", randomDate.ToString(), "Correct"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("i", randomDate.ToString(), "Incorrect"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("o", randomDate.ToString(), "Correct"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("o", randomDate.ToString(), "Incorrect"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("u", randomDate.ToString(), "Correct"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("u", randomDate.ToString(), "Incorrect"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("e", randomDate.ToString(), "Correct"));
        }

        for (int i = 0; i <= 100; i++)
        {
            DateTime currentDate = System.DateTime.Now;
            int randomNumber = UnityEngine.Random.Range(0, 4);
            int randomCorrector = UnityEngine.Random.Range(-3, 3);

            DateTime randomDate = currentDate.AddDays(-7 * randomNumber + randomCorrector);
            AddResponse(new Response("e", randomDate.ToString(), "Incorrect"));
        }
        */
    }

    //Deserializes the List<Response> responses
    void ReadFile()
    {
        if(File.Exists(saveFile))
        {
            Stream stream = File.Open(saveFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            responses = (List<Response>)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
    }
    
    //Serializes the List<Response> responses
    void WriteFile()
    {
        FileStream fileStream = new FileStream(saveFile, FileMode.OpenOrCreate);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, responses);
        fileStream.Close();
    }

    //Gets the file, add response to list and write it
    public void AddResponse(Response response)
    {
        ReadFile();
        responses.Add(response);
        WriteFile();
    }

    public List<Response> ReturnList()
    {
        ReadFile();
        return responses;
    }
}
