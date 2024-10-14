using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using Unity.VisualScripting;

public enum PlayerState 
{
    Flying,
    Resting,
    Falling,
}

public enum Walled
{
    Left,
    Right,
    None,
}

public class PlayerMovement : MonoBehaviour
{
    //----------Variables----------\\
    [SerializeField] PlayerState whatTheMothDoin;
    [SerializeField] Walled whichWallIsTheMothCliningTo;
    public Rigidbody rb;

    public float flapStrength;
    public float rotateStrength;
    public float maxSpeed;
    public float wallClingTollerance;
    [SerializeField] float stationaryTimer = 3f;

    //[SerializeField] bool grounded;
    //public Collider triggerCollider;
    public WallDetection wallDetectionHitBox;


    public Slider flapMeter;
    //----------Variables----------\\

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Note: I'm not using delta time because I want the movement to be consistant, and not change depending on frame rate (i think)
        //----------Movement_&_Controls----------\\
        if (flapMeter.value != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Right Down");
                rb.AddRelativeForce(new Vector3(0f, flapStrength, 0f), ForceMode.Impulse);
                rb.AddRelativeTorque(new Vector3(0f, 0f, -rotateStrength), ForceMode.Impulse);
                flapMeter.value -= 0.1f;


            }
            if (Input.GetMouseButtonDown(1))
            {
                //Debug.Log("Left Down");
                rb.AddRelativeForce(new Vector3(0f, flapStrength, 0f), ForceMode.Impulse);
                rb.AddRelativeTorque(new Vector3(0f, 0f, rotateStrength), ForceMode.Impulse);

                //update the flap meter
                flapMeter.value -= 0.1f;
            }
        }

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    rb.velocity = new Vector3(30f, 0f, 0f);
        //}

//-----------------------------------------------------------------------------------------------------------------------------------------\\


        // Code that checks the speed of the player, limiting their speed and re-rights the player if they are still for too long
        if (rb.velocity.magnitude <= 1.5f)
        {
            stationaryTimer -= Time.deltaTime;
            //Debug.Log(stationaryTimer);
        }
        else if (rb.velocity.magnitude > 1.5f && rb.velocity.magnitude <= maxSpeed)
        {
            stationaryTimer = 2f;
            //Debug.Log(stationaryTimer);
        }
        //Velocity limiter
        else if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            //Debug.Log("AHHHHHH");
        }
        if (stationaryTimer < 0f)
        {
            switch (whichWallIsTheMothCliningTo)
            {
                case Walled.Left:
                    rb.gameObject.transform.eulerAngles = new Vector3(0f, 0f, -15f);
                    break;

                case Walled.Right:
                    rb.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 15f);
                    break;

                case Walled.None:
                    rb.gameObject.transform.eulerAngles = Vector3.zero;
                    rb.velocity = Vector3.zero;
                    break;

            }
            
            flapMeter.value += 1f * Time.deltaTime;
        }

//-----------------------------------------------------------------------------------------------------------------------------------------\\

    }
    //if the player is on the ground, they will have more drag. meaning that they will slow down faster from rolling
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //grounded = true;
            //Debug.Log("im tired I stay on ground");
            rb.drag = 0.35f;
        }

        
    }
    //if the player hits a ceiling they will bounce off it and loose all their current flap meter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ceiling"))
        {
            //grounded = true;
            Debug.Log("ow my head");
            flapMeter.value = 0;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        // if the player leaves the ground, make the drag 0 again
        if (other.gameObject.CompareTag("Ground"))
        {
            //grounded = false;
            //Debug.Log("I am off the ground");
            rb.drag = 0f;

        }

        //if the player leaves a wall, if they were previously clinging onto it gravity will be activated again and they will fly normally.
        if (other.gameObject.CompareTag("Wall"))
        {
            rb.useGravity = true;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the player hits the ground they should bounce a decaying amount
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("Ouch");
            //Debug.Log(Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal));

            whichWallIsTheMothCliningTo = Walled.None;

            rb.velocity = Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal) * 0.2f;
            if (wallDetectionHitBox.playerBounceVelocity.magnitude <= 1.5f)
            {
                wallDetectionHitBox.playerBounceVelocity = Vector3.zero;
            }
            else
            {
                wallDetectionHitBox.playerBounceVelocity = wallDetectionHitBox.playerBounceVelocity * 0.5f;
            }
            //Debug.Log("This is the ground sir");


        }

        //If the player hits the wall, 2 things can happen based on their impact speed.

        //If the speed is slow enough, the player will cling onto the wall and slide down slowly, also allowing them to regain flap meter

        //If the speed is too fast, the player will bounce off and lose all their flap meter
        if (collision.gameObject.CompareTag("Wall"))
        {
            //what i wanna do is get the positional vector from the player to the wall and the x componenet of it;
            //if the x value is less than 0, the wall is to the left, if its greater than 0, the wall is to the right
            if(collision.gameObject.transform.position.x - rb.position.x < 0)
            {
                whichWallIsTheMothCliningTo = Walled.Left;
                Debug.Log(whichWallIsTheMothCliningTo);
            }
            if(collision.gameObject.transform.position.x - rb.position.x > 0)
            {
                whichWallIsTheMothCliningTo = Walled.Right;
                Debug.Log(whichWallIsTheMothCliningTo);
            }

            //Debug.Log("Ouch");
            //Debug.Log(Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal));
            if (wallDetectionHitBox.playerBounceVelocity.magnitude <= wallClingTollerance)
            {
                Debug.Log("i wanna cling onto the wall here");
                rb.velocity = Vector3.zero;
                stationaryTimer = 0.5f;
                rb.useGravity = false;
                rb.velocity = new Vector3(0, -1f, 0);
            }
            else
            {
                rb.velocity = Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal) * 0.2f;
                flapMeter.value = 0;
                Debug.Log("boyoyoyoing");
            }

            
        }

        //if the player hits a ceiling they will bounce off it and loose all their current flap meter
        if (collision.gameObject.CompareTag("Ceiling"))
        {
            Debug.Log("roooo rooo rooof");
            //Debug.Log(Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal));

            rb.velocity = Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal) * 0.2f;
        }
    }

}
