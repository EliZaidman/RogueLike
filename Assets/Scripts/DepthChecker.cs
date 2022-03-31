using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChecker : MonoBehaviour
{

    // Classes
    public RougeController con;
    public CameraFollow2D cam;
    private Quaternion v3frontView = new Quaternion(0, -90, 0, 0);
    bool triggerOnce = false;
    public enum CurrnetView
    {
        FrontView = 6,
        SideView = 7,
        BackView = 8,
    }
    private void OnTriggerStay(Collider player)
    {
        triggerOnce = true;
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

            if (triggerOnce)
            {
                triggerOnce = false;
                if (player.gameObject.layer == 7)
                {
                    player.gameObject.layer = player.gameObject.layer = (int)CurrnetView.FrontView;
                }
                else
                {
                    player.gameObject.layer = (int)CurrnetView.SideView;
                    player.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, v3frontView, Time.deltaTime);
                }
                Debug.Log("Exit");
            }
        }


    }
}



