using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayHiragana : MonoBehaviour
{
    HiraganaList hiraganaList;
    List<Hiragana> listOfHiragana;
    List<Hiragana> workingList;

    public Text textField;
    public GameObject hiraganaParent;
    public GameObject hiraganaPrefab;
    public Button returnButton;

    private void Start()
    {
        returnButton.onClick.AddListener(delegate { ReturnToRoom(); });

        hiraganaList = gameObject.GetComponent<HiraganaList>();
        hiraganaList.GenerateList();
        listOfHiragana = hiraganaList.GetList();

        workingList = new List<Hiragana>();

        List<int> levels = new List<int>();

        //int level = PlayerPrefs.GetInt("DisplayTheseCharacters");

        if(PlayerPrefs.GetInt("DisplayThisSet1") != 0)
        {
            levels.Add(PlayerPrefs.GetInt("DisplayThisSet1"));
        }

        if(PlayerPrefs.GetInt("DisplayThisSet2") != 0)
        {
            levels.Add(PlayerPrefs.GetInt("DisplayThisSet2"));
        }

        foreach(Hiragana h in listOfHiragana)
        {
            if(levels.Contains(h.GetLevel()))
            {
                workingList.Add(h);
            }
        }    

        StartCoroutine(LoopHiragana());
    }

    IEnumerator LoopHiragana()
    {
        foreach(Hiragana h in workingList)
        {
            GameObject o = Instantiate(hiraganaPrefab, hiraganaParent.transform);
            Text text = o.GetComponentInChildren<Text>();
            text.text = h.GetCharacter() + " = " + h.GetSound();
            PlaySound(h.GetSound());

            yield return new WaitForSeconds(2f);
        }

        returnButton.GetComponentInChildren<Text>().text = "Start Level";
    }

    void PlaySound(string sound)
    {
        string path = "Audio/" + sound;

        if(gameObject.GetComponent<AudioSource>())
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            audioSource.PlayOneShot(audioClip);
        }
    }

    void ReturnToRoom()
    {
        string returnRoom = PlayerPrefs.GetString("GoBackToThisRoom");
        PlayerPrefs.SetString("DisplayHiragana" + returnRoom, "false");
        PlayerPrefs.Save();
        SceneManager.LoadScene(returnRoom, LoadSceneMode.Single);
    }
}
