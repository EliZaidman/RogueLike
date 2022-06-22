using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingBook : MonoBehaviour
{

    public Transform player;
    Rigidbody rb;

    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < 10.0)
        {
            //ChasePlayer();
        }
        /// transform.rotation = Quaternion.Euler(transform.position.x, 0, transform.position.z);
       rb.MovePosition(player.position / dist);
    }

    void ChasePlayer()
    {
        //nav.destination = player.position;
        
        Debug.Log("Chasing player");
    }
}