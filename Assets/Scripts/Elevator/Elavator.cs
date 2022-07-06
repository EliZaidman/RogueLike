using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elavator : MonoBehaviour
{

    Rigidbody rb;
    public float speed;
    bool goingUp;
    bool canCallElev;

    [SerializeField]
    private Transform origin, _target;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (goingUp == true && canCallElev)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
            print("Going up");
        }
        else if (goingUp == false && canCallElev)
        {
            transform.position = Vector3.MoveTowards(transform.position, origin.position, speed * Time.deltaTime);
            print("Going Down");

        }
    }
    private void OnTriggerStay(Collider other)
    {
        print("InsideCollider");
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canCallElev = true;
                UseElavator();
            }
        }
    }

    private void UseElavator()
    {
        goingUp = !goingUp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canCallElev = false;
        if (other.tag == "Player")
        {
            other.transform.parent = null;

        }
    }
}
