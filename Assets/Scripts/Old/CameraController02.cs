using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController02 : MonoBehaviour
{
    private Transform target;
    private Vector3 offSet;

    [SerializeField]
    private float smoothSpeed = 0.15f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offSet = transform.position - target.position;
    }

    void Update()
    {
        Vector3 pos = target.position + offSet;
        transform.position = Vector3.Lerp(transform.position, pos, smoothSpeed);
    }
}
