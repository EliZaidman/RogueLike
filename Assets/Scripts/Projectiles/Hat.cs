using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {         
       gameObject.SetActive(false);
    }
    
    
    private void Update()
    {
        if (!gameObject)
        {

        }
    }
}
