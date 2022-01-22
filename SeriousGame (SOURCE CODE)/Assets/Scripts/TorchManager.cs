using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager : MonoBehaviour
{
    public string bookName;

    public Sprite completedTorchSprite;
    public Sprite incompletedTorchSprite;

    private void Start()
    {
        if(PlayerPrefs.GetInt(bookName) == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = completedTorchSprite;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = incompletedTorchSprite;
        }
    }
}
