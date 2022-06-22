//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DashMovement : MonoBehaviour
//{
//    public MovementV2 mvm2;
//    Vector3 dashPos;
//    [SerializeField]float DashDist = 1.2f;
    
//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            Dash();
//        }
//    }

//    public void Dash()
//    {
//        Debug.Log("Dash");
//        dashPos = gameObject.transform.position;
//        if (mvm2.facingRight)
//        {
//            dashPos.x += DashDist;
//        }
//        else
//        {
//            dashPos.x -= DashDist;
//        }
//        gameObject.transform.position = dashPos;
//    }
//}
