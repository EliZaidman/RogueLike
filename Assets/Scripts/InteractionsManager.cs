using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsManager : MonoBehaviour
{
    public bool timeIsSlowed;
    public bool swaped;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (timeIsSlowed)
        //{
        //    Time.timeScale = 0.3f;
        //}
        //else
        //{
        //    Time.timeScale = 1;

        //}
        swap();
    }

    void swap()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
        swaped = !swaped;
            if (swaped)
                timeIsSlowed = true;
            else
                timeIsSlowed = false;
        }
    }
}
