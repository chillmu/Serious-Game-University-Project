using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class SendEmail : MonoBehaviour
{
    private string fromEmail = "INSERT EMAIL HERE";
    private string toEmail = "INSERT EMAIL HERE";
    private string subject = "SeriousGameData";
    private string body = null; //this is set during SendMail() because we can't access PlayerPrefs yet
    private string password = "INSERT PASSWORD HERE";

    public Button sendEmailButton;

    // Start is called before the first frame update
    void Start()
    {
        sendEmailButton.onClick.AddListener(delegate { SendMail(); });
    }

    // Update is called once per frame
    void SendMail()
    {
        string responseFilePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "responses.dat";
        string sessionTimesFilePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "sessiontimes.dat";
        string roomTimesFilePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "roomtimes.dat";
        string encounterTimesFilePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "encountertimes.dat";
        string hpValuesFilePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "hpvalues.dat";

        Attachment data1 = new Attachment(File.Open(responseFilePath, FileMode.Open), "responses.dat");
        Attachment data2 = new Attachment(File.Open(sessionTimesFilePath, FileMode.Open), "sessiontimes.dat");
        Attachment data3 = new Attachment(File.Open(roomTimesFilePath, FileMode.Open), "roomtimes.dat");
        Attachment data4 = new Attachment(File.Open(encounterTimesFilePath, FileMode.Open), "encountertimes.dat");

        List<string> sessionTimesStrings = SessionManager.Instance.sessionTimes;
        List<TimeSpan> sessionTimeSpans = new List<TimeSpan>();
        List<EncounterTime> encounterTimes = EncounterTimes.Instance.encounterTimes;
        List<RoomTime> roomTimes = RoomTimes.Instance.roomTimes;
        List<int> hpValues = HPManager.Instance.hpValues;

        List<string> roomOrder = RoomOrder.Instance.roomOrder;
        string orderRoomsCompleted = String.Join(",", roomOrder);

        int numberOfVales = 0;
        int totalValue = 0;
        foreach(int hpValue in hpValues)
        {
            numberOfVales++;
            totalValue += hpValue;
        }

        int averageHpLeavingRooms = totalValue / numberOfVales;

        TimeSpan totalEncounterTime;
        foreach(EncounterTime et in encounterTimes)
        {
            string seconds = et.GetSeconds();
            TimeSpan thisTime = TimeSpan.FromSeconds(float.Parse(seconds));
            totalEncounterTime += thisTime;
        }

        TimeSpan totalRoomTime;
        foreach(RoomTime rt in roomTimes)
        {
            string seconds = rt.GetSeconds();
            TimeSpan thisTime = TimeSpan.FromSeconds(float.Parse(seconds));
            totalRoomTime += thisTime;
        }

        foreach(string time in sessionTimesStrings)
        {
            TimeSpan ts = TimeSpan.Parse(time);
            sessionTimeSpans.Add(ts);
        }

        TimeSpan averageTimeSpan;

        if(sessionTimeSpans.Count > 0)
        {
            averageTimeSpan = new TimeSpan((long)sessionTimeSpans.Select(ts => ts.Ticks).Average());
        }

        TimeSpan totalTimeSpan;

        foreach(TimeSpan ts in sessionTimeSpans)
        {
            totalTimeSpan += ts;
        }

        Dictionary<string, Achievement> achievementDictionary = AchievementManager.Instance.achievementDictionary;

        int totalNumberOfAchievements = 0;
        int totalNumberOfUnlockedAchievements = 0;
        int totalNumberOfPointsAvailable = 0;

        foreach(KeyValuePair<string, Achievement> pair in achievementDictionary)
        {
            if(pair.Value.Unlocked)
            {
                totalNumberOfUnlockedAchievements++;
            }

            totalNumberOfAchievements++;
            totalNumberOfPointsAvailable = totalNumberOfPointsAvailable + pair.Value.Points;
        }

        List<bool> completed = new List<bool>();

        for(int i = 1; i <= 15; i++)
        {
            if(PlayerPrefs.GetInt("Book" + i) == 1)
            {
                completed.Add(true);
            }
            else
            {
                completed.Add(false);
            }
        }

        body = "Participant ID: " + PlayerPrefs.GetString("ParticipantID") + Environment.NewLine +
               "Number of times the achievement menu was opened: " + PlayerPrefs.GetInt("AchievementAmount") + Environment.NewLine +
               "Number of times the dictionary menu was opened: " + PlayerPrefs.GetInt("DictionaryAmount") + Environment.NewLine +
               "Number of times the analytics menu was opened: " + PlayerPrefs.GetInt("AnalyticsAmount") + Environment.NewLine +
               "Number of times the mnemonics menu was opened: " + PlayerPrefs.GetInt("MnemonicsAmount") + Environment.NewLine +
               "Number of times an analytical graph was viewed: " + PlayerPrefs.GetInt("ViewGraphAmount") + Environment.NewLine +
               "Number of times a dictionary sound was played: " + PlayerPrefs.GetInt("PlaySoundAmount") + Environment.NewLine +
               "Number of times a sign post was fully interacted with: " + PlayerPrefs.GetInt("SignPostTimes") + Environment.NewLine + Environment.NewLine +
               "Order Rooms were completed in: " + orderRoomsCompleted + Environment.NewLine +
               "Was Room 1 completed? " + completed[0] + Environment.NewLine +
               "Date Room 1 was completed: " + PlayerPrefs.GetString("Room1CompletedDate") + Environment.NewLine +
               "Number of times Room 1 was entered: " + PlayerPrefs.GetInt("Room1Entered") + Environment.NewLine +
               "Number of times Room 1 was failed: " + PlayerPrefs.GetInt("Room1Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 2 completed? " + completed[1] + Environment.NewLine +
               "Date Room 2 was completed: " + PlayerPrefs.GetString("Room2CompletedDate") + Environment.NewLine +
               "Number of times Room 2 was entered: " + PlayerPrefs.GetInt("Room2Entered") + Environment.NewLine +
               "Number of times Room 2 was failed: " + PlayerPrefs.GetInt("Room2Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 3 completed? " + completed[2] + Environment.NewLine +
               "Date Room 3 was completed: " + PlayerPrefs.GetString("Room3CompletedDate") + Environment.NewLine +
               "Number of times Room 3 was entered: " + PlayerPrefs.GetInt("Room3Entered") + Environment.NewLine +
               "Number of times Room 3 was failed: " + PlayerPrefs.GetInt("Room3Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 4 completed? " + completed[3] + Environment.NewLine +
               "Date Room 4 was completed: " + PlayerPrefs.GetString("Room4CompletedDate") + Environment.NewLine +
               "Number of times Room 4 was entered: " + PlayerPrefs.GetInt("Room4Entered") + Environment.NewLine +
               "Number of times Room 4 was failed: " + PlayerPrefs.GetInt("Room4Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 5 completed? " + completed[4] + Environment.NewLine +
               "Date Room 5 was completed: " + PlayerPrefs.GetString("Room5CompletedDate") + Environment.NewLine +
               "Number of times Room 5 was entered: " + PlayerPrefs.GetInt("Room5Entered") + Environment.NewLine +
               "Number of times Room 5 was failed: " + PlayerPrefs.GetInt("Room5Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 6 completed? " + completed[5] + Environment.NewLine +
               "Date Room 6 was completed: " + PlayerPrefs.GetString("Room6CompletedDate") + Environment.NewLine +
               "Number of times Room 6 was entered: " + PlayerPrefs.GetInt("Room6Entered") + Environment.NewLine +
               "Number of times Room 6 was failed: " + PlayerPrefs.GetInt("Room6Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 7 completed? " + completed[6] + Environment.NewLine +
               "Date Room 7 was completed: " + PlayerPrefs.GetString("Room7CompletedDate") + Environment.NewLine +
               "Number of times Room 7 was entered: " + PlayerPrefs.GetInt("Room7Entered") + Environment.NewLine +
               "Number of times Room 7 was failed: " + PlayerPrefs.GetInt("Room7Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 8 completed? " + completed[7] + Environment.NewLine +
               "Date Room 8 was completed: " + PlayerPrefs.GetString("Room8CompletedDate") + Environment.NewLine +
               "Number of times Room 8 was entered: " + PlayerPrefs.GetInt("Room8Entered") + Environment.NewLine +
               "Number of times Room 8 was failed: " + PlayerPrefs.GetInt("Room8Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 9 completed? " + completed[8] + Environment.NewLine +
               "Date Room 9 was completed: " + PlayerPrefs.GetString("Room9CompletedDate") + Environment.NewLine +
               "Number of times Room 9 was entered: " + PlayerPrefs.GetInt("Room9Entered") + Environment.NewLine +
               "Number of times Room 9 was failed: " + PlayerPrefs.GetInt("Room9Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 10 completed? " + completed[9] + Environment.NewLine +
               "Date Room 10 was completed: " + PlayerPrefs.GetString("Room10CompletedDate") + Environment.NewLine +
               "Number of times Room 10 was entered: " + PlayerPrefs.GetInt("Room10Entered") + Environment.NewLine +
               "Number of times Room 10 was failed: " + PlayerPrefs.GetInt("Room10Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 11 completed? " + completed[10] + Environment.NewLine +
               "Date Room 11 was completed: " + PlayerPrefs.GetString("Room11CompletedDate") + Environment.NewLine +
               "Number of times Room 11 was entered: " + PlayerPrefs.GetInt("Room11Entered") + Environment.NewLine +
               "Number of times Room 11 was failed: " + PlayerPrefs.GetInt("Room11Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 12 completed? " + completed[11] + Environment.NewLine +
               "Date Room 12 was completed: " + PlayerPrefs.GetString("Room12CompletedDate") + Environment.NewLine +
               "Number of times Room 12 was entered: " + PlayerPrefs.GetInt("Room12Entered") + Environment.NewLine +
               "Number of times Room 12 was failed: " + PlayerPrefs.GetInt("Room12Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 13 completed? " + completed[12] + Environment.NewLine +
               "Date Room 13 was completed: " + PlayerPrefs.GetString("Room13CompletedDate") + Environment.NewLine +
               "Number of times Room 13 was entered: " + PlayerPrefs.GetInt("Room13Entered") + Environment.NewLine +
               "Number of times Room 13 was failed: " + PlayerPrefs.GetInt("Room13Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 14 completed? " + completed[13] + Environment.NewLine +
               "Date Room 14 was completed: " + PlayerPrefs.GetString("Room14CompletedDate") + Environment.NewLine +
               "Number of times Room 14 was entered: " + PlayerPrefs.GetInt("Room14Entered") + Environment.NewLine +
               "Number of times Room 14 was failed: " + PlayerPrefs.GetInt("Room14Failed") + Environment.NewLine + Environment.NewLine +
               "Was Room 15 completed? " + completed[14] + Environment.NewLine +
               "Date Room 15 was completed: " + PlayerPrefs.GetString("Room15CompletedDate") + Environment.NewLine +
               "Number of times Room 15 was entered: " + PlayerPrefs.GetInt("Room15Entered") + Environment.NewLine +
               "Number of times Room 15 was failed: " + PlayerPrefs.GetInt("Room15Failed") + Environment.NewLine + Environment.NewLine +
               "Average session time: " + averageTimeSpan + Environment.NewLine +
               "Total session time: " + totalTimeSpan + Environment.NewLine +
               "Total time spent in encounters: " + totalEncounterTime + Environment.NewLine +
               "Total time spent in rooms: " + totalRoomTime + Environment.NewLine +
               "Total number of questions correct in a row: " + PlayerPrefs.GetInt("TotalInARowHighScore") + Environment.NewLine +
               "Total number of achievements unlocked: " + totalNumberOfUnlockedAchievements + "/" + totalNumberOfAchievements + Environment.NewLine +
               "Total number of achievement points earned: " + PlayerPrefs.GetInt("Points") + "/" + totalNumberOfPointsAvailable + Environment.NewLine +
               "Total number of damage taken: " + PlayerPrefs.GetInt("DamageTaken") + Environment.NewLine +
               "Average hp when leaving a room: " + averageHpLeavingRooms + Environment.NewLine +
               "plus any other data needed...";

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = body;
        mail.Attachments.Add(data1);
        mail.Attachments.Add(data2);
        mail.Attachments.Add(data3);
        mail.Attachments.Add(data4);
        
        // you can use others too.
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
        //smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, password) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);
    }
}
