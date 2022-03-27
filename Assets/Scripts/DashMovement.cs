using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMovement : MonoBehaviour
{
    public RougeController Player;

    Vector3 dashPos;
    [SerializeField]float DashDist = 1.2f;
    public float dashTime = 1.000005f;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Dash();
        }
    }

    public void Dash()
    {
        dashPos.x = transform.position.x + DashDist;
        gameObject.transform.position = dashTime * Vector3.Lerp(gameObject.transform.position, dashPos, Time.deltaTime);
    }
}
