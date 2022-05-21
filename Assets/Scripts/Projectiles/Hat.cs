using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    Vector3 aim;
    Rigidbody rb;
    Vector3 worldPosition;
    public Transform pop;
    GameObject player;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
    public float speed;
    void FixedUpdate()
    {
        //var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.rotation = rot;
        //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        //rb.angularVelocity = Vector3.zero;

        //float input = Input.GetAxis("Vertical");
        //rb.AddForce(gameObject.transform.up * speed * input);
    }
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.rotation = Quaternion.Euler(pop.rotation.x, transform.rotation.y, transform.rotation.z);
        rb.velocity = mousePosition * speed;


    }
}
