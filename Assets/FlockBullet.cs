using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBullet : MonoBehaviour
{
    private Transform player;
    private Vector3 lastPos;
    private GameObject Paranet;
    public float bulletSpeed;
    private int wallDetectorRange = 1;
    public int damage;
    public float bulletRotation;

    private void OnEnable()
    {
        Paranet = transform.parent.gameObject;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = Paranet.transform.position;
        lastPos = player.transform.position;
    }
    private void Update()
    {
        StartCoroutine(DeActivate());
        transform.position = Vector3.MoveTowards(transform.position, lastPos, Time.deltaTime * 10 * bulletSpeed);

        if (isWallDetected())
        {
            transform.parent = Paranet.transform;
            gameObject.SetActive(false);

        }
        //FailSafe
        if (transform.position.x == lastPos.x || transform.position.y == lastPos.y)
        {
            gameObject.SetActive(false);
            transform.parent = Paranet.transform;

        }
<<<<<<< Updated upstream
        transform.rotation =  Quaternion.Euler(transform.position.x, transform.position.y, +bulletRotation);
=======
        transform.rotation = Quaternion.Euler(transform.position.x, transform.position.y, +bulletRotation);
>>>>>>> Stashed changes
    }

    IEnumerator DeActivate()
    {
        transform.parent = null;
        yield return new WaitForSeconds(3);
        transform.parent = Paranet.transform;
        gameObject.SetActive(false);

    }

    private bool isWallDetected()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, wallDetectorRange);
        foreach (Collider collider in colls)
        {
            if (collider.tag == "Platform") return true;
        }
        return false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            transform.parent = Paranet.transform;
            collision.collider.GetComponent<HPSystem>().TakeDamage(damage);
        }
    }
}