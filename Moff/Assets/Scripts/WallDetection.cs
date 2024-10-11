using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    public PlayerMovement player;
    public Vector3 playerBounceVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
        // I have moved this code outsid eth if statement so now the player can bounce on the ground !!! big pog
        playerBounceVelocity = player.rb.velocity;

        Debug.Log("THERES A WALL BRO");



        if (other.gameObject.CompareTag("Wall"))
        {

            //Im having a lot of trouble with this. The main issue is that "OnTriggerEnter isn't fast enough.
            //Meaning that by the time the code activates, the player's velocity is already 0 as they have hit the wall.
            //I can work around this by making the collider larger however this will only look good at high speeds.
            //I have tried changing it to "OnCollision: but for some reason nothing was being picked up.


            //Vector3 collisionPoint = other.ClosestPoint(player.rb.transform.position);
            //Debug.Log("collisionPoint= " + collisionPoint);

            //Vector3 collisionNormal = player.rb.transform.position - collisionPoint;
            //Debug.Log("collisionNormal= " + collisionNormal);
            //Debug.Log(Vector3.Reflect(player.rb.velocity, collisionNormal));

            //player.rb.velocity = Vector3.Reflect(player.rb.velocity, collisionNormal);


            //okay, what im doing now is im making a large detection area that will start tracking the player velocity very far from when they hit the wall,
            //this means that the player should never just stop against a wall.

            


            //player.flapMeter.value = 0f;
            //Debug.Log(player.rb.velocity * -1f);
            //player.rb.velocity = player.rb.velocity * -0.25f;

            //Debug.Log("bro thats a frogging wall slow down");

        }
    }

    //--------------------------------------------------------------------------THIS DIDNT WORK---------------------------------
    //void OnCollisionEnter(Collision collision)
    //{
    //    //player.rb.AddForce(player.rb.velocity, collision.contacts[0].normal);
    //    Debug.Log("reeeee");
    //    if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        player.rb.velocity = Vector3.Reflect(player.rb.velocity, collision.contacts[0].normal);
    //    }

    //}
}
