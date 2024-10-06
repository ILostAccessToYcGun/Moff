using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

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
    public Collider triggerCollider;

    public Slider wingMeter;
    //----------Variables----------\\

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Note: I'm not using delta time because I want the movement to be consistant, and not change depending on frame rate (i think)
        //----------Movement----------\\
        if (wingMeter.value != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Right Down");
                //rb.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);
                rb.AddRelativeForce(new Vector3(0f, flapStrength, 0f), ForceMode.Impulse);
                rb.AddRelativeTorque(new Vector3(0f, 0f, -rotateStrength), ForceMode.Impulse);
                wingMeter.value -= 0.1f;


            }
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Left Down");
                rb.AddRelativeForce(new Vector3(0f, flapStrength, 0f), ForceMode.Impulse);
                rb.AddRelativeTorque(new Vector3(0f, 0f, rotateStrength), ForceMode.Impulse);
                wingMeter.value -= 0.1f;
            }
        }

        

        //Debug.Log(rb.rotation.eulerAngles.z);


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
            wingMeter.value += 1f * Time.deltaTime;
        }


    }
    //Ground Detection?/Wall Detection?
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //grounded = true;
            Debug.Log("im tired I stay on ground");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //grounded = false;
            Debug.Log("I am off the ground");
        }
    }
}
