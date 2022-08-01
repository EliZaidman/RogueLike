using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingBook : MonoBehaviour
{

    [SerializeField] float ChaseSpeed;
    [SerializeField] float RangeToFindPlayer;
    [SerializeField] float ShootCD;
    [SerializeField] float DelayPerShot;
    [SerializeField] float ShootingPlayerRange;
    float ShootCDTimer = 0;
    Transform player;
    bool left;
    Vector3 startpos;
    bool shooting = false;
    [SerializeField] List<GameObject> bullets;
    int wallDetectorRange = 2;
    int count = 0;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startpos = transform.position;
    }


    void Update()
    {
        LeftOrRight();
        FlockAI();
    }

    void FlockAI()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < RangeToFindPlayer && !isWallDetected())
        {
            ShootCDTimer += Time.deltaTime;
            Debug.Log("Player");
            if (left && !shooting)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(
                    player.position.x / 2,
                    transform.position.y,
                    transform.position.z),
                    Time.deltaTime / ChaseSpeed);
            }
            else
            {
                if (!shooting)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(
                    player.position.x * 2,
                    transform.position.y,
                    transform.position.z),
                    Time.deltaTime / ChaseSpeed);
                }

            }
            if (dist < ShootingPlayerRange && ShootCDTimer >= ShootCD)
            {
                StartCoroutine(IterateWithPause());
            }
        }
        else
        {
            transform.position = transform.position = Vector3.Lerp(transform.position, startpos, Time.deltaTime / 2);
        }
    }
    void LeftOrRight()
    {
        if (transform.position.x > player.transform.position.x)
        {
            left = true;
        }
        else
        {
            left = false;
        }
    }

    public IEnumerator IterateWithPause()
    {
        if (count < 3)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                count++;
                bullets[i].SetActive(true);
                yield return new WaitForSeconds(DelayPerShot);
            }
        }
        else if (count >= 3)
        {
            count = 0;
            ShootCDTimer = 0;
            print("zero");
        }
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
}