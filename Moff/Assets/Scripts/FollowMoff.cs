using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMoff : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + new Vector3(0f, 1f, -10f);
    }
}
