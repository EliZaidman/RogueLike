using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChecker : MonoBehaviour
{
    // Classes
    public RougeController con;
    public CameraFollow2D cam;

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
            if (cam.offset == new Vector3(90, cam.offset.y, cam.offset.z))
            {
            con.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            }
            else
            {
                con.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            }
            Debug.Log("Exit");
        }
    }


}
