using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthChecker : MonoBehaviour
{

    // Classes
    public RougeController con;

    private void OnTriggerStay(Collider other)
    {
        con.canGoDeep = true;
    }

    private void OnTriggerExit(Collider other)
    {
        con.canGoDeep = false;
    }
}



