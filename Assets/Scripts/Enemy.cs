using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    public Quaternion wantedRotation;
    public Vector3 wantedPosition;

    Quaternion startRot;
    Vector3 startPos;
    float speed = 0.30f;
    public float timeCount = 0.0f;
    public float resetCounter = 0.0f;


    public bool attackingPlayer;
    private void Start()
    {
        player = GameObject.Find("Player 2D");
        startRot = transform.rotation;
        startPos = transform.position;
    }

    private void Update()
    {
        PlayerSide();

        if (attackingPlayer && timeCount <1)
        {

            SlashAttack();
        }

        if (timeCount > 1)
        {
            {
                attackingPlayer = false;
                Debug.Log("inside reset");
                timeCount = timeCount + Time.deltaTime;
                resetCounter = resetCounter + Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, startRot, resetCounter * speed);
                transform.position = Vector3.Lerp(transform.position, startPos, resetCounter * speed);
            }
        }
        if (resetCounter > 1)
        {
            timeCount = 0;
            resetCounter = 0;
        }
    }

    private void SlashAttack()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, wantedRotation, timeCount * speed);
        transform.position = Vector3.Lerp(transform.position, player.transform.position, resetCounter * speed);
        timeCount = timeCount + Time.deltaTime;
    }

    private void PlayerSide()
    {
        if (gameObject.transform.position.x > player.transform.position.x)
        {
            wantedRotation = Quaternion.Euler(wantedRotation.x, wantedRotation.y, 90);
           // Debug.Log("if");

        }
        else
        {
            wantedRotation = Quaternion.Euler(wantedRotation.x, wantedRotation.y, - 90);
            //Debug.Log("else");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            attackingPlayer = true;
        }
    }
}
