using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMonitor : MonoBehaviour
{
    public bool sentinel = false;
    CameraFollow2D cameraFollow;
    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow2D>();
    }
    private void OnTriggerEnter(Collider player)
    {
        if (!sentinel)
        {
            sentinel = true;
            if (cameraFollow.offset == cameraFollow.frontView)
            {
                Camera.main.fieldOfView = 20;
                cameraFollow.offset = cameraFollow.sideView;
                Debug.Log("FrontView");
            }

            else if (cameraFollow.offset == cameraFollow.sideView)
            {
                Camera.main.fieldOfView = 115;
                cameraFollow.offset = cameraFollow.frontView;
                Debug.Log("SideView");
            }
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (sentinel)
            {
                sentinel = false; //Allows for another object to be struck by this one
            }
        }
    }
}
