using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body; //The rigidbody component of this GameObject
    float horizontal; //The float value of horizontal user input
    float vertical; //The float value of vertical user input
    float diagonalLimit = 0.7f; //A limiter for diagonal movement
    public float speed = 5.0f; //The speed the player should move at
    public bool isMoving = false; //Is the player moving?

    /// <summary>
    /// Start function, gets the Rigidbody component attached to this GameObject
    /// </summary>
    void Start()
    {
        //Get the Rigidbody2D of the gameObject
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Get horizontal and vertical input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }


    void FixedUpdate()
    {
        //If both inputs are 0, the player is not moving
        if(horizontal == 0 && vertical == 0)
        {
            isMoving = false;
        }

        //If both input are not 0, limit diagonal movement speed
        if(horizontal != 0 && vertical != 0)
        {
            horizontal *= diagonalLimit;
            vertical *= diagonalLimit;
        }

        //If either inputs are not 0, the player is moving
        if(horizontal != 0 || vertical != 0)
        {
            isMoving = true;
        }

        //set the velocity of the gameObject equal to input times speed
        body.velocity = new Vector2(horizontal * speed, vertical * speed);

        if(isMoving)
        {
            gameObject.GetComponent<Animator>().StopPlayback();
        }
        else
        {
            gameObject.GetComponent<Animator>().StartPlayback();
        }

        if(horizontal == 1)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        if(horizontal == -1)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
