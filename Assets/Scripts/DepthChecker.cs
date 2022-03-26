using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChecker : MonoBehaviour
{
    // Classes
    public RougeController con;

    private void OnTriggerStay(Collider player)
    {
        if (player.tag == "Player")
        {
            con.canGoDeep = true;
            con.rb.constraints = RigidbodyConstraints.None;
            con.rb.constraints = RigidbodyConstraints.FreezeRotation;
            Debug.Log("insde");
        }
    }
    private void OnTriggerExit(Collider player)
    {
        if (player.tag == "Player")
        {
            con.canGoDeep = false;
            con.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            
            Debug.Log("Exit");
        }
    }
}
