using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningSceneManager : MonoBehaviour
{
    Vector3 camEndPoint;
    Vector3 camStartPoint;
    Camera cam;

    Vector3 playerEndPoint;
    Vector3 playerStartPoint;
    public GameObject player;

    public Text textContainer;

    float t;
    float t2;
    float seconds = 24f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camStartPoint = cam.transform.position;
        camEndPoint = new Vector3(0f, -10.5f, -10f);

        playerStartPoint = player.transform.position;
        playerEndPoint = new Vector3(0f, -13.5f, 0f);

        StartCoroutine(StartText());
    }

    void Update()
    {
        t += Time.deltaTime / seconds;
        t2 += Time.deltaTime / (seconds - 2f);

        cam.transform.position = Vector3.Lerp(camStartPoint, camEndPoint, t);
        player.transform.position = Vector3.Lerp(playerStartPoint, playerEndPoint, t2);
    }

    IEnumerator StartText()
    {

        textContainer.text = "Welcome, brave knight...";

        yield return new WaitForSeconds(4f);

        textContainer.text = "You have arrived just in time...";

        yield return new WaitForSeconds(4f);

        textContainer.text = "An evil wizard has entered the library...";

        yield return new WaitForSeconds(4f);

        textContainer.text = "And now all of the books have come to life!";

        yield return new WaitForSeconds(4f);

        textContainer.text = "O Brave Knight, please get these books for me...";

        yield return new WaitForSeconds(4f);

        textContainer.text = "You will find them in each of the Rooms of my castle...";

        yield return new WaitForSeconds(4f);

        textContainer.text = "Good luck brave knight!";

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }
}
