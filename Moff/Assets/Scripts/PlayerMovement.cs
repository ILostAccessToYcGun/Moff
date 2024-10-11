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

public class PlayerMovement : MonoBehaviour
{
    //----------Variables----------\\
    [SerializeField] PlayerState whatTheMothDoin;
    public Rigidbody rb;

    public float flapStrength;
    public float rotateStrength;
    public float maxSpeed;
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
            rb.gameObject.transform.eulerAngles = Vector3.zero;
            rb.velocity = Vector3.zero;
            flapMeter.value += 1f * Time.deltaTime;
        }

//-----------------------------------------------------------------------------------------------------------------------------------------\\

    }
    //Ground Detection?/Wall Detection?
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //grounded = true;
            //Debug.Log("im tired I stay on ground");
            rb.drag = 0.35f;
        }

        
    }

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
        if (other.gameObject.CompareTag("Ground"))
        {
            //grounded = false;
            //Debug.Log("I am off the ground");
            rb.drag = 0f;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log(rb.velocity);

        //Basically, if we hit the ground, we still wanna bounce but it should decay
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ouch");
            //Debug.Log(Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal));

            rb.velocity = Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal) * 0.5f;
            if (wallDetectionHitBox.playerBounceVelocity.magnitude <= 1.5f)
            {
                wallDetectionHitBox.playerBounceVelocity = Vector3.zero;
            }
            else
            {
                wallDetectionHitBox.playerBounceVelocity = wallDetectionHitBox.playerBounceVelocity * 0.5f;
            }
            Debug.Log("This is the ground sir");


        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ceiling"))
        {
            Debug.Log("Ouch");
            //Debug.Log(Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal));

            rb.velocity = Vector3.Reflect(wallDetectionHitBox.playerBounceVelocity, collision.contacts[0].normal) * 0.5f;
        }
    }

}
