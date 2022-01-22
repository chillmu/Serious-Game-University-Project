using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcessResponses : MonoBehaviour
{
    public string participantID;

    public TextMeshProUGUI processResponsesText;
    string processResponsesString;

    string responsesDatFile;
    string responsesCsvFile;

    string encounterTimesDatFile;
    string encounterTimesCsvFile;

    string sessionTimesDatFile;
    string sessionTimesCsvFile;

    string roomTimesDatFile;
    string roomTimesCsvFile;

    string roomOrdersDatFile;
    string roomOrdersCsvFile;

    string hpValuesDatFile;
    string hpValuesCsvFile;
    
    List<Response> responses;
    List<RoomTime> roomTimes;
    List<EncounterTime> encounterTimes;
    List<string> sessionTimes;
    List<string> roomOrders;
    List<int> hpValues;

    List<string[]> rowData = new List<string[]>();

    private void Start()
    {
        responsesDatFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "responses.dat";
        responsesCsvFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "responses_" + participantID + ".csv";

        encounterTimesDatFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "encountertimes.dat";
        encounterTimesCsvFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "encountertimes_" + participantID + ".csv";

        sessionTimesDatFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "sessiontimes.dat";
        sessionTimesCsvFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "sessiontimes_" + participantID + ".csv";

        roomTimesDatFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "roomtimes.dat";
        roomTimesCsvFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "roomtimes_" + participantID + ".csv";

        roomOrdersDatFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "roomorders.dat";
        roomOrdersCsvFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "roomorders_" + participantID + ".csv";

        hpValuesDatFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "hpvalues.dat";
        hpValuesCsvFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "hpvalues_" + participantID + ".csv";

        ReadFiles();
    }

    void ReadFiles()
    {
        if(File.Exists(responsesDatFile))
        {
            Debug.Log("responses.dat file has been found");
            processResponsesString += "<color=green>responses.dat file has been found</color>" + "\n";
            processResponsesText.text = processResponsesString;
            Stream stream = File.Open(responsesDatFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            responses = (List<Response>)binaryFormatter.Deserialize(stream);
            stream.Close();
            ProcessResponsesData();
        }
        else
        {
            Debug.Log("The responses.dat file could not be found");
            processResponsesString += "<color=yellow>responses.dat file could not be found</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }

        if(File.Exists(encounterTimesDatFile))
        {
            Debug.Log("encountertimes.dat file has been found");
            processResponsesString += "<color=green>encountertimes.dat file has been found</color>" + "\n";
            processResponsesText.text = processResponsesString;
            Stream stream = File.Open(encounterTimesDatFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            encounterTimes = (List<EncounterTime>)binaryFormatter.Deserialize(stream);
            stream.Close();
            ProcessEncounterTimesData();
        }
        else
        {
            Debug.Log("The encountertimes.dat file could not be found");
            processResponsesString += "<color=yellow>encountertimes.dat file could not be found</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }    

        if(File.Exists(sessionTimesDatFile))
        {
            Debug.Log("sessiontimes.dat file has been found");
            processResponsesString += "<color=green>sessiontimes.dat file has been found</color>" + "\n";
            processResponsesText.text = processResponsesString;
            Stream stream = File.Open(sessionTimesDatFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            sessionTimes = (List<string>)binaryFormatter.Deserialize(stream);
            stream.Close();
            ProcessSessionTimesData();
        }
        else
        {
            Debug.Log("The sessiontimes.dat file could not be found");
            processResponsesString += "<color=yellow>sessiontimes.dat file could not be found</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }

        if(File.Exists(roomTimesDatFile))
        {
            Debug.Log("roomtimes.dat file has been found");
            processResponsesString += "<color=green>roomtimes.dat file has been found</color>" + "\n";
            processResponsesText.text = processResponsesString;
            Stream stream = File.Open(roomTimesDatFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            roomTimes = (List<RoomTime>)binaryFormatter.Deserialize(stream);
            stream.Close();
            ProcessRoomTimesData();
        }
        else
        {
            Debug.Log("roomtimes.dat file could not be found");
            processResponsesString += "<color=yellow>roomtimes.dat file could not be found</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }

        if (File.Exists(roomOrdersDatFile))
        {
            Debug.Log("roomorders.dat file has been found");
            processResponsesString += "<color=green>roomorders.dat file has been found</color>" + "\n";
            processResponsesText.text = processResponsesString;
            Stream stream = File.Open(roomOrdersDatFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            roomOrders = (List<string>)binaryFormatter.Deserialize(stream);
            stream.Close();
            ProcessRoomOrdersData();
        }
        else
        {
            Debug.Log("roomorders.dat file could not be found");
            processResponsesString += "<color=yellow>roomorders.dat file could not be found</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }

        if (File.Exists(hpValuesDatFile))
        {
            Debug.Log("hpvalues.dat file has been found");
            processResponsesString += "<color=green>hpvalues.dat file has been found</color>" + "\n";
            processResponsesText.text = processResponsesString;
            Stream stream = File.Open(hpValuesDatFile, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            hpValues = (List<int>)binaryFormatter.Deserialize(stream);
            stream.Close();
            ProcessHPValuesData();
        }
        else
        {
            Debug.Log("hpvalues.dat file could not be found");
            processResponsesString += "<color=yellow>hpvalues.dat file could not be found</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }

        processResponsesString += "\n" + "<color=white>The csv files can be found in: " + Application.persistentDataPath + "</color>";
        processResponsesText.text = processResponsesString;
    }

    void ProcessResponsesData()
    {
        Debug.Log("Processing response data");
        processResponsesString += "processing the response.dat file..." + "\n";
        processResponsesText.text = processResponsesString;
        rowData = new List<string[]>();
        string[] rowDataTemp = new string[6];
        rowDataTemp[0] = "Response Sound";
        rowDataTemp[1] = "Response Date";
        rowDataTemp[2] = "Correct Response";
        rowDataTemp[3] = "Incorrect Response";
        rowDataTemp[4] = "Response Time";
        rowDataTemp[5] = "Room Encounterd In";
        rowData.Add(rowDataTemp);

        foreach(Response response in responses)
        {
            rowDataTemp = new string[6];
            rowDataTemp[0] = response.GetResponseSound();
            rowDataTemp[1] = response.GetResponseDate().ToString();

            if(response.GetResponseOutcome() == "Correct")
            {
                rowDataTemp[2] = 1.ToString();
                rowDataTemp[3] = 0.ToString();
            }
            else
            {
                rowDataTemp[2] = 0.ToString();
                rowDataTemp[3] = 1.ToString();
            }

            rowDataTemp[4] = response.GetSecondsToRespond();
            rowDataTemp[5] = response.GetRoomEncounteredIn();

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for(int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        StreamWriter outStream = File.CreateText(responsesCsvFile);
        outStream.WriteLine(sb);
        outStream.Close();

        if(File.Exists(responsesCsvFile))
        {
            processResponsesString += "<color=green>the responses.dat file has successfully been processed" + "\n";
            processResponsesText.text = processResponsesString;
        }
        else
        {
            processResponsesString += "<color=red>the responses.dat file could not be processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
    }

    void ProcessEncounterTimesData()
    {
        Debug.Log("Processing encounter times data");
        processResponsesString += "processing the encountertimes.dat file..." + "\n";
        processResponsesText.text = processResponsesString;
        rowData = new List<string[]>();
        string[] rowDataTemp = new string[3];
        rowDataTemp[0] = "Seconds Taken";
        rowDataTemp[1] = "Room Name";
        rowDataTemp[2] = "Outcome";
        rowData.Add(rowDataTemp);

        foreach(EncounterTime et in encounterTimes)
        {
            rowDataTemp = new string[3];
            rowDataTemp[0] = et.GetSeconds();
            rowDataTemp[1] = et.GetRoom();
            rowDataTemp[2] = et.GetEncounterOutcome();

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        StreamWriter outStream = File.CreateText(encounterTimesCsvFile);
        outStream.WriteLine(sb);
        outStream.Close();

        if (File.Exists(encounterTimesCsvFile))
        {
            processResponsesString += "<color=green>the encountertimes.dat file has been successfully processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
        else
        {
            processResponsesString += "<color=red>the encountertimes.dat file could not be processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
    }

    void ProcessSessionTimesData()
    {
        Debug.Log("Processing encounter times data");
        processResponsesString += "processing encountertimes.dat file..." + "\n";
        processResponsesText.text = processResponsesString;
        rowData = new List<string[]>();
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = "Session Time";
        rowData.Add(rowDataTemp);

        foreach(string s in sessionTimes)
        {
            rowDataTemp = new string[1];
            rowDataTemp[0] = s;

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        StreamWriter outStream = File.CreateText(sessionTimesCsvFile);
        outStream.WriteLine(sb);
        outStream.Close();

        if (File.Exists(sessionTimesCsvFile))
        {
            processResponsesString += "<color=green>the sessiontimes.dat file has successfully been processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
        else
        {
            processResponsesString += "<color=red>the sessiontimes.dat file could not be processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
    }

    void ProcessRoomTimesData()
    {
        Debug.Log("Processing room times data");
        processResponsesString += "processing roomtimes.dat file..." + "\n";
        processResponsesText.text = processResponsesString;
        rowData = new List<string[]>();
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Seconds Taken";
        rowDataTemp[1] = "Room Name";
        rowData.Add(rowDataTemp);

        foreach(RoomTime rt in roomTimes)
        {
            rowDataTemp = new string[3];
            rowDataTemp[0] = rt.GetSeconds();
            rowDataTemp[1] = rt.GetRoom();

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        StreamWriter outStream = File.CreateText(roomTimesCsvFile);
        outStream.WriteLine(sb);
        outStream.Close();

        if (File.Exists(roomTimesCsvFile))
        {
            processResponsesString += "<color=green>the roomtimes.dat file has successfully been processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
        else
        {
            processResponsesString += "<color=red>the roomtimes.dat file could not be processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
    }

    void ProcessRoomOrdersData()
    {
        Debug.Log("Processing room orders data");
        processResponsesString += "processing roomorders.dat file..." + "\n";
        processResponsesText.text = processResponsesString;
        rowData = new List<string[]>();
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = "Room Order Completed";
        rowData.Add(rowDataTemp);

        foreach (string s in roomOrders)
        {
            rowDataTemp = new string[1];
            rowDataTemp[0] = s;

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        StreamWriter outStream = File.CreateText(roomOrdersCsvFile);
        outStream.WriteLine(sb);
        outStream.Close();

        if (File.Exists(roomOrdersCsvFile))
        {
            processResponsesString += "<color=green>the roomorders.dat file has successfully been processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
        else
        {
            processResponsesString += "<color=red>the roomorders.dat file could not be processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
    }

    void ProcessHPValuesData()
    {
        Debug.Log("Processing hp values data");
        processResponsesString += "processing hpvalues.dat file..." + "\n";
        processResponsesText.text = processResponsesString;
        rowData = new List<string[]>();
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = "HP Values";
        rowData.Add(rowDataTemp);

        foreach(int i in hpValues)
        {
            rowDataTemp = new string[1];
            rowDataTemp[0] = i.ToString();

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        StreamWriter outStream = File.CreateText(hpValuesCsvFile);
        outStream.WriteLine(sb);
        outStream.Close();

        if (File.Exists(hpValuesCsvFile))
        {
            processResponsesString += "<color=green>the hpvalues.dat file has successfully been processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
        else
        {
            processResponsesString += "<color=red>the hpvalues.dat file could not be processed</color>" + "\n";
            processResponsesText.text = processResponsesString;
        }
    }
}
