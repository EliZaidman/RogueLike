using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour, IPooledObject
{
    public float velocity = 100;
    RougeController rougeController;
    public void OnObjectSpawn()
    {
        //gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * velocity * 1000 * Time.deltaTime);
    }
    //private void Start()
    //{
    //    rougeController = GameObject.Find("Player 2D").GetComponent<RougeController>();
    //}
    //private void OnEnable()
    //{
    //    Debug.Log("Active");
    //    gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3
    //                                                 (-1500, 0, 0));
    //}
}
