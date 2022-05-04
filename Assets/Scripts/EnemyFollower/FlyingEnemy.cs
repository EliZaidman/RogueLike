using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    public bool chase = false;
    public bool isPlayerInRange;
    public float detectionRange;
    public LayerMask Layer;
    Collider[] colliders;
    public Transform startingPoint;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, detectionRange, Layer);
        if (player == null)
            return;
        if (colliders.Length != 0)
            Chase();
        else
            ReturnStartPoint();
        Flip();
    }
    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        //if (Vector3.Distance(transform.position, player.transform.position) <= 0.5f)
        //{
            //change speed, shoot, animation
        //}
        //else
        //{
            //reset values
        //}
    }
    private void ReturnStartPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }
    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);

        //gameObject.transform.LookAt(player.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
