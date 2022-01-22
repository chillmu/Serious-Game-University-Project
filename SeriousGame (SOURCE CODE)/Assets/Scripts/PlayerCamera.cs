using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Camera cam; //The game camera

    /// <summary>
    /// Start function, set the camera reference in this script to the main game camera
    /// </summary>
    private void Start()
    {
        cam = Camera.main;
    }

    /// <summary>
    /// Sets the position of the camera to the position of the player so the camera will always follow the player
    /// </summary>
    private void Update()
    {
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f); //Make the camera follow the player
    }
}
